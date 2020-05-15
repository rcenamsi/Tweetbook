using System.Threading.Tasks;
using Tweetbook.Domain;

namespace Tweetbook.Services.Interface
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);
    }
}