using ClientManagement.Models.DataTransferObjects.Base;
using System.ComponentModel.DataAnnotations;


namespace ClientManagement.Models.DataTransferObjects
{
    public class ProductDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [DisplayFormat(DataFormatString ="{0:C}")]
        public double Price { get; set; }
       
    }
}
