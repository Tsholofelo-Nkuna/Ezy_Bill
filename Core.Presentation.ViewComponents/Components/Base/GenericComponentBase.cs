using Core.Presentation.Models.DataTransferObjects.Base;
using Core.Presentation.Models.Interfaces.Base;
using  Core.Presentation.Models.Base;
using Core.Presentation.ViewComponents.Interfaces;
using Core.Presentation.ViewComponents.Interfaces.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Security.Principal;
using Core.Utils.Constants;

namespace Core.Presentation.ViewComponents.Components.Base
{
    public abstract class GenericComponentBase<TViewModel, TRecordType> : ComponentBase, 
        IGenericComponentBase<TViewModel, TRecordType>
      
            where TViewModel : IGenericListViewModel<TRecordType>, new()
            where TRecordType : BaseDto, new()
    {
        [Parameter]
        public TViewModel ViewModel { get; set; }
        [Parameter]
        public virtual IEnumerable<string> CssClassList { get; set; } = ["position-absolute","top-0","start-50 start-xs-0"];

        public string CssClass => string.Join(" ", CssClassList);

        [Inject]
        public NavigationManager NavManager { get; set; }
        [Inject] private IHttpClientFactory _httpClientFactory {  get; set; }
        [Inject] private IHttpContextAccessor _httpContextAccessor { get; set; }
      
        public HttpClient AppApi
        {
            get
            {
                var httpC = _httpClientFactory.CreateClient("AppApi");
               
                httpC.DefaultRequestHeaders.Add(AuthConstants.XApiKey, $"{this.CurrentUser.Identity.Name}");
                return httpC;
            }
        }
        public ClaimsPrincipal? CurrentUser => _httpContextAccessor.HttpContext?.User;
        public IEnumerable<string> BreadcrumbItems {
            get
            {
              var path = Regex.Match(this.NavManager.Uri, $@"(?<={this.NavManager.BaseUri}).+");
              return (path?.Success ?? false) ? path.Value.Split("/", StringSplitOptions.RemoveEmptyEntries) : Enumerable.Empty<string>();
            }
        }
        public virtual string BaseUrl { get; set; } = string.Empty;
        [Inject] public IJSRuntime JS { get; set; }
        public GenericComponentBase() : this(new TViewModel()) { }
        public GenericComponentBase(TViewModel viewModel)
        {
            ViewModel = viewModel;
            viewModel.OnViewModelStateChangedEvent += this.OnViewModelStateChanged;
           

        }

        //protected override Task OnInitializedAsync()
        //{
        //    var returned =  base.OnInitializedAsync();
        //    if(!(this.CurrentUser?.Identity?.IsAuthenticated ?? false))
        //    {
        //        this.NavManager.NavigateTo("Accounts/Login", true);
        //    }
        //    return returned;
        //}

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (!(this.CurrentUser?.Identity?.IsAuthenticated ?? false))
            {
                //this.NavManager.NavigateTo(LoginPathConstants.Login, true);
            }
        }

        public virtual void OnNavigate(string controllerName, string actionName, Guid stateId)
        {
            var baseUrl = string.IsNullOrEmpty(controllerName) ? "/" : "";
            NavManager.NavigateTo($"{controllerName}/{actionName}/{stateId}");
           
        }

        public virtual Task OnViewModelStateChanged(IEnumerable<TRecordType> update)
        {
           StateHasChanged();
           return Task.CompletedTask;
        }


        public IEnumerable<KeyValuePair<string, object?>> RecordAsKeyValuePairs(TRecordType record) =>
             typeof(TRecordType)
                .GetProperties()
                .Where(x => x is PropertyInfo { CanRead: true } and { GetMethod.IsPublic: true })
                .Select(pInfo => new KeyValuePair<string, object>(pInfo.Name, pInfo.GetValue(record)));

      

        ~GenericComponentBase()
        {
            this.ViewModel.OnViewModelStateChangedEvent-= this.OnViewModelStateChanged;
        }

    }
}
