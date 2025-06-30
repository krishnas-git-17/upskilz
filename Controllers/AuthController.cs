// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using theUpSkilzAPI.Data;
using theUpSkilzAPI.Dtos;
using theUpSkilzAPI.Models;
using theUpSkilzAPI.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    //private readonly SmsService _smsService;

    public AuthController(AppDbContext context, SmsService smsService)
    {
        _context = context;
        //_smsService = smsService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (_context.Users.Any(u => u.Email == dto.Email))
            return BadRequest(new { message = "Email already exists" });

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = AuthService.HashPassword(dto.Password),
            Role = "user",
            PhoneNumber = dto.PhoneNumber // <-- Save phone number
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
        if (user == null)
            return NotFound(new { message = "User not found" });

        var isValid = AuthService.VerifyPassword(dto.Password, user.PasswordHash);
        if (!isValid)
            return BadRequest(new { message = "Invalid credentials" });

        return Ok(new
        {
            message = "Login successful",
            user = new
            {
                user.Id,
                user.Username,
                user.Email,
                user.Role
            }
        });
    }

    //[HttpPost("send-otp")]
    //public async Task<IActionResult> SendOtp([FromBody] string phone)
    //{
    //    var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == phone);
    //    if (user == null)
    //        return NotFound("Phone number not registered.");

    //    var otp = new Random().Next(100000, 999999).ToString();
    //    OtpStore.SetOtp(phone, otp);

    //    await _smsService.SendOtp(phone, otp);
    //    return Ok("OTP sent to your mobile number.");
    //}

    //[HttpPost("reset-password")]
    //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    //{
    //    var savedOtp = OtpStore.GetOtp(dto.PhoneNumber);
    //    if (savedOtp != dto.Otp)
    //        return BadRequest("Invalid OTP.");

    //    var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == dto.PhoneNumber);
    //    if (user == null)
    //        return NotFound("User not found.");

    //    user.PasswordHash = AuthService.HashPassword(dto.NewPassword);
    //    _context.Users.Update(user);
    //    await _context.SaveChangesAsync();

    //    OtpStore.ClearOtp(dto.PhoneNumber);
    //    return Ok("Password reset successfully.");
    //}
}
