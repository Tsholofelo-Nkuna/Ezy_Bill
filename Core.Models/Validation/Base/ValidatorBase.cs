using Core.Presentation.Models.DataTransferObjects.Base;
using Core.Presentation.Models.Interfaces;
using Core.Presentation.Models.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaultValidators = Core.Presentation.Models.Validation.Validators;

namespace Core.Presentation.Models.Validation.Base
{
    public class ValidatorBase<TRecord> : IValidatorBase<TRecord> where TRecord: BaseDto, new()
    {
        public string TargetPropertyName { get; set; }
        public ValidatorBase(IEnumerable<IValidatorFn> validators, string targetPropertyName) { 
            this.Validators = validators;
            this.TargetPropertyName = targetPropertyName;
     
        }
        public bool AddValidator(IValidatorFn validator)
        {
           var currentValidatorCount = this.Validators.Count();  
           if(!this.Validators.Any(x => x.Name == validator.Name))
           {
                this.Validators = this.Validators.Append(validator);
           }
           else
           {
                throw new ArgumentException($"Validator {validator.Name} already exists");
           }
         
           return this.Validators.Count()  == currentValidatorCount +1;
        }

        public bool RemoveValidator(IValidatorFn validator)
        {
            var currentValidatorCount = this.Validators.Count();
            this.Validators = this.Validators.Where(x => x.Name != validator.Name);
            return this.Validators.Count() <= currentValidatorCount;
        }

        public KeyValuePair<string, string>? Validate(TRecord? record)
        {
            var propValue = record?.GetType()?.GetProperty(this.TargetPropertyName)?.GetValue(record);
            return this.Validators.Select(x => x.Validator(propValue)).FirstOrDefault(r => r!=null);
        }

        public bool ClearValidators()
        {
            this.Validators = this.Validators.Where(x => false);
            return !this.Validators.Any();
        }

        public IEnumerable<IValidatorFn> Validators { get; set; }

        ~ValidatorBase()
        {
            this.ClearValidators();
        }
    }
}
