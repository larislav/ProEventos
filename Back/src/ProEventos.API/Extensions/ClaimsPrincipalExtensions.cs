using System.Security.Claims;

namespace ProEventos.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        //para criar método de extensão,
        //a classe também deve ser estática /\
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}