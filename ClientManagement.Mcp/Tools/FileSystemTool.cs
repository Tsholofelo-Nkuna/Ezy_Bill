using ModelContextProtocol.Server;
using System.ComponentModel;

namespace ClientManagement.Mcp.Tools
{
    [McpServerToolType]
    public class FileSystemTool
    {
        [McpServerTool, Description("Reads contents of a file")]
        public string ReadFileContents([Description("The path of the file from which contents will be read")]string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
