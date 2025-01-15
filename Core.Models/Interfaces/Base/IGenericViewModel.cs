using Core.Presentation.Models.DataTransferObjects.Base;
using Core.Presentation.ViewComponents.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.Models.Interfaces.Base
{
    public interface IGenericViewModel<TState> : IBindToSetterDelegateBase<string, object?, TState> where TState : BaseDto, new()
    {
        public delegate void ViewModelStateChangeDelegate(TState viewModelState);
        public void Set<TSource>(string propName, TSource propValue);
        public dynamic? Get(string propName, Type propType, bool applyDisplayFormat = false);
        public void AddStateChangeListener(ViewModelStateChangeDelegate stateChangeListener);
        #region "Setter used to implement carrying pattern"
        public Action<TKey> Set<TKey, TValue>(TValue value);
        #endregion
    }
}
