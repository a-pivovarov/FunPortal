using Asp.Versioning;
using FunPortal.Application.DTOs.Auth;
using FunPortal.Application.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FunPortal.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[AllowAnonymous]
public class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Register a new user account
    /// </summary>
    /// <param name="request">The registration details.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created user details.</returns>
    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register a new user", Description = "Creates a new user account with username, email, and password.")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> RegisterAsync(
        [SwaggerParameter(Description = $"The registration details. See {nameof(RegisterUserRequest)} for more information."),
        FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RegisterUserCommand(request), cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>
    /// Login to obtain access and refresh tokens
    /// </summary>
    /// <param name="request">The login credentials.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Access and refresh tokens.</returns>
    [HttpPost("login")]
    [SwaggerOperation(Summary = "Login", Description = "Authenticates a user and returns JWT access and refresh tokens.")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenResponse>> LoginAsync(
        [SwaggerParameter(Description = $"The login credentials. See {nameof(LoginRequest)} for more information."),
        FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new LoginCommand(request), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Refresh access token using refresh token
    /// </summary>
    /// <param name="request">The refresh token.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>New access and refresh tokens.</returns>
    [HttpPost("refresh")]
    [SwaggerOperation(Summary = "Refresh tokens", Description = "Exchanges a refresh token for new access and refresh tokens.")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenResponse>> RefreshAsync(
        [SwaggerParameter(Description = $"The refresh token. See {nameof(RefreshTokenRequest)} for more information."),
        FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RefreshTokenCommand(request), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Revoke a refresh token
    /// </summary>
    /// <param name="request">The refresh token to revoke.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content.</returns>
    [HttpPost("revoke")]
    [SwaggerOperation(Summary = "Revoke token", Description = "Revokes a refresh token to prevent its future use.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RevokeAsync(
        [SwaggerParameter(Description = $"The refresh token to revoke. See {nameof(RefreshTokenRequest)} for more information."),
        FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new RevokeTokenCommand(request), cancellationToken);
        return NoContent();
    }
}
