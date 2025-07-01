// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using theUpSkilzAPI.Data;
using theUpSkilzAPI.Dtos;
using theUpSkilzAPI.Models;
using theUpSkilzAPI.Services;
using theUpSkilzAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using BCrypt.Net;



[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly SmsService _smsService;
    private readonly JwtService _jwtService;
    private readonly IConfiguration _config;


    public AuthController(AppDbContext context, SmsService smsService, JwtService jwtService , IConfiguration config)
    {
        _context = context;
        _smsService = smsService;
        _jwtService = jwtService;
        _config = config;
    }
   
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (_context.Users.Any(u => u.Email == dto.Email))
            return BadRequest(new { message = "Email already exists" });

        var role = dto.Email.ToLower() == "krishnakothapalle@gmail.com" ? "admin" : "user";

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = AuthService.HashPassword(dto.Password),
            Role = role,
            PhoneNumber = dto.PhoneNumber
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "User registered successfully", role = user.Role });
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            return Unauthorized("Invalid email or password");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["JwtSettings:SecretKey"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new
        {
            message = "Login successful",
            token = tokenString,
            user = new
            {
                id = user.Id,
                email = user.Email,
                username = user.Username,
                role = user.Role
            }
        });
    }


    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOtp([FromBody] string phone)
    {
        var normalizedPhone = PhoneUtils.NormalizePhone(phone);
        var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == normalizedPhone);
        if (user == null)
            return NotFound("Phone number not registered.");

        var otp = new Random().Next(100000, 999999).ToString();
        OtpStore.SetOtp(normalizedPhone, otp);

        await _smsService.SendOtp(normalizedPhone, otp);
        return Ok("OTP sent to your mobile number.");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        var normalizedPhone = PhoneUtils.NormalizePhone(dto.PhoneNumber);
        var savedOtp = OtpStore.GetOtp(normalizedPhone);
        if (savedOtp != dto.Otp)
            return BadRequest("Invalid OTP.");

        var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == normalizedPhone);
        if (user == null)
            return NotFound("User not found.");

        user.PasswordHash = AuthService.HashPassword(dto.NewPassword);
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        OtpStore.ClearOtp(normalizedPhone);
        return Ok("Password reset successfully.");
    }

    [HttpPost("get-phone-by-email")]
    public IActionResult GetPhoneByEmail([FromBody] EmailDto dto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == dto.Email.ToLower());

        if (user == null || string.IsNullOrWhiteSpace(user.PhoneNumber))
            return NotFound(new { message = "No phone number associated with this email." });

        return Ok(new { phoneNumber = user.PhoneNumber });
    }

    [HttpPost("promote-admin")]
    public async Task<IActionResult> PromoteToAdmin()
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == "krishnakothapalle@gmail.com");

        if (user == null)
        {
            return NotFound("User not found.");
        }
        user.Role = "admin";
        await _context.SaveChangesAsync();

        return Ok("User promoted to admin.");
    }



}
