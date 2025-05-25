
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjetoAuvo.Repository.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjetoAuvo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Valida o login com JWT.
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///
        /// </remarks>
        /// <param name="request">Parâmetros de Login</param>
        /// <returns>Valida o login do Usuário</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Login não encontrado</response>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Email == "thiago.artuni@hotmail.com.br" && request.Senha == "123456")
            {
                var claims = new[]
                {
                new Claim(ClaimTypes.Name, request.Email),
                new Claim(ClaimTypes.Role, "User")
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.KMUFsIDTnFmyG3nMiGM6H9FNFUROf3wh7SmqJp-QV30"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "ProjetoAuvo",
                    audience: "ProjetoAuvoUser",
                    claims: claims,
                    expires: DateTime.Now.AddHours(2),
                    signingCredentials: creds);

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(jwt);
            }

            return Unauthorized("Usuário ou senha inválidos");
        }

    }
}
