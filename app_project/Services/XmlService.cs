using app_project.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace app_project.Services
{
    public static class XmlService
    {
        // ── Users ──────────────────────────────────────────────

        public static List<User> LoadUsers(string filePath)
        {
            if (!File.Exists(filePath))
            {
                var defaults = new List<User>
                {
                    new User("admin", "admin123", UserRole.Admin),
                    new User("visitor", "visitor123", UserRole.Visitor)
                };
                SaveUsers(defaults, filePath);
                return defaults;
            }

            var serializer = new XmlSerializer(typeof(List<User>),
                                               new XmlRootAttribute("Users"));
            using var reader = new StreamReader(filePath);
            return (List<User>)serializer.Deserialize(reader);
        }

        public static void SaveUsers(List<User> users, string filePath)
        {
            EnsureDirectory(filePath);
            var serializer = new XmlSerializer(typeof(List<User>),
                                               new XmlRootAttribute("Users"));
            using var writer = new StreamWriter(filePath);
            serializer.Serialize(writer, users);
        }

        // ── Moments ────────────────────────────────────────────

        public static List<IconicMoment> LoadMoments(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<IconicMoment>();

            var serializer = new XmlSerializer(typeof(List<IconicMoment>),
                                               new XmlRootAttribute("Moments"));
            using var reader = new StreamReader(filePath);
            return (List<IconicMoment>)serializer.Deserialize(reader);
        }

        public static void SaveMoments(List<IconicMoment> moments, string filePath)
        {
            EnsureDirectory(filePath);
            var serializer = new XmlSerializer(typeof(List<IconicMoment>),
                                               new XmlRootAttribute("Moments"));
            using var writer = new StreamWriter(filePath);
            serializer.Serialize(writer, moments);
        }

        // ── Helper ─────────────────────────────────────────────

        private static void EnsureDirectory(string filePath)
        {
            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);
        }
    }
}