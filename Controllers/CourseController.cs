using Microsoft.AspNetCore.Mvc;
using theUpSkilzAPI.Data;
using theUpSkilzAPI.Dtos;
using theUpSkilzAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly AppDbContext _context;

    public CourseController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadCourse([FromForm] CourseUploadDto dto, IFormFile file)
    {
        //var userEmail = HttpContext.Request.Headers["X-User-Email"].ToString();
        //if (userEmail.ToLower() != "krishnakothapalle@gmail.com")
        //    return Unauthorized("Only admin can save courses.");

        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        if (!Directory.Exists(uploadsDir))
            Directory.CreateDirectory(uploadsDir);

        string savedPath = null;
        if (file != null && file.Length > 0)
        {
            var filePath = Path.Combine(uploadsDir, file.FileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            savedPath = $"Uploads/{file.FileName}";
        }

        var course = new Course
        {
            Name = dto.Name,
            Price = dto.Price,
            Description = dto.Description,
            ImageBase64 = dto.ImageBase64,
            FileName = file?.FileName,
            FilePath = savedPath,
            Chapters = dto.Chapters.Select(c => new Chapter
            {
                Name = c.Name,
                Topics = c.Topics.Select(t => new Topic { Title = t }).ToList()
            }).ToList()
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Course uploaded successfully" });
    }

}
