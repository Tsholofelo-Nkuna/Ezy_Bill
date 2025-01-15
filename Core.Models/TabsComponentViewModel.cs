using Core.Presentation.Models.Base;
using Core.Presentation.Models.DataTransferObjects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.Models
{
    public class TabsComponentViewModel<TRecord> : GenericListViewModel<TRecord> where TRecord : BaseDto, new()
    {
        public TabsComponentViewModel() : this(new[] { new TRecord() }) { }
        public TabsComponentViewModel(IEnumerable<TRecord> state) : base(state)
        {
        }

        public IEnumerable<TabItemViewModel> TabItems { get; set; } = Enumerable.Empty<TabItemViewModel>();
    }
}
