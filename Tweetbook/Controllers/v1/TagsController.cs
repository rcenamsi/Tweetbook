﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tweetbook.Contracts.v1;
using Tweetbook.Services.Interface;

namespace Tweetbook.v1.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TagsController : Controller
    {
        private readonly IPostService _postService;
        
        public TagsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet(ApiRoutes.Tags.GetAll)]
        [Authorize(Policy = "TagViewer")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _postService.GetAllTagsAsync());
        }
    }
}