using ClientManagement.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.DataAccessLayer.Entities
{
    public class ProductEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty ;
        public double Price { get; set; }
    }
}
