

using Core.Presentation.Models.DataTransferObjects.Base;
using Core.Presentation.Models.Base;

namespace Core.Presentation.Models
{
    public class ModalViewModel<TRecordType> : GenericListViewModel<TRecordType> where TRecordType : BaseDto, new()
    {
        public ModalViewModel(): this(Enumerable.Empty<TRecordType>()) { }
        public ModalViewModel(IEnumerable<TRecordType> recs) : base(recs)
        {
        }
        public string DisplayStyle { get; set; } = "none";
        public string ShowhowClass { get; set; } = "show";
        private bool _show = false;
        public string Title { get; set; } = string.Empty;
        public bool Show { 
            get 
            { 
                return this._show;
            } 
            set {
               _show = value;
                if (this._show)
                {
                    this.DisplayStyle = "inline-block";
                    this.ShowhowClass = "show";
                }
                else
                {
                    this.DisplayStyle = "none";
                    this.ShowhowClass = "";
                }
            } 
        }
    }
}
