namespace theUpSkilzAPI.Dtos
{
    public class CourseDto
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public string ImageBase64 { get; set; } // still store image
        public string FileBase64 { get; set; } // 📄 PDF
        public string FileName { get; set; }   // e.g. syllabus.pdf
        public List<ChapterDto> Chapters { get; set; }
    }

    public class ChapterDto
    {
        public string Name { get; set; }
        public List<string> Topics { get; set; }
    }

    public class CourseUploadDto
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public string ImageBase64 { get; set; }
        public List<ChapterDto> Chapters { get; set; }
    }

}
