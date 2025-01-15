using Core.Presentation.Models.DataTransferObjects.Base;
using Core.Presentation.Models.Interfaces.Base;
using  Core.Presentation.Models.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Core.Presentation.ViewComponents.Interfaces.Base
{
    public interface IGenericComponentBase<TViewModel, TRecordType> : IComponent
        where TViewModel : IGenericListViewModel<TRecordType>
        where TRecordType : BaseDto
    {
        TViewModel ViewModel { get; set; }
        NavigationManager NavManager { get; set; }
        IJSRuntime JS { get; set; }
        public void OnNavigate(string controllerName, string actionName, Guid stateId);
        public Task OnViewModelStateChanged(IEnumerable<TRecordType> update);
        public IEnumerable<KeyValuePair<string, object?>> RecordAsKeyValuePairs(TRecordType record);
    }
}
