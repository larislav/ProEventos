using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Identity;

namespace ProEventos.Application
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public readonly SymmetricSecurityKey _key;

        //Injeta o IConfiguration para poder usar a chave segredo
        public TokenService(IConfiguration configuration,
                            UserManager<User> userManager,
                            IMapper mapper) 
        {
            _configuration = configuration;
            _userManager = userManager;
            _mapper = mapper;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));
        }
        public async Task<string> CreateToken(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = _mapper.Map<User>(userUpdateDto);

            //Claims = afirmações sobre o usuario
            //Ex: cpf, assinatura etc
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            //Se adicionar na tabela de roles, as roles que
            //aquele usuario possui, a gente tem que retornar isso no token.
            //Rem que retornar as roles que o usuario possui
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            //vou adicionar esses roles dentro dos claims

            //Montar o token:
            //monto a estrutura de descrição do meu token
            //ou seja, quais sao as claims, como é constituido o token em 
            //relação as claims
            //Para isso, instalar o System.IdentityModel.Tokens.Jwt 
            //e também o Microsoft.Identity.Model , esses no application
            var credenciais = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credenciais
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            var tokenCriado = tokenHandler.WriteToken(token);
            return tokenCriado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            
        }
    }
}