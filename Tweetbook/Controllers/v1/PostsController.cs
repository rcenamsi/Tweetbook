using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Tweetbook.Contracts.v1;
using Tweetbook.Contracts.v1.Requests;
using Tweetbook.Contracts.v1.Responses;
using Tweetbook.Domain;

namespace Tweetbook.v1.Controllers
{
    public class PostsController : Controller
    {
        private List<Post> _posts;

        public PostsController()
        {
            _posts = new List<Post>();
            for (var i = 0; i < 5; i++)
            {
                _posts.Add(new Post{Id = Guid.NewGuid().ToString()});
            }
        }
        
        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_posts);
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public IActionResult Create([FromBody] CreatePostRequest postRequest)
        {
            var post = new Post
            {
                Id = postRequest.Id
            };
            
            if (string.IsNullOrEmpty(post.Id))
                post.Id = Guid.NewGuid().ToString();
            
            _posts.Add(post);

            var baseUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUri + "/id?" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id);
            
            var response = new PostResponse
            {
                Id = post.Id
            };
            return Created(locationUri, response);
        }
    }
}