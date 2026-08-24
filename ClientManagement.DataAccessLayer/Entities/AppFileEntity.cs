using ClientManagement.DataAccessLayer.Entities.Base;
using System;

namespace ClientManagement.DataAccessLayer.Entities
{
    /// <summary>
    /// Database entity that represents a file stored in the application
    /// </summary>
    public class AppFileEntity : BaseEntity
    {
        /// <summary>
        /// The name of the file
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// The size of the file in bytes
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// The unique identifier of the file
        /// </summary>
        public Guid FileId { get; set; }

        /// <summary>
        /// The raw bytes that make up the file's contents
        /// </summary>
        public byte[] Contents { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// The MIME type of the file (e.g. "application/pdf", "image/png")
        /// </summary>
        public string MimeType { get; set; } = string.Empty;
    }
}
