using Core.Presentation.Models;
using Core.Presentation.Models.Base;
using Core.Presentation.Models.DataTransferObjects.Base;
using Core.Presentation.Models.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.ViewComponents.Components
{
    public partial class InputComponent<TRecord> where TRecord: BaseDto, new()
    {
        

        [Parameter]
        public InputFieldViewModel<TRecord>? ViewModel { get; set; }
        [Parameter]
        public TRecord ViewModelState
        {
            get => _genericListViewModel.ViewModelState.FirstOrDefault() ?? new TRecord();
            set
            {
                _genericListViewModel.ViewModelState = (value is not null) ? new[] {value} : new[] {new TRecord()};
            }
        }

        [Parameter]
        public Action<KeyValuePair<string, string>?>? OnValidationFail { get; set; }

        private GenericListViewModel<TRecord> _genericListViewModel = new GenericListViewModel<TRecord>(Enumerable.Empty<TRecord>());
        public void OnInputChange(Guid id, InputFieldViewModel<TRecord> field, object? value)
        {

            if (field.Validator is { Validators: IEnumerable<IValidatorFn> })
            {
                _genericListViewModel.Set(id, field.Name, value);
                var validationResult = field
                    .Validator
                    .Validators.Select(v => v.Validator(value));

                if (validationResult.All(x => x is null))
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
                _genericListViewModel.Set(id, field.Name, value);
            }
        }

        public void OnMultiSelectInputChange(Guid recordId, InputFieldViewModel<TRecord> field, object? isChecked, KeyValuePair<string, string> sourceItem)
        {
            var sourceRecord = this.ViewModelState;
            var targetProp = sourceRecord?.GetType()?.GetProperty(field.Name);
            if (targetProp is { CanRead: true, CanWrite: true } validTargetProp && validTargetProp.PropertyType == typeof(IEnumerable<string>))
            {
                var propVal = targetProp.GetValue(sourceRecord) ?? Enumerable.Empty<string>();
                if (propVal is IEnumerable<string> strCollection)
                {
                    if (isChecked is true && !strCollection.Contains(sourceItem.Key))
                    {
                        strCollection = strCollection.Append(sourceItem.Key);
                    }
                    else if (isChecked is false && strCollection.Contains(sourceItem.Key))
                    {
                        strCollection = strCollection.Where(x => x != sourceItem.Key);
                    }

                    this.OnInputChange(recordId, field, strCollection);
                }
            }

        }



    }
}
