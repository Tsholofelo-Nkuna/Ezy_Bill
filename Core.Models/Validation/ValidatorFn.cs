using ClientManagement.Models.DataTransferObjects.Base;
using ClientManagement.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.Models.Validation
{
    public class ValidatorFn : IValidatorFn
    {
        public string Name { get; set; } = string.Empty;
        public string Message { get; set ; } = string.Empty;
        public Func<object?, KeyValuePair<string, string>?> Validator { get; set ; } = (val) => null;
    }
}
