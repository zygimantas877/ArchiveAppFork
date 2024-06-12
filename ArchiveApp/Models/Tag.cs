using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArchiveApp.Models
{
    public class Tag : Info
    {
        public Tag() { }
        public Tag(string name)
        {
            Name = name;
        }

        public int ImageId { get; set; }
        public Image Image { get; set; }
    }
}
