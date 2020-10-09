using System;

namespace AwesomeBlog.Model
{
    public class Blog
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public Author Author { get; set; }

        public Blog()
        {
        }
        
        public Blog(Guid id, string name, DateTime createdOn, Author author)
        {
            Id = id;
            Name = name;
            CreatedOn = createdOn;
            Author = author;
        }
    }
}