using AmazonWebService.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AmazonWebService.Controllers
{
    public class HubProductDispatcher : IHubProductDispatcher
    {
        private readonly IHubContext<ProductHub> _hubContext;
        public HubProductDispatcher(IHubContext<ProductHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task ChangeProduct(Product product)
        {
            await _hubContext.Clients.All.SendAsync("ChangeProduct", product);
        }
    }
}
