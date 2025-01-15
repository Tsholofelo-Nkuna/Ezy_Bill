using Core.Presentation.Models.DataTransferObjects.Base;
using Core.Presentation.Models.Interfaces.Base;
using System.Diagnostics;


namespace Core.Presentation.Models.Base
{
    public class GenericListViewModel<TRecordType> : IGenericListViewModel<TRecordType> where TRecordType: BaseDto, new()
    {
        private GenericViewModel<TRecordType> _genericViewModel = new GenericViewModel<TRecordType>(new TRecordType());

        public event IGenericListViewModel<TRecordType>.OnViewModelStateChangeDelegate OnViewModelStateChangedEvent;

        public IEnumerable<TRecordType> ViewModelState { get; set; }
       
        public GenericListViewModel(IEnumerable<TRecordType> state) {
           this.ViewModelState = state;
        }
      

        public TValue? Get<TValue>(Guid id, string propName, bool applyDisplayFormat = false)
        {
           var source = this.ViewModelState.FirstOrDefault(x => x.Id == id);
           if(source is TRecordType validSource)
            {
                _genericViewModel.ViewModelState = source;
                return _genericViewModel.Get(propName,typeof(TValue), applyDisplayFormat);
            }
            else
            {
                return default;
            }
        }

        public void Set(Guid id, string propName, object? value)
        {
           var target = this.ViewModelState.FirstOrDefault(y => y.Id == id);
            if(target is TRecordType validTarget)
            {
                _genericViewModel.ViewModelState = target;
                _genericViewModel.Set(propName, value);
                this.OnViewModelStateChangedEvent?.Invoke(this.ViewModelState);
            }

        }

        public void AddViewModelStateChangeListener(IGenericListViewModel<TRecordType>.OnViewModelStateChangeDelegate listener)
        {
            this.OnViewModelStateChangedEvent += listener;  
        }

        public void ClearViewModelStateChangeListeners()
        {
            this.OnViewModelStateChangedEvent = null;
        }

        ~GenericListViewModel()
        {
          this.ClearViewModelStateChangeListeners();
        }
    }
}
