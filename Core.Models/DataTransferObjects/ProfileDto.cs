using Core.Presentation.Models.DataTransferObjects.Base;


namespace Core.Presentation.Models.DataTransferObjects
{
    public class ProfileDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Logo { get; set; }
    }
}
