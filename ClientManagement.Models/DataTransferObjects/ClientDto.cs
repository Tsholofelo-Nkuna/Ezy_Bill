using ClientManagement.Models.DataTransferObjects.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClientManagement.Models.DataTransferObjects
{
    public class ClientDto : BaseDto
    {
        [Description("A company's name or the name of an individual")]
        public string CompanyName { get; set; } = string.Empty;
        [Description("The trading name of the business. If the client is an individual, this should be their surname.")]
        public string TradingAs { get; set; } = string.Empty;
        [Description("Contact number of a person or company, this is the `LandLine` field"), Obsolete("Use primary contact phone field")]
        public string LandlineNumber { get; set; } = string.Empty;
        [Description("State/Province of where a person resides or where a company is located")]
        public string Province { get; set; } = string.Empty;
        [Description("Street address of where a company is located or where a person resides")]
        public string Address { get; set; } = string.Empty;
        [Description("The name of the owner of both the email and phone contact")]
        public string PrimaryContactName { get; set; } = string.Empty;

        [Description("The company email or email of a person")]
        public string PrimaryContactEmail { get; set; } = string.Empty;
        [Description("The telephonic or cell number of a company or an individual, this should match the contact number provided by the `LandLine` field")]
        public string PrimaryContactPhone { get; set; } = string.Empty;
        public List<ContactPersonDto> ContactPerson { get; set; } = new List<ContactPersonDto>();

    }
}
