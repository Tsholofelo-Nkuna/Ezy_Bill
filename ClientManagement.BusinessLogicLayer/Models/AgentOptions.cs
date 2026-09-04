using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.BusinessLogicLayer.Models
{
    public class AgentOptions
    {
        public string SkillPath { get; set; } = string.Empty;
        public IEnumerable<AgentMetaData> AiAgentMetaData { get; set; } = [];
        public string OllamaUrl { get; set; } = string.Empty;
        public string OllamaModel { get; set; } = string.Empty;
        public string VectorStoreUrl {  get; set; } = string.Empty;
        public string EmbeddingModel { get; set; } = string.Empty;
        public string VectorStoreCollectionName {  get; set; } = string.Empty;
    }
}
