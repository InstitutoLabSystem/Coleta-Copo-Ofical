using Microsoft.EntityFrameworkCore;
using System.Linq;
using Copo_Coleta.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Copo_Coleta.Data;
using MySqlConnector;
using System.Reflection.Metadata;
using static Azure.Core.HttpHeader;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Copo_Coleta.Models.HomeModel;


namespace Copo_Coleta.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BancoContext _context;
        private readonly IHttpContextAccessor _contextAcessor;

        public HomeController(ILogger<HomeController> logger, BancoContext context, IHttpContextAccessor contextAcessor)
        {
            _logger = logger;
            _context = context;
            _contextAcessor = contextAcessor;
        }

        //Meu index.
        public IActionResult Index()
        {
            _contextAcessor.HttpContext.Session.SetString("Nome", "usuario");

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
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
                  .OrderBy(os=>os.item)
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
                _logger.LogError(ex, "Erro ao buscar orçamento: {0}", ex.Message);
                throw;
            }
        }

        public async Task<IActionResult> LoginBuscar([Bind("Nome_Usuario, Senha_Usuario")] HomeModel.Usuario salvar)
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
                    var Nome_Usuario = salvar.Nome_Usuario;
                    var Senha_Usuario = salvar.Senha_Usuario;

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

                    if (Nome_Usuario == pegarValores.Nome_Usuario && Senha_Usuario == pegarValores.Senha_Usuario)
                    {
                        if(pegarValores.setor == "Especial" && pegarValores.cargo == "Especial" || pegarValores.setor == "TI" && pegarValores.cargo == "TI")
                        {
                           
                            TempData["Mensagem"] = "foi";
                            return View("Index");
                        }
                        else
                        {
                            TempData["Mensagem"] = "Usuário não encontrado.";
                            return View("Login");
                        }

                    }
                    else
                    {
                        TempData["Mensagem"] = "Usuário não encontrado.";
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}