using Core.Presentation.Models.DataTransferObjects.Base;

namespace Core.Presentation.Models.DataTransferObjects
{
    public class ClientDto : BaseDto
    {
        public string CompanyName { get; set; } = string.Empty;
        public string TradingAs { get; set; } = string.Empty;
        public string LandlineNumber { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PrimaryContactName { get; set; } = string.Empty;
        public string PrimaryContactEmail { get; set; } = string.Empty;
        public string PrimaryContactPhone { get; set; } = string.Empty;
        public List<ContactPersonDto> ContactPerson { get; set; } = new List<ContactPersonDto>();

    }
}
