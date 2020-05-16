using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Tweetbook.Contracts.v1;
using Tweetbook.Contracts.v1.Requests;
using Tweetbook.Domain;
using Xunit;

namespace Tweetbook.IntegrationTests
{
    public class PostControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetAll_WithoutAnyPosts_ReturnEmptyResponse()
        {
            // Arrange
            await AuthenticateAsync();
            
            // Act
            var response = await TestsClient.GetAsync(ApiRoutes.Posts.GetAll);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK); // HttpStatusCode.NotAcceptable
            (await response.Content.ReadAsAsync<List<Post>>()).Should().BeEmpty();
        }

        [Fact]
        public async Task Get_ReturnsPosts_WhenExistsInTheDatabase()
        {
            // Arrange
            await AuthenticateAsync();
            var createdPost = await CreatePostAsync(new CreatePostRequest {Name = "Test Post"});
            
            // Act
            var response =
                await TestsClient
                    .GetAsync(ApiRoutes.Posts.Get
                        .Replace("{postId}", createdPost.Id.ToString()));
            

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedPost = await response.Content.ReadAsAsync<Post>();
            returnedPost.Id.Should().Be(createdPost.Id);
            returnedPost.Name.Should().Be("Test Post");
        }
        
    }
}