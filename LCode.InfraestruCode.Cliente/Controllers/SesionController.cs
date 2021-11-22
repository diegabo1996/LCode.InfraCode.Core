using BCP.NETCore.Base;
using LCode.InfraestruCode.Cliente.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NETCore.Base._3._0;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LCode.InfraestruCode.Cliente.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SesionController : ControllerBase
    {
        private List<string> ObtenerRoles(System.DirectoryServices.ResultPropertyValueCollection Grupos)
        {
            List<string> Roles = new List<string>();
            bool Desarrollo = DirectorioActivo.ValidacionGrupo(Grupos, "DESARROLLO");
            if (Desarrollo)
            {
                Roles.Add("Usuario-Desarrollo");
            }
            bool Certificacion = DirectorioActivo.ValidacionGrupo(Grupos, "CERTIFICACION");
            if (Certificacion)
            {
                Roles.Add("Usuario-Certificacion");
            }
            bool ServidoresMonitoreo = DirectorioActivo.ValidacionGrupo(Grupos, "GDMonitoreo");
            if (ServidoresMonitoreo)
            {
                Roles.Add("Admin");
            }
            return Roles;
        }
        // POST api/<SesionAController>
        [HttpPost]
        public IActionResult Post([FromBody] Sesion value)
        {
            try
            {
                BaseConfiguracion BC = new BaseConfiguracion();
                string Llave = BC.ObtenerValor("SecretKey");
                var key = Base64UrlEncoder.DecodeBytes(Llave);
                try
                {
                    DirectorioActivo DA = new DirectorioActivo();
                    if (DA.ValidarUsrContrasena(value.Usuario, value.Contrasenia))
                    {
                        var One = DA.ObtenerInformacion(value.Usuario, value.Contrasenia);
                        var Roles=ObtenerRoles(One.Properties["memberof"]);
                        if (Roles.Count>0)
                        { 
                            //DatosSesion Datos = new DatosSesion();
                            //Datos.Usuario = One.Properties["samaccountname"][0].ToString();
                            //Datos.Nombres = One.Properties["givenname"][0].ToString();
                            //Datos.Apellidos = One.Properties["sn"][0].ToString();
                            //Datos.Correo = One.Properties["userprincipalname"][0].ToString();
                            string guidsesion = Guid.NewGuid().ToString();
                            var claims = new ClaimsIdentity(new[]
                            {
                                new Claim(ClaimTypes.Name, One.Properties["givenname"][0].ToString()),
                                new Claim(ClaimTypes.Surname, One.Properties["sn"][0].ToString()),
                                new Claim(ClaimTypes.WindowsUserClaim, One.Properties["samaccountname"][0].ToString()),
                                new Claim(ClaimTypes.Email, One.Properties["userprincipalname"][0].ToString()),
                                new Claim(ClaimTypes.Sid, guidsesion),
                                new Claim(ClaimTypes.PrimarySid, guidsesion+";"+value.Usuario),
                            });
                            foreach (string Rol in Roles)
                            {
                                claims.AddClaim(new Claim(ClaimTypes.Role, Rol));
                            }
                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = claims,
                                Expires = DateTime.UtcNow.AddMinutes(60),
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                            };

                            var tokenHandler = new JwtSecurityTokenHandler();
                            var createdToken = tokenHandler.CreateToken(tokenDescriptor);
                            string token = tokenHandler.WriteToken(createdToken);
                            Response.Cookies.Append("token", token);
                            return Ok(JsonConvert.SerializeObject(token));
                        }
                        else
                        {
                            return Unauthorized();
                        }
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                catch (Exception Ex)
                {
                    return StatusCode(500);
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }
        //public string Generate()
        //{
        //    List<Claim> claims = new List<Claim>() {
        //    new Claim (JwtRegisteredClaimNames.Jti,
        //        Guid.NewGuid().ToString()),

        //    new Claim (JwtRegisteredClaimNames.Email,
        //        user.EmailAddress),

        //    new Claim (JwtRegisteredClaimNames.Sub,
        //        user.Id.ToString()),
		    
        //    // Add the ClaimType Role which carries the Role of the user
        //    new Claim (ClaimTypes.Role, user.Role)
        //    };

        //    JwtSecurityToken token = new TokenBuilder()
        //        .AddAudience(TokenConstants.Audience)
        //        .AddIssuer(TokenConstants.Issuer)
        //        .AddExpiry(TokenConstants.ExpiryInMinutes)
        //        .AddKey(TokenConstants.key)
        //        .AddClaims(claims)
        //        .Build();

        //    string accessToken = new JwtSecurityTokenHandler()
        //        .WriteToken(token);

        //    return accessToken;
        //}

    }
}
