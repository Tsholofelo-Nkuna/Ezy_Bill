using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.Models
{
    public class TabItemViewModel
    {
        private bool _active = false;
        public string Title { get; set; }
        public string Id { get; init; }
        public string Name { get; set; }
        public bool Active
        {
            get => _active;
            set
            {
                _active = value;
                if (_active)
                {
                    ActiveClass = "active";
                }
                else
                {
                    ActiveClass = string.Empty;
                }
            }
        }
        public string ActiveClass { get; set; } = string.Empty;

        [Obsolete("Use the three argument contructor instead")]
        public TabItemViewModel(string title, string id)
        {
            Title = title;
            Id = id;

        }

        public TabItemViewModel(string title, string id, string name)
        {
            Title = title;
            Id = id;
            Name = name;
        }


    }
}
