using Core.Presentation.Models.DataTransferObjects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.Models.Interfaces
{
    public interface IValidatorFn
    {
        string Name { get; set; }
        string Message { get; set; }
        Func<object?, KeyValuePair<string, string>?> Validator { get; set; }
    }
}
