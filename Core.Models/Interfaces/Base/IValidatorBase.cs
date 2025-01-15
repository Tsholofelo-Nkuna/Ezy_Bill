using Core.Presentation.Models.DataTransferObjects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.Models.Interfaces.Base
{
    public interface IValidatorBase<TRecordType> where TRecordType: BaseDto, new()
    {
        string TargetPropertyName { get; set; }
        IEnumerable<IValidatorFn> Validators { get; set; }
        KeyValuePair<string, string>? Validate(TRecordType? record);
        bool AddValidator(IValidatorFn validator);
        bool RemoveValidator(IValidatorFn validator);
        bool ClearValidators();
    }
}
