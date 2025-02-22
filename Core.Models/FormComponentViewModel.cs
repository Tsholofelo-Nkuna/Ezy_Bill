
using Core.Presentation.Models.DataTransferObjects.Base;
using Core.Presentation.Models.Base;
using System.Reflection;

namespace Core.Presentation.Models
{

    public class FormComponentViewModel<TRecordType> : GenericListViewModel<TRecordType> where TRecordType : BaseDto, new()
    {
        public FormComponentViewModel() : this(Enumerable.Empty<TRecordType>(), string.Empty)
        {

        }
        public FormComponentViewModel(IEnumerable<TRecordType> vModelState, string formName) : base(vModelState) {
            this.FormName = formName;
        }

        public List<InputFieldViewModel<TRecordType>> Fields { get; set; } = new List<InputFieldViewModel<TRecordType>>();
        public string ColClass { get; set; } = "col-md-6 col-lg-4";
        public string SubmitButtonText { get; set; } = "Submit";
        public string ActionName { get; set; } = string.Empty;
        public string FormName { get; set; }
        public string ControllerName { get; set; } = string.Empty;
        public bool CollapseFooter { get; set; }
        public bool IncludeFormStatePropsAsHidden { get; set; }
        public bool Validate()
        {
            this.ViewModelState.ToList()
                .ForEach(rec =>
                {
                    this.Fields.ForEach(field =>
                    {
                        var vResult = field.Validator?.Validate(rec);
                        field.ValidationMessage = vResult?.Value ?? string.Empty;
                    });
                });
            return IsValid;
        }

        public void Reset()
        {
            this.Fields.ForEach(field => { 
              field.ValidationMessage = string.Empty;
            });
            this.ViewModelState = this.ViewModelState.Select(x => new TRecordType());
           
        }
        public bool IsValid
        {
            get
            {
                var validators = this.Fields
                    .Where(x => x.Validator != null)
                    .Select(x => x.Validator);
                    
                if (validators.Any())
                {
                   var validationResults = this.ViewModelState
                        .SelectMany(record =>
                        {
                            return validators.Select(validatorFn => validatorFn?.Validate(record));
                        });
                    
                    return validationResults.All(r => r is null);
                }
                else
                {
                    return false;
                }
            }
        }

    }
}
