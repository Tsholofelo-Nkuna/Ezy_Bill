using Core.Presentation.Models.DataTransferObjects.Base;
using Core.Presentation.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.Models.Validation
{
    public class ValidatorFn : IValidatorFn
    {
        public string Name { get; set; } = string.Empty;
        public string Message { get; set ; } = string.Empty;
        public Func<object?, KeyValuePair<string, string>?> Validator { get; set ; } = (val) => null;
    }
}
