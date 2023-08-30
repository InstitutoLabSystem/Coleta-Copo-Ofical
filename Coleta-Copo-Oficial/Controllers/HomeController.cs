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
            return View();
        }
        public IActionResult Privacy()
        {
            var retornarFinalizadas = _copos.copos_finalizado
                                      .ToList();

            return View(retornarFinalizadas);
        }
        public IActionResult Login()
        {
            return View();
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

        [HttpPost]
        public async Task<IActionResult> LoginBuscar(string ReturnUrl,[Bind("Nome_Usuario, Senha_Usuario")] HomeModel.Usuario salvar)
        {
            try
            {
                if (string.IsNullOrEmpty(salvar.Nome_Usuario) || string.IsNullOrEmpty(salvar.Senha_Usuario))
                {
                    TempData["Mensagem"] = "Por favor, preencha o nome de usuário e senha.";
                    return View("Login");
                }
                else
                {
                    var Nome_Usuario = salvar.Nome_Usuario.ToUpper();
                    var Senha_Usuario = salvar.Senha_Usuario.ToUpper();

                    var pegarValores = await _context.usuario_copy
                        .Where(u => u.Nome_Usuario == Nome_Usuario)
                        .Select(u => new
                        {
                            u.Nome_Usuario,
                            u.Senha_Usuario,
                            u.cargo,
                            u.setor,
                            u.laboratorio
                        })
                        .FirstOrDefaultAsync();

                    if (pegarValores != null)
                    {
                        if (pegarValores.Nome_Usuario == Nome_Usuario && pegarValores.Senha_Usuario == Senha_Usuario)
                        {
                            if (pegarValores.setor == "Especial" && pegarValores.cargo == "Especial" || pegarValores.setor == "TI" && pegarValores.cargo == "TI")
                            {
                                TempData["Mensagem"] = "Logado com sucesso";
                                return View("Index");
                            }
                            else
                            {
                                TempData["Mensagem"] = "Usuário não tem permissão";
                                return View("Login");
                            }

                        }
                        else
                        {
                            TempData["Mensagem"] = "Usuário não encontrado.";
                            return View("Login");
                        }
                    }
                    else
                    {
                        TempData["Mensagem"] = "Usuário Errado";
                        return View("Login");
                    }

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