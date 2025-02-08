using Core.Utils.Interfaces;

namespace Core.Utils.State
{
    public class AppState : IAppState
    {
        public Guid ProfileId { get ; set; }
        public string Username { get; set; } = string.Empty;
    }
}
