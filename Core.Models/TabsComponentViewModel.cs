using ClientManagement.Models.Base;
using ClientManagement.Models.DataTransferObjects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.Models
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
