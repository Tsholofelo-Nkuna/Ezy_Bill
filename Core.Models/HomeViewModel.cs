using Core.Presentation.Models.Base;
using Core.Presentation.Models.DataTransferObjects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.Models
{
    public class HomeViewModel : GenericListViewModel<BaseDto>
    {
        public HomeViewModel() : this(new List<BaseDto>())
        {
        }
        public HomeViewModel(IEnumerable<BaseDto> state) : base(state)
        {
        }
    }
}
