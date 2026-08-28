using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.BusinessLogicLayer.Models
{
    public class AgentMetaData
    {
        public string Instructions { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Type {  get; set; } = string.Empty;
        public string? Description = string.Empty;
        public VectoreStoreMetaData? VecStoreMetaData { get; set; }
    }
}
