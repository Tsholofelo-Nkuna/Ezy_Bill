using ClientManagement.Models.Base;
using ClientManagement.Models.DataTransferObjects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.Models
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
