using instagram.DB;
using instagram.DB.Moduls;
using instagram.DTO;
using instagram.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace instagram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly UserManager<User> _userManager;
        readonly ILogger<UserController> _logger;   
        readonly IConfiguration _configuration;
        readonly IEmailService _emailService;
        readonly AppDBContext _context;
        public UserController(AppDBContext context,UserManager<User> userManager , ILogger<UserController> logger, IConfiguration configuration, IEmailService emailService)
        {
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
            _emailService = emailService;
            _context = context;
        }


        [HttpPost("register")]
        public async Task <IActionResult> Register (registerDto re)
        {
            
            if(!ModelState.IsValid) return BadRequest(ModelState.ErrorCount);
            var User = new User
            {
                First_Name = re.First_Name,
                Last_Name = re.Last_Name,
                UserName = re.UserName,
                Email = re.Email,
            };
            var result = await _userManager.CreateAsync(User, re.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);



            return Created();

        }

        [HttpPost("Login")]
        public async Task <IActionResult> Login(loginDto lo)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid login attempt.\n"+"Email: " +lo.Email +"\nPassword: "+lo.Password);
                return BadRequest(ModelState.ErrorCount);
            }
                
            var user = await _userManager.FindByEmailAsync(lo.Email);
            if (user == null)
            {
                _logger.LogWarning("Email is not Fount.\n" + "Email: " + lo.Email);
                return BadRequest("Invalid login attempt.");

            }
            var result= await _userManager.CheckPasswordAsync(user, lo.Password);

            if (!result) 
            {
                _logger.LogWarning("Invalid password for user {Email}\n" + "Email: " + lo.Email );


                return BadRequest("Invalid login attempt.");


            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName!),
        new Claim(ClaimTypes.Email, user.Email!)
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(2),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            _logger.LogInformation("User {Email} logged in successfully.", lo.Email);
            return Ok(new
            {
                Token = jwt,
                Expire = token.ValidTo,
                UserID = user.Id,
            });
        }
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid forgot password request.\n" + "Email: " + dto.Email);
                return BadRequest(ModelState.ErrorCount);
            }
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                _logger.LogWarning("Email not found for forgot password request.\n" + "Email: " + dto.Email);
                return BadRequest("Invalid email address.");
            }
            var random = new Random();
            var otp = random.Next(100000, 999999).ToString();
            var oldotp = await _context.EmailOtps.FirstOrDefaultAsync(e => e.Email == dto.Email);
            if (oldotp != null)
            {
                _context.EmailOtps.Remove(oldotp);
            }
            _context.EmailOtps.Add(new EmailOtp
            {
                Email = dto.Email,
                Otp = otp,
                ExpirationTime = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            });

            await _context.SaveChangesAsync();

            await _emailService.SendEmailAsync(
                dto.Email, "Password Reset OTP", $"Your OTP for password reset is: {otp}. It will expire in 5 minutes.");
            return Ok("OTP sent to your email address.");
        }

        [HttpPost("VerifOtp")]
        public async Task<IActionResult> VerifOtp(VerifyOtpDTO dto)
        {
            var otp = await _context.EmailOtps.FirstOrDefaultAsync
                (e => e.Email == dto.Email && e.Otp == dto.Dto);
            if (otp == null)
            {
                _logger.LogWarning("Invalid OTP verification attempt.\n" + "Email: " + dto.Email + "\nOTP: " + dto.Dto);
                return BadRequest("Invalid OTP.");
            }
            if(otp.IsUsed == true)
            {
                _logger.LogWarning("OTP already used for verification.\n" + "Email: " + dto.Email + "\nOTP: " + dto.Dto);
                return BadRequest("OTP has already been used.");
            }
            if(otp.ExpirationTime < DateTime.UtcNow)
            {
                _logger.LogWarning("Expired OTP verification attempt.\n" + "Email: " + dto.Email + "\nOTP: " + dto.Dto);
                return BadRequest("OTP has expired.");
            }
            otp.IsUsed = true;
            await _context.SaveChangesAsync();
            return Ok("OTP verified successfully.");
        }

        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                _logger.LogWarning("Invalid password reset attempt.\n" + "Email: " + dto.Email);
                return BadRequest("Invalid email address.");
            }
            var otp = await _context.EmailOtps.FirstOrDefaultAsync(e => e.Email == dto.Email && e.IsUsed );
            if (otp == null)
            {
                _logger.LogWarning("OTP not verified for password reset.\n" + "Email: " + dto.Email);
                return BadRequest("OTP has not been verified.");
            }

            if (dto.newPassword != dto.ConfirmPassword)
            {
                _logger.LogWarning("Password and confirm password do not match for user {Email}.\n" + "Email: " + dto.Email);
                return BadRequest("Password and confirm password do not match.");
            }
            if (otp.ExpirationTime < DateTime.UtcNow)
            {
                return BadRequest("OTP has expired.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, dto.newPassword);

            if (!result.Succeeded)
            {
                _logger.LogWarning("Password reset failed for user {Email}.\n" + "Email: " + dto.Email);
                return BadRequest(result.Errors);
            }
            _context.EmailOtps.Remove(otp);
            await _context.SaveChangesAsync();
            return Ok("Password reset successfully.");

        }


    }
}
