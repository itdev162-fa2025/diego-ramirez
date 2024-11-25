using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;

using Persistence;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PostsController : ControllerBase
    {
        private readonly DataContext _context;
        public PostsController(DataContext context)
        {
            this._context = context;
        }
        /// <summary>
        /// GET api/posts
        /// </summary>
        /// <returns> A list of posts</returns>
        [HttpGet(Name = "GetPosts")]
        public ActionResult<List<Post>> Get()
        {
            return this._context.Posts.ToList();
        }
        /// <summary>
        ///  Get api/post/[id]
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A single post</returns>
        [HttpGet("{id}", Name = "GetById")]
        public ActionResult<Post> GetById(Guid id)
        {
            var post = this._context.Posts.Find(id);
            if (post is null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        /// <summary>
        /// POST api/post
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A new post</returns>
        [HttpPost(Name = "Create")]
        public ActionResult<Post> Create([FromBody] Post request)
        {
            var post = new Post
            {
                Id = request.Id,
                Title = request.Title,
                Body = request.Body,
                Date = request.Date
            };

            _context.Posts.Add(post);
            var success = _context.SaveChanges() > 0;

            if (success)
            {
                return Ok(post);
            }

            throw new Exception("Error creating post");
        }

        /// <summary>
        /// PUT api/post
        /// </summary>
        /// <param name="request">JSON request containing one or more updated post fields</param>
        /// <returns></returns>
        [HttpPut(Name = "Update")]
        public ActionResult<Post> Update([FromBody] Post request)
        {
            var post = _context.Posts.Find(request.Id);
            if (post == null)
            {
                throw new Exception("Could not find post");
            }

            //Update the post properties with request values, if present
            post.Title = request.Title != null ? request.Title : post.Title;
            post.Body = request.Body != null ? request.Body : post.Body;
            post.Date = request.Date != DateTime.MinValue ? request.Date : post.Date;

            var success = _context.SaveChanges() > 0;
            if (success)
            {
                return Ok(post);
            }

            throw new Exception("Error updating post");
        }
    }
}