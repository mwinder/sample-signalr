using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Notifications.Hubs
{
    public class MatchHub : Hub<IMatchEvents>
    {
        public override Task OnConnected()
        {
            return base.OnConnected();
        }
    }

    public interface IMatchEvents
    {
        void GoalScored(string team, string player);
        void YellowCard(YellowCard yellowCard);
        void Handle(GoalScored goalScored);
        void Handle(YellowCard yellowCard);
    }

    public class GoalScored
    {
        public string Event { get; set; } = "GoalScored";

        public string Team { get; set; }
        public string Player { get; set; }
    }

    public class YellowCard
    {
        public string Player { get; set; }
    }
}