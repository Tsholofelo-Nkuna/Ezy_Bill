

namespace Core.Presentation.Models.DataTransferObjects.Base
{
    public class BaseDto
    {
        public Guid Id { get; set; }
        public bool Archived { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
