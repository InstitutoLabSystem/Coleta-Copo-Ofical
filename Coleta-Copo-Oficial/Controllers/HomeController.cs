using Microsoft.EntityFrameworkCore;
using Copo_Coleta.Models;
using Microsoft.AspNetCore.Mvc;
using Copo_Coleta.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Copo_Coleta.Models.HomeModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Copo_Coleta.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BancoContext _context;
        private readonly CoposContext _copos;
        public HomeController(ILogger<HomeController> logger, BancoContext context, CoposContext copos)
        {
            _logger = logger;
            _context = context;
            _copos = copos;
        }

        //Meu index.
        public IActionResult Index()
        {
            //PEGANDO O NOME DA SESSAO DO USUARIO. E MOSTRANDO NO INDEX.
            var nomeUsuarioClaim = User.FindFirstValue(ClaimTypes.Name);
            ViewBag.NomeUsuario = nomeUsuarioClaim;
            return View();
        }
        public IActionResult Privacy()
        {
            var retornarFinalizadas = _copos.copos_finalizado
                                      .ToList();

            return View(retornarFinalizadas);
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acess");

        }

        //Pesquisar orçamento...
        [HttpPost]
        public IActionResult BuscarOrcamento(string orcamento)
        {
            // exucutar query consulta;
            try
            {

                var resultado = _context.ordemservico_copylab
                  .Where(os => os.orcamento == orcamento)
                  .OrderBy(os => os.item)
                  .Select(os => new OrdemServico // Substitua 'OrdemServico' pela classe correta
                  {
                      Rev = os.Rev,
                      orcamento = os.orcamento,
                      item = os.item,
                      codigo = os.codigo,
                      mes = os.mes,
                      ano = os.ano
                  })
                  .ToList();

                //verificar se tem algo preenchido..
                if (resultado.Count > 0)
                {
                    return View("Index", resultado);
                }
                else
                {
                    TempData["Mensagem"] = "Orçamento não encontrado.";
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar orçamento: {}", ex.Message);
                throw;
            }
        }
    }
}