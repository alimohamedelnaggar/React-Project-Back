using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using React.BLL.Common;
using React.BLL.DTOs.Cart;
using React.BLL.Interfaces;

namespace React.API.Controllers.Customer;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<CartDto>>> GetCart()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var response = await _cartService.GetCartAsync(userId);
        return Ok(response);
    }

    [HttpPost("add")]
    public async Task<ActionResult<ApiResponse<CartDto>>> AddToCart([FromBody] AddToCartDto addToCartDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var response = await _cartService.AddToCartAsync(userId, addToCartDto);
        return Ok(response);
    }

    [HttpPut("update")]
    public async Task<ActionResult<ApiResponse<CartDto>>> UpdateCartItem([FromBody] UpdateCartItemDto updateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var response = await _cartService.UpdateCartItemAsync(userId, updateDto);
        return Ok(response);
    }

    [HttpDelete("remove/{cartItemId}")]
    public async Task<ActionResult<ApiResponse>> RemoveFromCart(int cartItemId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var response = await _cartService.RemoveFromCartAsync(userId, cartItemId);
        return Ok(response);
    }

    [HttpDelete("clear")]
    public async Task<ActionResult<ApiResponse>> ClearCart()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var response = await _cartService.ClearCartAsync(userId);
        return Ok(response);
    }
}
