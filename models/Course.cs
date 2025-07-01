using System.Collections.Generic;

namespace theUpSkilzAPI.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public string ImageBase64 { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public string? FileBase64 { get; set; }  // ✅ Make this nullable

        public List<Chapter> Chapters { get; set; }
    }



    public class Chapter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Topic> Topics { get; set; } = new();
    }

    public class Topic
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
