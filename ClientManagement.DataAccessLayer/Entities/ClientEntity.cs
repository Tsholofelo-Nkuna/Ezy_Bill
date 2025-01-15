using ClientManagement.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.DataAccessLayer.Entities
{
    public class ClientEntity : BaseEntity
    {
        public string CompanyName { get; set; } = string.Empty;
        public string TradingAs { get; set; } = string.Empty ;
        public string LandlineNumber { get; set; } = string.Empty;
        public string Province {  get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<ContactPersonEntity> ContactPerson { get; set; } = new List<ContactPersonEntity>();
    }
}
