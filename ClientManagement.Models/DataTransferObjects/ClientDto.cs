using ClientManagement.Models.DataTransferObjects.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClientManagement.Models.DataTransferObjects
{
    public class ClientDto : BaseDto
    {
        [Description("A company's name or the name of an individual"), Required]
        public string CompanyName { get; set; } = string.Empty;
        [Description("The trading name of the business. If the client is an individual, this should be their surname."), Required]
        public string TradingAs { get; set; } = string.Empty;
        [Description("Contact number of a person or company, this is the `LandLine` field"), Required]
        public string LandlineNumber { get; set; } = string.Empty;
        [Description("State/Province of where a person resides or where a company is located"), Required]
        public string Province { get; set; } = string.Empty;
        [Description("Street address of where a company is located or where a person resides"), Required]
        public string Address { get; set; } = string.Empty;
        [Description("The name of the owner of both the email and phone contact"), Required]
        public string PrimaryContactName { get; set; } = string.Empty;

        [Description("The company email or email of a person"), Required]
        public string PrimaryContactEmail { get; set; } = string.Empty;
        [Description("The telephonic or cell number of a company or an individual, this should match the contact number provided by the `LandLine` field"), Required]
        public string PrimaryContactPhone { get; set; } = string.Empty;
        public List<ContactPersonDto> ContactPerson { get; set; } = new List<ContactPersonDto>();

    }
}
