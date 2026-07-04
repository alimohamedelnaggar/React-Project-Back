using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using FoodOrdering.Application.Common;
using FoodOrdering.Application.DTOs.Auth;
using FoodOrdering.Application.Interfaces;
using FoodOrdering.Domain.Entities;
using FoodOrdering.Domain.Interfaces;

namespace FoodOrdering.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto registerDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser != null)
            return ApiResponse<AuthResponseDto>.FailureResult("Email already registered");

        var user = new ApplicationUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            FullName = registerDto.FullName
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
            return ApiResponse<AuthResponseDto>.FailureResult(
                "Registration failed", result.Errors.Select(e => e.Description).ToList());

        await _userManager.AddToRoleAsync(user, "Customer");

        var cart = new Cart { UserId = user.Id };
        await _unitOfWork.Carts.AddAsync(cart);
        await _unitOfWork.CompleteAsync();

        var token = await GenerateJwtTokenAsync(user);
        var roles = (await _userManager.GetRolesAsync(user)).ToList();

        var response = new AuthResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email!,
            Roles = roles,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        return ApiResponse<AuthResponseDto>.SuccessResult(response, "Registration successful");
    }

    public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
            return ApiResponse<AuthResponseDto>.FailureResult("Invalid email or password");

        var passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!passwordValid)
            return ApiResponse<AuthResponseDto>.FailureResult("Invalid email or password");

        var token = await GenerateJwtTokenAsync(user);
        var roles = (await _userManager.GetRolesAsync(user)).ToList();

        var response = new AuthResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email!,
            Roles = roles,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        return ApiResponse<AuthResponseDto>.SuccessResult(response, "Login successful");
    }

    private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.FullName)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
