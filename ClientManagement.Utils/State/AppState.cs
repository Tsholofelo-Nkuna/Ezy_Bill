using ClientManagement.Utils.Interfaces;

namespace ClientManagement.Utils.State
{
    public class AppState : IAppState
    {
        public Guid ProfileId { get ; set; }
        public string Username { get; set; } = string.Empty;
    }
}
