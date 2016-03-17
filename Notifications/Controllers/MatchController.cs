using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Notifications.Hubs;

namespace Notifications.Controllers
{
    public class MatchController : ApiController
    {
        private readonly IHubConnectionContext<IMatchEvents> _context;

        public MatchController(IHubConnectionContext<IMatchEvents> context)
        {
            _context = context;
        }

        [Route("Match/Goals")]
        public IHttpActionResult Post(GoalScored goalScored)
        {
            //_context.Clients.All.GoalScored(goalScored.Team, goalScored.Player);
            _context.All.Handle(goalScored);
            return Ok();
        }

        [Route("Match/YellowCards")]
        public IHttpActionResult Post(YellowCard yellowCard)
        {
            //_context.Clients.All.YellowCard(yellowCard);
            _context.All.Handle(yellowCard);
            return Ok();
        }
    }
}