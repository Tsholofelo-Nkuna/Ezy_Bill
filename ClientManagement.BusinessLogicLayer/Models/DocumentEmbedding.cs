using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.BusinessLogicLayer.Models
{
    public class DocumentEmbedding
    {
        [VectorStoreKey]
        public Guid Id { get; set; } = Guid.NewGuid();
        [VectorStoreData]
        public string Title {  get; set; }
        [VectorStoreVector(1024, DistanceFunction = DistanceFunction.CosineDistance, StorageName = "vec")]
        public Embedding<float> TextEmbdedding { get; set; }
    }
}
