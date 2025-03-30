using BootstrapBlazor.Components;
using Core.Presentation.Models;
using Core.Presentation.Models.DataTransferObjects;
using Core.Presentation.Models.DataTransferObjects.Base;
using Core.Presentation.ViewComponents.Components.Base;
using Core.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Net.Http.Json;

namespace Core.Presentation.ViewComponents.Components
{
  
    public partial class TableComponent<TRecordType> : GenericComponentBase<TableComponentViewModel<TRecordType>, TRecordType>, IFilterable<TRecordType, TRecordType>
        where TRecordType : BaseDto, new()

    {
        [Parameter]
        public Func<Guid, Task>? OnViewClick { get; set; }
        [Parameter, Obsolete("Use OnDeleteClicked instead")]
        public Func<bool, Task>? OnDeleteClick { get; set; }
        [Parameter]
        public Func<EventState<TRecordType?>, Task>? OnDeleteClicked { get; set; }
        [Parameter]
        public Func<bool , Task>? OnArchiveClick { get; set; }
        [Parameter, Obsolete("Use OnNewClicked instead")]
        public Func<Task>? OnNewClick { get; set; }
        [Parameter]
        public Func<EventState<TRecordType?>, Task>? OnNewClicked { get; set; }
        [Parameter]
        public Func<EventState<TRecordType?>, Task>? OnSaveUpdatesClick { get; set; }
        [Parameter]
        public bool IsLoading { get; set; }
        [Parameter] 
        public EventCallback<bool> IsLoadingChanged { get; set; }
        [Parameter, Obsolete("Use IsLoadingChanged")]
        public EventCallback<bool> IsLoaddingChanged { get; set; }

        private TRecordType? _updatedRecordStateBeforeEdit;

        [Inject]
        private IConfiguration _appSettings { get; set; }
       
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            this.ViewModel.ColumnConfigs.Add(new TableComponentColumnConfig<TRecordType> { Name = _actionColumnName });
            this.ViewModel.ColumnConfigs.ForEach(cConfig =>
            {
                if(cConfig.EditInputFieldViewModel is not null)
                {
                    cConfig.EditInputFieldViewModel.Label = string.Empty; //Edit fields used during inline edits shouldn't have labels set
                }
            });
           int.TryParse(this._appSettings["Pagination:PageSize"], out var pSize);
           this.ViewModel.PageRequest.PageSize = pSize;
           await this.GetPageData(this.ViewModel.PageRequest);
        }

        //protected override async Task OnAfterRenderAsync(bool firstRender)
     
        //{
        //    await base.OnAfterRenderAsync(firstRender);
        //    var bDimenstions = await this.JS.InvokeAsync<BrowserDimensions>("getWindowDimenstions", null);
        //    this.ViewModel.InnerHeight = bDimenstions.InnerHeight;
        //    this.ViewModel.InnerWidth = bDimenstions.InnerWidth;
        //    //if (firstRender)
        //    //{
        //    //    StateHasChanged();
        //    //}

        //}
        public void OnView(Guid recordId) {
            OnViewClick?.Invoke(recordId);
        }

        private bool _shouldAllowInlineEdit => ViewModel.ColumnConfigs.Any(x => x.EditInputFieldViewModel is not null);
        private bool _showInlineSave {get;set;}
        private int _editIndex = -1;
        private string _actionColumnName = $"{nameof(TableComponent<TRecordType>)}DefaultActions";
        [Parameter]
        public int EditIndex
        {
            get => _editIndex; 
            set
            {
                _editIndex = value;

            }
        }

        [Parameter]
        public EventCallback<int> EditIndexChanged { get; set; }
        [Parameter]
        public TRecordType Filter { get; set; } = new TRecordType();

        public async Task OnPageIndexChange(int pageIndex)
        {
            ViewModel.PageRequest.PageIndex = pageIndex -1;
            await this.GetPageData(ViewModel.PageRequest);
            await Task.CompletedTask;
        }
        public async Task OnDelete(Guid id)
        {
            var deleteEventState = new EventState<TRecordType?> { Success = false, Payload = ViewModel.ViewModelState.FirstOrDefault(x => x.Id == id) };
            if (!string.IsNullOrWhiteSpace(this.ViewModel.DeleteController))
            {
                var response = await this.AppApi.DeleteAsync($"api/{this.ViewModel.DeleteController}/{this.ViewModel.DeleteAction}/{id}");
             
                if (response.IsSuccessStatusCode)
                {
                    var responseResult = await response.Content.ReadFromJsonAsync<bool>();
                    deleteEventState.Success = responseResult;
                    OnDeleteClick?.Invoke(responseResult);
                }
                else
                {
                    deleteEventState.Success = false;
                   OnDeleteClick?.Invoke(false);
                }
            }
            else
            {
                deleteEventState.Success = false;
                OnDeleteClick?.Invoke(false);
            }
            OnDeleteClicked?.Invoke(deleteEventState);
        }

        public async Task OnArchive(Guid id)
        {
            if (!string.IsNullOrWhiteSpace(this.ViewModel.ArchiveController))
            {
                var response = await this.AppApi.GetAsync($"api/{this.ViewModel.ArchiveController}/{this.ViewModel.ArchiveAction}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    OnArchiveClick?.Invoke(await response.Content.ReadFromJsonAsync<bool>());
                }
                else
                {
                    OnArchiveClick?.Invoke(false);
                }
            }
            else
            {
                OnArchiveClick?.Invoke(false);
            }

        }

        public async Task OnUpdate(Guid id)
        {
            _updatedRecordStateBeforeEdit = null;
            var updated = ViewModel.ViewModelState.FirstOrDefault(x => x.Id == id);
            var eventState = new EventState<TRecordType?> { Payload = updated };
            if (!string.IsNullOrWhiteSpace(this.ViewModel.SaveOrUpdateController))
            {
                if (updated is TRecordType)
                {
                    var response = await this.AppApi.PostAsJsonAsync(
                   $"api/{this.ViewModel.SaveOrUpdateController}/{this.ViewModel.SaveOrUpdateAction}",
                     updated);
                    if (response.IsSuccessStatusCode && await response.Content.ReadFromJsonAsync<bool>())
                    {
                        eventState.Success = true;
                      
                    }
                    else
                    {
                        eventState.Success = false;
                      
                    }
                }
                else
                {
                    eventState.Success = false;
                   
                }
            }
            else
            {
                eventState.Success = false;
            }
            this.OnSaveUpdatesClick?.Invoke(eventState);
        }

        public async Task OnCancelUpdate()
        {
            EditIndex = -1;
            await EditIndexChanged.InvokeAsync(EditIndex);
            if (_updatedRecordStateBeforeEdit is TRecordType formerRecordState)
            {
                this.RestoreRecordState(formerRecordState);
                _updatedRecordStateBeforeEdit = null;
            }
           
        }

        private void RestoreRecordState(TRecordType priorStateOfRecord)
        {
            this.ViewModel.ViewModelState = this.ViewModel.ViewModelState
                .Select(rec =>
                {
                    if(rec.Id == priorStateOfRecord.Id)
                    {
                        rec = priorStateOfRecord.Clone<TRecordType>();
                    }
                    return rec;
                });
        }
        public async Task OnEditClicked(int editIndex, TRecordType targetRecord)
        {
            EditIndex = editIndex;
            if(_updatedRecordStateBeforeEdit is not null)
            {
                this.RestoreRecordState(_updatedRecordStateBeforeEdit);
            }
            await EditIndexChanged.InvokeAsync(EditIndex);
            if (targetRecord is not null)
            {
                _updatedRecordStateBeforeEdit = targetRecord.Clone<TRecordType>();
            }
           
        }

        public Task OnCreateNew()
        {
            this.OnNewClick?.Invoke();
            this.OnNewClicked?.Invoke(new EventState<TRecordType?> { Success = true, Payload = null });
            return Task.CompletedTask;
        }

        public async Task<PageResponseDto<TRecordType>?> GetPageData(PageRequestDto<TRecordType> pageRequest)
        {
            IsLoading = true;
            if(pageRequest.PageSize <= 0)
            {
                pageRequest.PageSize = ViewModel.PageRequest.PageSize;
            }
            ViewModel.PageRequest = pageRequest;
            await IsLoadingChanged.InvokeAsync(IsLoading);
            StateHasChanged();
           
            if (!string.IsNullOrWhiteSpace(ViewModel.GetDataController))
            {
                var fetchUrl = $"api/{ViewModel.GetDataController}/{ViewModel.GetDataAction}";
                var requestResponse = await this.AppApi.PostAsJsonAsync(fetchUrl, pageRequest);

                if (requestResponse.IsSuccessStatusCode)
                {
                    ViewModel.PageResponse = (await requestResponse.Content.ReadFromJsonAsync<PageResponseDto<TRecordType>>());
                    ViewModel.ViewModelState = ViewModel.PageResponse?.Items ?? Enumerable.Empty<TRecordType>();
                }

            }
          
          
            IsLoading = false;
            await IsLoadingChanged.InvokeAsync(IsLoading);
            StateHasChanged();
            return ViewModel.PageResponse;
        }

        [Obsolete("Use GetPageData instead")]
        public async Task<IEnumerable<TRecordType>> GetData(TRecordType filters)
        {
            IsLoading = true;
            await IsLoaddingChanged.InvokeAsync(IsLoading);
            await IsLoadingChanged.InvokeAsync(IsLoading);
            StateHasChanged();
            var returnedResult = Enumerable.Empty<TRecordType>();
            if (!string.IsNullOrWhiteSpace(ViewModel.GetDataController))
            {
                var fetchUrl = $"api/{ViewModel.GetDataController}/{ViewModel.GetDataAction}";
                var requestResponse = await this.AppApi.PostAsJsonAsync(fetchUrl, filters);
                
                if (requestResponse.IsSuccessStatusCode)
                {  
                    returnedResult = (await requestResponse.Content.ReadFromJsonAsync<IEnumerable<TRecordType>>()) ?? Enumerable.Empty<TRecordType>();
                    
                }
              
            }
            ViewModel.ViewModelState = returnedResult;
            IsLoading = false;
            await IsLoaddingChanged.InvokeAsync(IsLoading);
            StateHasChanged();
            return returnedResult;
        }

        public TableComponent() { }



    }
}
