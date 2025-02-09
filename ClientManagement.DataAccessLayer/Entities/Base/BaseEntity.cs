


using Microsoft.EntityFrameworkCore;

namespace ClientManagement.DataAccessLayer.Entities.Base
{
    [Index("ProfileId")]
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public bool Archived { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid ProfileId { get;set; }
    }
}
