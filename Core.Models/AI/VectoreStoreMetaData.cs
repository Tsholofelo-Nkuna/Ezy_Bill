using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClientManagement.Models.AI
{
    public class VectoreStoreMetaData
    {
        [Description("The name of the knowledge source. Always use this when asked to provide a name for the knowledge source unless instructed otherwise.")]
        public string Name { get; set;  } = string.Empty;
        [Description("A user friendly name for the knowledge source. This is the display name that can be used in user interfaces as a label.")]
        public string DisplayName { get; set; } = string.Empty;
        [Description("Gives a description of the type of data that the knowledge source contains.")]
        public string Description {  get; set; } = string.Empty;
    }
}
