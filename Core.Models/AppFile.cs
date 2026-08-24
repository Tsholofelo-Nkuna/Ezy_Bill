using System;

namespace Core.Presentation.Models
{
    /// <summary>
    /// Represents a file that has been uploaded or is being processed in the application
    /// </summary>
    public class AppFile
    {
        /// <summary>
        /// The name of the file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The size of the file in bytes
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Unique identifier for the file
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

        /// <summary>
        /// Initializes a new instance of the AppFile class
        /// </summary>
        public AppFile()
        {
            FileId = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the AppFile class with specified parameters
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <param name="fileSize">The size of the file in bytes</param>
        public AppFile(string fileName, long fileSize)
        {
            FileName = fileName;
            FileSize = fileSize;
            FileId = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the AppFile class with all parameters
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <param name="fileSize">The size of the file in bytes</param>
        /// <param name="fileId">The unique identifier for the file</param>
        public AppFile(string fileName, long fileSize, Guid fileId)
        {
            FileName = fileName;
            FileSize = fileSize;
            FileId = fileId;
        }
    }
}
