using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveApp.Models
{
    public class Group : Info
    {
        public Group() { }
        public Group(string name ,string location)
        {
            Name = name;
            Location = location;
        }

        [Required]
        public string Location{ get; set; }

        public int ImageId { get; set; }
        public Image Image { get; set; }
    }
}
