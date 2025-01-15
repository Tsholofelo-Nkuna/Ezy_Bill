using Core.Presentation.Models.Base;
using Core.Presentation.Models.DataTransferObjects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.Models
{
    public class PageComponentViewModel : GenericListViewModel<BaseDto>
    {
        public PageComponentViewModel() : this(Enumerable.Empty<BaseDto>().Append(new BaseDto())) { }

        public PageComponentViewModel(IEnumerable<BaseDto> state) : base(state)
        {
        }
    }
}
