using ClientManagement.Models.DataTransferObjects.Base;
using ClientManagement.Models.Interfaces;
using ClientManagement.Models.Interfaces.Base;
using ClientManagement.Models.Validation;
using ClientManagement.Models.Validation.Base;

namespace ClientManagement.Models
{
   public enum ControlType
    {
        //[Obsolete("Use the \"Text\" option instead")]
        //Input,
        Select,
        Checkbox,
        Date,
        Text,
        Email,
        MultiSelect,
        DateRange
    }

    public  class InputType
    {
        public static string Text = "text";
        public static string Checkbox = "checkbox";
        public static string Email = "email";
        public static string Password = "password";
        public static string Date = "date";
    }
    public class InputFieldViewModel<TRecord> where TRecord: BaseDto, new()
    {
        private  ControlType _controlType = ControlType.Text;
        public InputFieldViewModel(string name, string label, IValidatorBase<TRecord>? validator = null) {
            this.Name = name;
            this.Label = label;
            this.Validator = validator;
        }
        public string ValidationMessage { get; set; } = string.Empty;
        public ControlType ControlType 
        {
            get => _controlType;
            set
            {
                _controlType = value;
                
                if (_controlType == ControlType.Date) {
                    if (this.Validator is { Validators : IEnumerable<IValidatorFn>}) {
                        this.Validator.Validators = this.Validator.Validators
                            .Append(Validators.Date());
                    }
                    else if(this.Validator is null)
                    {
                        this.Validator = new ValidatorBase<TRecord>(new[] { Validators.Date() }, this.Name);
                    }
                }
                else if(_controlType == ControlType.Email)
                {
                    if (this.Validator is { Validators: IEnumerable<IValidatorFn> })
                    {
                        this.Validator.Validators = this.Validator.Validators
                            .Append(Validators.Email());
                    }
                    else if (this.Validator is null)
                    {
                        this.Validator = new ValidatorBase<TRecord>(new[] { Validators.Email() }, this.Name);
                    }
                }
               
            }
        }
        public string Label { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty ;
        public IValidatorBase<TRecord>? Validator { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Options { get; set; } = Enumerable.Empty<KeyValuePair<string, string>>();
        public bool SearchBoxOptionsVisible { get; set; }


    }
}
