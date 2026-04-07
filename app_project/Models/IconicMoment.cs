using System;
using System.Xml.Serialization;

namespace app_project.Models
{
    public class IconicMoment
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Title")]
        public string Title { get; set; } = string.Empty;

        [XmlElement("Year")]
        public int Year { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; } = string.Empty;

        [XmlElement("ImagePath")]
        public string ImagePath { get; set; } = string.Empty;

        [XmlElement("RtfFilePath")]
        public string RtfFilePath { get; set; } = string.Empty;

        [XmlElement("DateAdded")]
        public DateTime DateAdded { get; set; }

        public IconicMoment() { }

        public IconicMoment(int id, string title, int year, string description,
                            string imagePath, string rtfFilePath, DateTime dateAdded)
        {
            Id = id;
            Title = title;
            Year = year;
            Description = description;
            ImagePath = imagePath;
            RtfFilePath = rtfFilePath;
            DateAdded = dateAdded;
        }
    }
}