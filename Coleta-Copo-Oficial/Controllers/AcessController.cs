using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using static Coleta_Copo_Oficial.Models.AcessModel;
using Copo_Coleta.Data;
using System.Data.Entity;
using Copo_Coleta.Models;
using Microsoft.EntityFrameworkCore;
using Coleta_Copo_Oficial.Models;

namespace Coleta_Copo_Oficial.Controllers
{
    public class AcessController : Controller
    {
        private readonly ILogger<AcessController> _logger;
        private readonly BancoContext _context;
        public AcessController(ILogger<AcessController> logger, BancoContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Login()
        {
            //IDENTIFICADNO SE O USUARIO ESTA VALIDO.
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // FUNÇÃO PARA LOGIN, E GUARDANDO A SESSAO DO USUARIO EM COOKIES
        [HttpPost]
        public async Task<IActionResult> Login([Bind("Nome_Usuario, Senha_Usuario")] AcessModel.Usuario modelLogin)
        {
            try
            {
                if (string.IsNullOrEmpty(modelLogin.Nome_Usuario) || string.IsNullOrEmpty(modelLogin.Senha_Usuario))
                {
                    TempData["Mensagem"] = "Por favor, preencha o nome de usuário e senha.";
                    return View("Login", "Acess");
                }
                else
                {
                    var Nome_Usuario = modelLogin.Nome_Usuario.ToUpper();
                    var Senha_Usuario = modelLogin.Senha_Usuario.ToUpper();

                    var pegarValores = _context.usuario_copy
                    .Where(u => u.Nome_Usuario == Nome_Usuario)
                    .Select(u => new
                    {
                        u.Nome_Usuario,
                        u.Senha_Usuario,
                        u.cargo,
                        u.setor,
                        u.laboratorio,
                        u.nomecompleto
                    })
                    .FirstOrDefault();
                    

                    if (pegarValores != null)
                    {
                        var nomeCompleto = pegarValores.nomecompleto;
                        if (pegarValores.Nome_Usuario == Nome_Usuario && pegarValores.Senha_Usuario == Senha_Usuario)
                        {
                            if (pegarValores.setor == "Especial" && pegarValores.cargo == "Especial" || pegarValores.setor == "TI" && pegarValores.cargo == "TI" || pegarValores.setor == "Qualidade")
                            {
                                List<Claim> claims = new List<Claim>()
                                {
                                new Claim(ClaimTypes.Name, Nome_Usuario),
                                new Claim (ClaimTypes.NameIdentifier, nomeCompleto),
                                new Claim("OtherProperties","Example Role")
                                };

                                ClaimsIdentity claimsIdenty = new ClaimsIdentity(claims,
                                    CookieAuthenticationDefaults.AuthenticationScheme);

                                AuthenticationProperties properties = new AuthenticationProperties()
                                {
                                    AllowRefresh = true,

                                };
                                
                                TempData["Mensagem"] = "Logado Com Sucesso";
                                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdenty), properties);
                                return RedirectToAction("Index", "Home");

                            }
                            else
                            {
                                TempData["Mensagem"] = "Usuário não tem permissão";
                                return View("Login", "Acess");
                            }

                        }
                    }
                    else
                    {
                        TempData["Mensagem"] = "Usuário Errado";
                        return View("Login", "Acess");
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuário", ex.Message);
                throw;
            }

        }

    }
}
