using System;
using System.Collections.Generic;
using System.Linq;
using Tweetbook.Domain;
using Tweetbook.Services.Interface;

namespace Tweetbook.Services
{
    public class PostService : IPostService
    {
        private readonly List<Post> _posts;
        
        public PostService()
        {
            _posts = new List<Post>();
            for (var i = 0; i < 5; i++)
            {
                _posts.Add(new Post
                {
                    Id = Guid.NewGuid(),
                    Name = $"Post Name{i}"
                });
            }
        }
        
        public List<Post> GetPosts()
        {
            return _posts;
        }

        public Post GetPostById(Guid postId)
        {
            return _posts.SingleOrDefault(x => x.Id == postId);
        }

        public bool UpdatePost(Post updatePost)
        {
            var isExists = GetPostById(updatePost.Id) != null;
            if (!isExists)
                return false;

            var index = _posts.FindIndex(x => x.Id == updatePost.Id);
            _posts[index] = updatePost;
            return true;
        }
    }
}