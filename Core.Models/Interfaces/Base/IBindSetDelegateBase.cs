using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.ViewComponents.Interfaces.Base
{
   
    public interface IBindToSetterDelegateBase<TKey, TValue, TModelState> 
    {
        public delegate void BindToSetterDelegate(TKey key,TValue val, TModelState modelState);
        public void Set(TKey key,TValue val, BindToSetterDelegate bindToSetter);
    }
}
