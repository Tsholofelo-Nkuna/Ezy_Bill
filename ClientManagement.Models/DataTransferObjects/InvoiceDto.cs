using ClientManagement.Models.DataTransferObjects.Base;
using System.ComponentModel.DataAnnotations;


namespace ClientManagement.Models.DataTransferObjects
{
    public class InvoiceDto : BaseDto
    {
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}")]
        public DateTime DueDate { get; set; } = DateTime.MinValue;
        public ClientDto Client { get; set; } = new ClientDto();
        public string ClientId
        {
            get => this.Client is not null ? this.Client.Id.ToString() : string.Empty;
            set
            {
                if (this.Client is not null)
                {
                    _ = Guid.TryParse(value, out var guidResult);
                    this.Client.Id = guidResult;
                }
                else
                {
                    _ = Guid.TryParse(value, out var guidResult);
                    this.Client = new ClientDto { Id = guidResult };
                }
            }
        }

        public string ClientName
        {
            get => this.Client is not null ? $"{this.Client.CompanyName}" : string.Empty;
            set
            {
              
                if (this.Client is not null) { 
                  this.Client.CompanyName = value;
                }
                else
                {
                    this.Client = new ClientDto { CompanyName = value }; 
                }
            }
        }

        public string PrimaryContact
        {
          
            get
            {
                return this.Client is not null ? this.Client.ContactPerson.FirstOrDefault(x => x.IsPrimaryContact)?.Phone ?? string.Empty : string.Empty;
            }
            set
            {
                
                if (this.Client is not null)
                {
                    if (this.Client.ContactPerson is { Count: > 0 })
                    {
                        var cInfo = this.Client.ContactPerson.FirstOrDefault(x => x.IsPrimaryContact);
                        if (cInfo is ContactPersonDto)
                        {
                            cInfo.Phone = value;
                        }
                    }
                    else { 
                       this.Client.ContactPerson.Add(new ContactPersonDto { Phone = value, IsPrimaryContact = true });
                    }
                 
                }
                else
                {
                    this.Client = new ClientDto();
                    this.Client.ContactPerson.Add(new ContactPersonDto {  IsPrimaryContact = true, Phone = value });
                    
                }
            }
        }

        public string PrimaryEmail 
        {
            get => this.Client is not null ? this.Client.ContactPerson
                .FirstOrDefault(contact => contact.IsPrimaryContact)?.Email ?? string.Empty : string.Empty;
            set
            {
                if (this.Client is not null)
                {
                    if(this.Client.ContactPerson is { Count : > 0 })
                    {
                        var contactInfo = this.Client.ContactPerson.FirstOrDefault(x => x.IsPrimaryContact);
                        if(contactInfo is ContactPersonDto)
                        {
                            contactInfo.Email = value;
                        }
                    }
                    else
                    {
                        this.Client.ContactPerson.Add(new ContactPersonDto
                        {
                            IsPrimaryContact = true,
                            Email = value
                        });
                    }
                }
                else
                {
                    this.Client = new ClientDto();
                    this.Client.ContactPerson.Add(new ContactPersonDto { 
                      IsPrimaryContact = true,
                      Email = value
                    });
                }
            }
        }

        public string Contact => $"{PrimaryContact} | {PrimaryEmail}";
        [DisplayFormat(DataFormatString ="{0:C}")]
        public double Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double OutstandingAmount { get; set; }
        public bool Unpaid { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double PaidAmount
        {
            get
            {
                return Amount - OutstandingAmount;
            }
        }

        public IEnumerable<string> ProductIdentifiers { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<InvoiceProductDto> InvoiceProducts { get; set; } = Enumerable.Empty<InvoiceProductDto>();

    }
}
