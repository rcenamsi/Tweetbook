using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tweetbook.Contracts.v1;
using Tweetbook.Contracts.v1.Requests;
using Tweetbook.Contracts.v1.Responses;
using Tweetbook.Data;

namespace Tweetbook.IntegrationTests
{
    public class IntegrationTest : IDisposable
    {
        protected readonly HttpClient TestsClient;
        private readonly IServiceProvider _serviceProvider;
        
        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    // Comment this to test it with the real database
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DataContext));
                        services.AddDbContext<DataContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });
                    });
                });
            
            _serviceProvider = appFactory.Services;
            TestsClient = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            TestsClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        protected async Task<PostResponse> CreatePostAsync(CreatePostRequest request)
        {
            var response = await TestsClient.PostAsJsonAsync(ApiRoutes.Posts.Create, request);
            return await response.Content.ReadAsAsync<PostResponse>();
        }

        private async Task<string> GetJwtAsync()
        {
            var response = await TestsClient.PostAsJsonAsync(ApiRoutes.Identity.Register, new UserRegistrationRequest
            {
                Email = "test@integration.com",
                Password = "P@ssw0rd123!"
            });
            var registrationResponse = await response.Content.ReadAsAsync<AuthSuccessResponse>();
            return registrationResponse.Token;
        }

        public void Dispose()
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<DataContext>();
            context.Database.EnsureDeleted();

        }
    }
}