using ClientManagement.Models.DataTransferObjects.Base;
using System;

namespace ClientManagement.Models.DataTransferObjects
{
    public class AppFileDto : BaseDto
    {
        public string FileName { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public Guid FileId { get; set; }
        public byte[] Contents { get; set; } = Array.Empty<byte>();
        public string MimeType { get; set; } = string.Empty;
    }
}
