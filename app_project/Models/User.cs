using app_project.Models;
using System.Xml.Serialization;

namespace app_project.Models
{
    public class User
    {
        [XmlElement("Username")]
        public string Username { get; set; } = string.Empty;

        [XmlElement("Password")]
        public string Password { get; set; } = string.Empty;

        [XmlElement("Role")]
        public UserRole Role { get; set; }

        public User() { }

        public User(string username, string password, UserRole role)
        {
            Username = username;
            Password = password;
            Role = role;
        }
    }
}