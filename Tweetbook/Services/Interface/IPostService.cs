using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetbook.Domain;

namespace Tweetbook.Services.Interface
{
    public interface IPostService
    {

        Task<List<Post>> GetPostsAsync();
        Task<Post> GetPostByIdAsync(Guid postId);

        Task<bool> UpdatePostAsync(Post updatePost);
        
        Task<bool> DeletePostAsync(Guid postId);

        Task<bool> CreatePostAsync(Post post);


        Task<bool> UserOwnPostAsync(Guid postId, string userId);
        
        Task<List<Tag>> GetAllTagsAsync();
    }
}