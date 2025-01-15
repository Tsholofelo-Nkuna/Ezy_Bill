using Core.Presentation.Models;
using Core.Presentation.Models.DataTransferObjects.Base;
using Core.Presentation.Models.Interfaces;
using Core.Presentation.ViewComponents.Components.Base;
using Core.Presentation.ViewComponents.Interfaces;
using Core.Presentation.ViewComponents.Interfaces.Base;
using Microsoft.AspNetCore.Components;
using System;

namespace Core.Presentation.ViewComponents.Components
{
    public partial class FormComponent<TRecord>: GenericComponentBase<FormComponentViewModel<TRecord>, TRecord> where TRecord : BaseDto, new()
    {

        [Parameter] 
        public EventCallback<IEnumerable<TRecord>> ViewModelStateChanged { get; set; } = new EventCallback<IEnumerable<TRecord>>();
        [Parameter, Obsolete("Use OnFormSubmitClicked instead")]
        public Func<IEnumerable<TRecord>, Task>? OnFormSubmitClick { get; set; }
        [Parameter]
        public Func<EventState<IEnumerable<TRecord>>, Task>? OnFormSubmitClicked { get; set; }
        [Parameter]
        public Action<KeyValuePair<string, string>?>? OnValidationFail { get; set; }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.ViewModel.AddViewModelStateChangeListener(this.OnViewModelStateChanged); 
        }
      

        public override async Task OnViewModelStateChanged(IEnumerable<TRecord> update)
        {
            await base.OnViewModelStateChanged(update);
            await ViewModelStateChanged.InvokeAsync(update);
            
        }

        public void OnInputChange(Guid id, InputFieldViewModel<TRecord> field, object? value)
        {
           
         if(field.Validator is { Validators : IEnumerable<IValidatorFn>})
            {
                ViewModel.Set(id, field.Name, value);
                var validationResult = field
                    .Validator
                    .Validators.Select(v => v.Validator(value));

                if(validationResult.All(x => x is null))
                {
                    field.ValidationMessage = string.Empty;
                   
                }
                else
                {
                    var vResult = validationResult.FirstOrDefault(x => x is not null);
                    field.ValidationMessage = vResult?.Value ?? string.Empty;
                    if (this.OnValidationFail is not null)
                    {
                        this.OnValidationFail(vResult);
                    }
                }
              
                
            }
            else
            {
                ViewModel.Set(id, field.Name, value);
            }
        }

        public void OnMultiSelectInputChange(Guid recordId, InputFieldViewModel<TRecord> field, object? isChecked, KeyValuePair<string, string> sourceItem )
        {
            var sourceRecord = this.ViewModel
                   .ViewModelState.FirstOrDefault(x => x.Id == recordId);
            var targetProp = sourceRecord?.GetType()?.GetProperty(field.Name);
            if (targetProp is { CanRead: true, CanWrite: true } validTargetProp && validTargetProp.PropertyType == typeof(IEnumerable<string>))
            {
                var propVal =  targetProp.GetValue(sourceRecord) ?? Enumerable.Empty<string>();
                if(propVal is IEnumerable<string> strCollection)
                {
                    if (isChecked is true && !strCollection.Contains(sourceItem.Key))
                    {
                        strCollection = strCollection.Append(sourceItem.Key);
                    }
                    else if(isChecked is false && strCollection.Contains(sourceItem.Key))
                    {
                        strCollection = strCollection.Where(x => x != sourceItem.Key);
                    }

                    this.OnInputChange(recordId, field, strCollection);
                }
            }
           
        }

        public Task OnFormSubmit()
        {
           
            if (this.ViewModel.Validate())
            {
              //model is valid
            }
            else
            {
               
                //model is invalid
            }
            OnFormSubmitClick?.Invoke(this.ViewModel.ViewModelState);
            OnFormSubmitClicked?.Invoke(new EventState<IEnumerable<TRecord>> { Success = true, Payload = this.ViewModel.ViewModelState });
            return Task.CompletedTask;
        }


        //public string Get(string key)
        //{
        //    var returned = this.Model.Get(key);
        //    return 
        //}

        ~FormComponent()
        {
           this.ViewModel.ClearViewModelStateChangeListeners();
        }
    }
}
