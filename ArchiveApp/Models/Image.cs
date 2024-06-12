using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArchiveApp.Models
{
    public class Image
    {
        public Image() { }
        public Image(string name ,string location, int userId, Group group, ICollection<Tag> tags)
        {
            Name = name;
            Location = location;
            UserId = userId;
            Group = group;
            Tags = tags;
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public Group Group { get; set; }

        public ICollection<Tag> Tags { get; set; }
    }
}
