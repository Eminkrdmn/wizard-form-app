using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WizardFormApi.Data;
using WizardFormApi.DTOs;
using WizardFormApi.Models;

namespace WizardFormApi.Services;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Department)
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null || !user.IsActive)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        var token = GenerateToken(user);

        return new AuthResponse
        {
            Token = token,
            User = MapToUserInfo(user)
        };
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            return null;

        var role = await _context.Roles
            .Include(r => r.Department)
            .FirstOrDefaultAsync(r => r.Id == request.RoleId);

        if (role == null)
            return null;

        var user = new User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            DisplayName = request.DisplayName,
            Email = request.Email,
            RoleId = role.Id,
            DepartmentId = role.DepartmentId
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        user.Role = role;
        user.Department = role.Department;

        var token = GenerateToken(user);

        return new AuthResponse
        {
            Token = token,
            User = MapToUserInfo(user)
        };
    }

    private string GenerateToken(User user)
    {
        var jwtSettings = _config.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("role", user.Role.Code),
            new Claim("role_level", user.Role.Level.ToString()),
            new Claim("department", user.Department.Code),
            new Claim("department_id", user.DepartmentId.ToString()),
            new Claim("role_id", user.RoleId.ToString()),
        };

        var expMinutes = int.Parse(jwtSettings["ExpirationInMinutes"]!);
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static UserInfo MapToUserInfo(User user) => new()
    {
        Id = user.Id,
        Username = user.Username,
        DisplayName = user.DisplayName,
        Email = user.Email,
        RoleName = user.Role.Name,
        RoleCode = user.Role.Code,
        RoleLevel = user.Role.Level,
        DepartmentName = user.Department.Name,
        DepartmentCode = user.Department.Code
    };
}
