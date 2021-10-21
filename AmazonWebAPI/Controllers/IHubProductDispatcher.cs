using AmazonWebService.Models;
using System.Threading.Tasks;

namespace AmazonWebService.Controllers
{
    public interface IHubProductDispatcher
    {
        Task ChangeProduct(Product product);
    }
}
