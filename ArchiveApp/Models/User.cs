using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace ArchiveApp.Models
{
    public class User : IEquatable<User>, IFormattable
    {
        public User() { }

        public User(string name, string password)
        {
            Name = name;
            Password = password;
        }
        public User(string name, string password, string location)
        {
            Name = name;
            Password = password;
            ArchiveLocation = location;
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ArchiveLocation { get; set; }

        public ICollection<Image> Images { get; set; }

        public bool Equals(User other)
        {
            if (other is null)
                return false;

            if (this.Name == other.Name && this.Password == other.Password)
                return true;
            else
                return false;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            User personObj = obj as User;
            if (personObj == null)
                return false;
            else
                return Equals(personObj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator == (User user1, User user2)
        {
            return user1.Equals(user2);
        }

        public static bool operator != (User user1, User user2)
        {
            return !user1.Equals(user2);
        }

        public string ToString(string format)
        {
            return this.ToString(format, CultureInfo.CurrentCulture);
        }
        
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (String.IsNullOrEmpty(format)) format = "G";
            if (formatProvider == null) formatProvider = CultureInfo.CurrentCulture;

            switch (format.ToUpperInvariant())
            {
                case "G": return this.ToString();
                case "1L":
                    return "Id = " + Id + ", Name = " + Name + ", Password = " + Password + ", Archive Location = " + ArchiveLocation;
                case "ML":
                    return $"Id = {Id}\nName = {Name}\nPassword = {Password}\nArchive Location = {ArchiveLocation}";
                default:
                    throw new FormatException(String.Format("The {0} format string is not supported.", format));
            }
        }
    }
}
