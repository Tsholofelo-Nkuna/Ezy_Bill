using ClientManagement.Models.Base;
using ClientManagement.Models.DataTransferObjects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.Models
{
    public class PageComponentViewModel : GenericListViewModel<BaseDto>
    {
        public PageComponentViewModel() : this(Enumerable.Empty<BaseDto>().Append(new BaseDto())) { }

        public PageComponentViewModel(IEnumerable<BaseDto> state) : base(state)
        {
        }
    }
}
