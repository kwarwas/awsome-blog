using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AwesomeBlog.Infrastructure;
using AwesomeBlog.Model;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeBlog.Controllers
{
    [Route("[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly BlogRepository _blogRepository;

        public BlogController(BlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        
        [HttpGet]
        public ActionResult<ICollection<Blog>> Get()
        {
            return Ok(_blogRepository.GetBlogs());
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Blog> Get(Guid id)
        {
            var blog = _blogRepository.GetBlog(id);

            if (blog == null)
                return BadRequest();
            
            return blog;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Blog blog)
        {
            await _blogRepository.Create(blog);

            return Created(string.Empty, blog);
        }

        [HttpPut]
        public ActionResult Update([FromBody] Blog blog)
        {
            if (_blogRepository.Update(blog))
                return Ok();

            return BadRequest();
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Delete(Guid id)
        {
            if (_blogRepository.Delete(id))
                return Ok();

            return BadRequest();
        }
    }
}