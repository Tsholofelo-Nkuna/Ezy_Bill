using ClientManagement.Models.Base;
using ClientManagement.Models.DataTransferObjects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.Models
{
    public class OverviewComponentViewModel<TRecord> : GenericListViewModel<TRecord> where TRecord : BaseDto, new()
    {
        public OverviewComponentViewModel() : this(Enumerable.Empty<TRecord>()) { }
        public OverviewComponentViewModel(IEnumerable<TRecord> state) : base(state)
        {
        }

        public IEnumerable<OverviewSectionViewModel> OverviewSectionsViewModel { get; set; } = Enumerable.Empty<OverviewSectionViewModel>();
    }
}
