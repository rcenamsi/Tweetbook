using System.ComponentModel.DataAnnotations;

namespace Tweetbook.Contracts.v1.Requests
{
    public class UserLoginRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}