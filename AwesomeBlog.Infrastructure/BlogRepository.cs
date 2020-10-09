using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwesomeBlog.Model;

namespace AwesomeBlog.Infrastructure
{
    public class BlogRepository
    {
        private static readonly ICollection<Blog> _blogCollection = new List<Blog>();
        private readonly DatabaseContext _databaseContext;

        public BlogRepository()
        {
            _databaseContext = new DatabaseContext();
        }

        public ICollection<Blog> GetBlogs() => _blogCollection;
        public Blog GetBlog(Guid id) => _blogCollection.FirstOrDefault(x => x.Id == id);

        public async Task Create(Blog blog)
        {
            await _databaseContext
                .GetCollection<Blog>()
                .InsertOneAsync(blog);
        }

        public bool Update(Blog blog)
        {
            var entity = _blogCollection.FirstOrDefault(x => x.Id == blog.Id);

            if (entity == null)
                return false;

            entity.Author = blog.Author;
            entity.Name = blog.Name;
            entity.CreatedOn = blog.CreatedOn;
            
            return true;
        }

        public bool Delete(Guid id)
        {
            var entity = _blogCollection.FirstOrDefault(x => x.Id == id);

            if (entity == null)
                return false;

            _blogCollection.Remove(entity);

            return true;
        }
    }
}