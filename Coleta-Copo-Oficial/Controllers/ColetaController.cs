using Copo_Coleta.Data;
using Copo_Coleta.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.WebEncoders.Testing;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using System.Data.Entity;
using System.Numerics;
using static Copo_Coleta.Models.ColetaModel;
using static Copo_Coleta.Models.HomeModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics.Metrics;
using Microsoft.CodeAnalysis.Differencing;
using System.Xml.Linq;
using Coleta_Copo_Oficial.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Copo_Coleta.Controllers
{
    [Authorize]
    public class ColetaController : Controller

    {
        //private readonly CoposContext _context;
        private readonly ILogger<ColetaController> _logger;
        private readonly CoposContext _context;
        private readonly BancoContext _bancocontext;
        private readonly IncertezaContext _incertezacontext;

        public ColetaController(ILogger<ColetaController> logger, CoposContext context, BancoContext bccontext, IncertezaContext incertezacontext)
        {
            _logger = logger;
            _context = context;
            _bancocontext = bccontext;
            _incertezacontext = incertezacontext;
        }

        public ActionResult Index(int os, string orcamento, int Rev)
        {
            var model = new ColetaViewModel();
            model.oDescricao = ObterDescricao(os, Rev);
            model.oDatas = ObterDatas(os, Rev);
            model.oAspectosvisuais = ObterAspectosvisuais(os, Rev);
            model.oMassa = Obtermassa(os, Rev);
            model.oTablemassa = ObterTablemassa(os, Rev);
            model.oCompressao = ObterCompressao(os, Rev);
            model.oAmostra = ObterAmostra(os, Rev);
            model.oEmbalagem = ObterEmbalagem(os, Rev);
            model.oMarcacao = ObterMarcacao(os, Rev);
            model.oMateriais = ObterMateriais(os, Rev);
            model.oCondicionamento = ObterCondicionamento(os, Rev);
            model.oCondicionamentoMaximo = ObterCondicionamentoMaximo(os, Rev);
            model.oCondInstrumentos = ObterInstrumentoCond(os, Rev);
            model.oInstrumentosMassa = ObterInstrumentoMassa(os, Rev);
            model.oInstrumentosCompressao = ObterInstrumentoCompressao(os, Rev);

            //pegando nome do usuario da sessao por viewbag
            var nomeUsuarioClaim = User.FindFirstValue(ClaimTypes.Name);
            var nomeCompletoClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.NomeUsuario = nomeUsuarioClaim;
            ViewBag.NomeCompleto = nomeCompletoClaim;

            ViewBag.OS = os;
            ViewBag.Orcamento = orcamento;
            ViewBag.Rev = Rev;
            return View(model);
        }


        private Datas ObterDatas(int os, int Rev)
        {
            var datas = _context.copo_datas
                   .Where(x => x.os == os && x.Rev == Rev)
                   .FirstOrDefault();
            return datas;
        }

        private Descricao ObterDescricao(int os, int Rev)
        {
            var osDescricao = os;
            var pegarValoresDescricao = _context.copos_descricao
                .Where(item => item.os == osDescricao && item.rev == Rev)
                .Select(item => new Descricao
                {
                    qtd_recebida = item.qtd_recebida,
                    qtd_ensaiada = item.qtd_ensaiada,
                    capacidade_copo = item.capacidade_copo,
                    quant_manga = item.quant_manga,
                    capacidade_manga = item.capacidade_manga,
                    executador = item.executador,
                    auxiliador = item.auxiliador,
                    resp_conf = item.resp_conf,
                    fator_correlacao = item.fator_correlacao,
                    especificada = item.especificada

                })
                .FirstOrDefault();

            if (pegarValoresDescricao != null)
            {
                return pegarValoresDescricao;
            }
            else
            {
                return null;
            }
        }
        private Aspectosvisuais ObterAspectosvisuais(int os, int Rev)
        {
            var Visuais = _context.copos_aspectosvisuais
                     .Where(x => x.os == os && x.Rev == Rev)
                     .FirstOrDefault();
            return Visuais;
        }
        private List<Massa> Obtermassa(int os, int Rev)
        {
            var massa = _context.copos_massa
                   .Where(x => x.os == os && x.rev == Rev)
                   .ToList();
            return massa;
        }
        private Tablemassa ObterTablemassa(int os, int Rev)
        {
            var tablemassa = _context.copos_tablemassa
                    .Where(x => x.os == os && x.Rev == Rev)
                    .FirstOrDefault();
            return tablemassa;
        }
        private Compressao ObterCompressao(int os, int Rev)
        {
            var compressao = _context.copos_compressao
                     .Where(x => x.os == os && x.rev == Rev)
                     .FirstOrDefault();
            return compressao;
        }
        private List<Amostra> ObterAmostra(int os, int Rev)
        {
            var Amostras = _context.copos_amostra
                     .Where(x => x.os == os && x.rev == Rev)
                     .ToList();
            return Amostras;
        }
        private Embalagem ObterEmbalagem(int os, int Rev)
        {
            var embalagem = _context.copos_embalagem
                    .Where(x => x.os == os && x.rev == Rev)
                    .FirstOrDefault();
            return embalagem;
        }
        private List<Marcacao> ObterMarcacao(int os, int Rev)
        {
            var marcacao = _context.copos_marcacao
                     .Where(x => x.os == os && x.rev == Rev)
                     .ToList();
            return marcacao;
        }
        private Materiais ObterMateriais(int os, int Rev)
        {
            var materiais = _context.copos_materiais
                    .Where(x => x.os == os && x.rev == Rev)
                    .FirstOrDefault();
            return materiais;
        }


        private CondicionamentoMinimo ObterCondicionamento(int os, int Rev)
        {
            var obterCond = _context.copos_codicionamento_minimo
                            .Where(x => x.os == os && x.rev == Rev)
                            .FirstOrDefault();
            return obterCond;
        }
        private CondicMaximo ObterCondicionamentoMaximo(int os, int Rev)
        {
            var obterCondMaximo = _context.copos_codicionamento_maximo
                            .Where(x => x.os == os && x.rev == Rev)
                            .FirstOrDefault();
            return obterCondMaximo;
        }

        private List<instrumentosCond> ObterInstrumentoCond(int os, int Rev)
        {
            var trazerInstrumentoCond = _context.instrumentos_cond
                      .Where(x => x.os == os && x.rev == Rev)
                      .ToList();
            return trazerInstrumentoCond;
        }
        private List<instrumentosMassa> ObterInstrumentoMassa(int os, int Rev)
        {
            var trazerInstrumentoMassa = _context.instrumentos_massa
                      .Where(x => x.os == os && x.rev == Rev)
                      .ToList();
            return trazerInstrumentoMassa;
        }
        private List<instrumentosCompressao> ObterInstrumentoCompressao(int os, int Rev)
        {
            var trazerInstrumentoComp = _context.instrumentos_compressao
                      .Where(x => x.os == os && x.rev == Rev)
                      .ToList();
            return trazerInstrumentoComp;
        }



        [HttpPost]
        public async Task<IActionResult> SalvarData(int os, string orcamento, string massamin, string fatcorrelacao, int Rev, [Bind("data_de_início,data_de_termino,executador,auxiliador,resp_conf")] ColetaModel.Datas salvar, [Bind("qtd_recebida,qtd_ensaiada,capacidade_copo,quant_manga,capacidade_manga,executador,auxiliador,resp_conf")] ColetaModel.Descricao descricao)
        {
            try
            {
                if (orcamento != null)
                {
                    //pegando o resultado da tabela salvar.
                    var data_de_início = salvar.data_de_início;
                    var data_de_termino = salvar.data_de_termino;
                    string executadorData = salvar.executador;
                    string auxiliadorData = salvar.auxiliador;
                    string res_confData = salvar.resp_conf;

                    //pegando o resultado da tabela descricao.
                    var qtd_recebida = descricao.qtd_recebida;
                    var qtd_ensaiada = descricao.qtd_ensaiada;
                    var capacidade_copo = descricao.capacidade_copo;
                    var quant_manga = descricao.quant_manga;
                    var capacidade_manga = descricao.capacidade_manga;
                    string executador = descricao.executador;
                    string auxiliador = descricao.auxiliador;
                    string res_conf = descricao.resp_conf;

                    //verificando se os campos estao vazios.
                    if (qtd_recebida == null || qtd_ensaiada == null || capacidade_copo == null || quant_manga == null || capacidade_manga == null)
                    {
                        TempData["Mensagem"] = "Preencha todos os campos para salvar";
                        return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                    }

                    // Aqui, você pode carregar os dados existentes do banco de dados e atualizar as propriedades necessárias
                    var dadosExistente = _context.copo_datas.FirstOrDefault(d => d.os == os && d.orcamento == orcamento && d.Rev == Rev);
                    if (dadosExistente != null)
                    {
                        dadosExistente.data_de_início = data_de_início;
                        dadosExistente.data_de_termino = data_de_termino;
                        dadosExistente.os = os;
                        dadosExistente.orcamento = orcamento;
                        dadosExistente.Rev = Rev;
                        dadosExistente.editar = 1;

                        _context.SaveChanges();
                        await _context.SaveChangesAsync();
                        TempData["Mensagem"] = "Dados Editados com sucesso!!";

                    }
                    else
                    {
                        //guardando os valores de inicio e termino.
                        var salvarDados = new Datas
                        {
                            data_de_início = data_de_início,
                            data_de_termino = data_de_termino,
                            os = os,
                            orcamento = orcamento,
                            Rev = Rev,
                            executador = executadorData,
                            auxiliador = auxiliadorData,
                            resp_conf = res_confData,
                            editar = 1
                        };
                        _context.Add(salvarDados);
                        await _context.SaveChangesAsync();
                        TempData["Mensagem"] = "Dados salvos com sucesso!!";

                    }

                    var descricaoExistente = _context.copos_descricao.FirstOrDefault(d => d.os == os && d.rev == Rev);
                    if (descricaoExistente != null)

                    {
                        double capacidadecopo = double.Parse(capacidade_copo); 

                        descricaoExistente.qtd_recebida = qtd_recebida;
                        descricaoExistente.qtd_ensaiada = qtd_ensaiada;
                        descricaoExistente.capacidade_copo = capacidade_copo;
                        descricaoExistente.quant_manga = quant_manga;
                        descricaoExistente.capacidade_manga = capacidade_manga;

                        if (capacidadecopo == 1 && capacidadecopo < 50)
                        {
                            descricaoExistente.fator_correlacao = "0,00150";

                            double correlacao;
                            correlacao = Double.Parse(fatcorrelacao);
                            double resultado = capacidadecopo * correlacao;
                            massamin = resultado.ToString("0.000");
                            descricaoExistente.especificada = massamin;

                        }
                        if (capacidade_copo == "50")
                        {
                            massamin = "0,75";
                            descricaoExistente.fator_correlacao = "0,0150";
                            descricaoExistente.especificada = massamin;
                        }
                        if (capacidadecopo >= 51 && capacidadecopo <= 149 && capacidadecopo != 80 && capacidadecopo != 110)
                        {
                            fatcorrelacao = "0,00175";

                            double correlacao;
                            correlacao = Double.Parse(fatcorrelacao);
                            double resultado = capacidadecopo * correlacao;
                            massamin = resultado.ToString("0.000");
                            descricaoExistente.especificada = massamin;
                            descricaoExistente.fator_correlacao = fatcorrelacao;

                        }
                        if (capacidade_copo == "80")
                        {
                            massamin = "1,40";
                            fatcorrelacao = "0,0175";
                            descricaoExistente.especificada = massamin;
                            descricaoExistente.fator_correlacao = fatcorrelacao;
                        }
                        if (capacidade_copo == "110")
                        {
                            massamin = "1,90";
                            fatcorrelacao = "0,0175";
                            descricaoExistente.especificada = massamin;
                            descricaoExistente.fator_correlacao = fatcorrelacao;
                        }
                        if (capacidade_copo == "150")
                        {
                            massamin = "1,35";
                            fatcorrelacao = "0,0090";
                            descricaoExistente.especificada = massamin;
                            descricaoExistente.fator_correlacao = fatcorrelacao;
                        }
                        if (capacidadecopo >= 151 && capacidadecopo <= 299 && capacidadecopo != 180 && capacidadecopo != 200 && capacidadecopo != 250)
                        {
                            fatcorrelacao = "0,0090";

                            double correlacao;
                            correlacao = Double.Parse(fatcorrelacao);
                            double resultado = capacidadecopo * correlacao;
                            massamin = resultado.ToString("0.000");
                            descricaoExistente.especificada = massamin;
                            descricaoExistente.fator_correlacao = fatcorrelacao;

                        }
                        if (capacidade_copo == "180")
                        {
                            massamin = "1,62";
                            fatcorrelacao = "0,0090";
                            descricaoExistente.especificada = massamin;
                            descricaoExistente.fator_correlacao = fatcorrelacao;
                        }
                        if (capacidade_copo == "200")
                        {
                            massamin = "1,80";
                            fatcorrelacao = "0,0090";
                            descricaoExistente.especificada = massamin;
                            descricaoExistente.fator_correlacao = fatcorrelacao;
                        }
                        if (capacidade_copo == "250")
                        {
                            massamin = "2,25";
                            fatcorrelacao = "0,0090";
                            descricaoExistente.especificada = massamin;
                            descricaoExistente.fator_correlacao = fatcorrelacao;
                        }
                        if (capacidade_copo == "300")
                        {
                            massamin = "2,70";
                            fatcorrelacao = "0,0090";
                            descricaoExistente.especificada = massamin;
                            descricaoExistente.fator_correlacao = fatcorrelacao;
                        }
                        if (capacidadecopo >= 301 && capacidadecopo <= 329)
                        {
                            fatcorrelacao = "0,0110";

                            double correlacao;
                            correlacao = Double.Parse(fatcorrelacao);
                            double resultado = capacidadecopo * correlacao;
                            massamin = resultado.ToString("0.000");
                            descricaoExistente.especificada = massamin;
                            descricaoExistente.fator_correlacao = fatcorrelacao;

                        }
                        if (capacidade_copo == "330")
                        {
                            massamin = "3,63";
                            fatcorrelacao = "0,0110";
                            descricaoExistente.especificada = massamin;
                            descricaoExistente.fator_correlacao = fatcorrelacao;
                        }
                        if (capacidadecopo >= 331 && capacidadecopo <= 549 && capacidadecopo != 400 && capacidadecopo != 440 &&
                            capacidadecopo != 500)
                        {
                            fatcorrelacao = "0,0126";

                            double correlacao;
                            correlacao = Double.Parse(fatcorrelacao);
                            double resultado = capacidadecopo * correlacao;
                            massamin = resultado.ToString("0.000");
                            descricaoExistente.especificada = massamin;
                            descricaoExistente.fator_correlacao = fatcorrelacao;

                        }
                        if (capacidade_copo == "400")
                        {
                            massamin = "5,00";
                            fatcorrelacao = "0,0126";
                            descricaoExistente.especificada = massamin;
                            descricaoExistente.fator_correlacao = fatcorrelacao;
                        }
                        if (capacidade_copo == "440")
                        {
                            massamin = "5,54";
                            fatcorrelacao = "0,0126";
                            descricaoExistente.especificada = massamin;
                            descricaoExistente.fator_correlacao = fatcorrelacao;
                        }
                        if (capacidade_copo == "500")
                        {
                            massamin = "6,30";
                            fatcorrelacao = "0,0126";
                            descricaoExistente.especificada = massamin;
                            descricaoExistente.fator_correlacao = fatcorrelacao;
                        }
                        if (capacidade_copo == "550")
                        {
                            massamin = "6,93";
                            fatcorrelacao = "0,0126";
                            descricaoExistente.especificada = massamin;
                            descricaoExistente.fator_correlacao = fatcorrelacao;
                        }
                        if (capacidadecopo >= 551 && capacidadecopo != 770)
                        {
                            fatcorrelacao = "0,0156";

                            double correlacao;
                            correlacao = Double.Parse(fatcorrelacao);
                            double resultado = capacidadecopo * correlacao;
                            massamin = resultado.ToString("0.000");
                            descricaoExistente.especificada = massamin;
                            descricaoExistente.fator_correlacao = fatcorrelacao;
                        }
                        if (capacidade_copo == "770")
                        {
                            massamin = "12,00";
                            fatcorrelacao = "0,0156";
                            descricaoExistente.especificada = massamin;
                            descricaoExistente.fator_correlacao = fatcorrelacao;
                        }

                        _context.SaveChanges();
                        await _context.SaveChangesAsync();
                        TempData["Mensagem"] = "Dados Editados com sucesso!!";
                    }
                    else
                    {

                        double capacidadeCopo;
                        capacidadeCopo = Double.Parse(capacidade_copo);

                        if (capacidadeCopo == 1 && capacidadeCopo < 50)
                        {
                            fatcorrelacao = "0,00150";

                            double correlacao;
                            correlacao = Double.Parse(fatcorrelacao);
                            double resultado = capacidadeCopo * correlacao;
                            massamin = resultado.ToString("0.000");

                        }
                        if (capacidade_copo == "50")
                        {
                            massamin = "0,75";
                            fatcorrelacao = "0,0150";
                        }
                        if (capacidadeCopo >= 51 && capacidadeCopo <= 149 && capacidadeCopo != 80 && capacidadeCopo != 110)
                        {
                            fatcorrelacao = "0,00175";

                            double correlacao;
                            correlacao = Double.Parse(fatcorrelacao);
                            double resultado = capacidadeCopo * correlacao;
                            massamin = resultado.ToString("0.000");

                        }
                        if (capacidade_copo == "80")
                        {
                            massamin = "1,40";
                            fatcorrelacao = "0,0175";
                        }
                        if (capacidade_copo == "110")
                        {
                            massamin = "1,90";
                            fatcorrelacao = "0,0175";
                        }
                        if (capacidade_copo == "150")
                        {
                            massamin = "1,35";
                            fatcorrelacao = "0,0090";
                        }
                        if (capacidadeCopo >= 151 && capacidadeCopo <= 299 && capacidadeCopo != 180 && capacidadeCopo != 200 && capacidadeCopo != 250)
                        {
                            fatcorrelacao = "0,0090";

                            double correlacao;
                            correlacao = Double.Parse(fatcorrelacao);
                            double resultado = capacidadeCopo * correlacao;
                            massamin = resultado.ToString("0.000");

                        }
                        if (capacidade_copo == "180")
                        {
                            massamin = "1,62";
                            fatcorrelacao = "0,0090";
                        }
                        if (capacidade_copo == "200")
                        {
                            massamin = "1,80";
                            fatcorrelacao = "0,0090";
                        }
                        if (capacidade_copo == "250")
                        {
                            massamin = "2,25";
                            fatcorrelacao = "0,0090";
                        }
                        if (capacidade_copo == "300")
                        {
                            massamin = "2,70";
                            fatcorrelacao = "0,0090";
                        }
                        if (capacidadeCopo >= 301 && capacidadeCopo <= 329)
                        {
                            fatcorrelacao = "0,0110";

                            double correlacao;
                            correlacao = Double.Parse(fatcorrelacao);
                            double resultado = capacidadeCopo * correlacao;
                            massamin = resultado.ToString("0.000");

                        }
                        if (capacidade_copo == "330")
                        {
                            massamin = "3,63";
                            fatcorrelacao = "0,0110";
                        }
                        if (capacidadeCopo >= 331 && capacidadeCopo <= 549 && capacidadeCopo != 400 && capacidadeCopo != 440 &&
                            capacidadeCopo != 500)
                        {
                            fatcorrelacao = "0,0126";

                            double correlacao;
                            correlacao = Double.Parse(fatcorrelacao);
                            double resultado = capacidadeCopo * correlacao;
                            massamin = resultado.ToString("0.000");

                        }
                        if (capacidade_copo == "400")
                        {
                            massamin = "5,00";
                            fatcorrelacao = "0,0126";
                        }
                        if (capacidade_copo == "440")
                        {
                            massamin = "5,54";
                            fatcorrelacao = "0,0126";
                        }
                        if (capacidade_copo == "500")
                        {
                            massamin = "6,30";
                            fatcorrelacao = "0,0126";
                        }
                        if (capacidade_copo == "550")
                        {
                            massamin = "6,93";
                            fatcorrelacao = "0,0126";
                        }
                        if (capacidadeCopo >= 551 && capacidadeCopo != 770)
                        {
                            fatcorrelacao = "0,0156";

                            double correlacao;
                            correlacao = Double.Parse(fatcorrelacao);
                            double resultado = capacidadeCopo * correlacao;
                            massamin = resultado.ToString("0.000");
                        }
                        if (capacidade_copo == "770")
                        {
                            massamin = "12,00";
                            fatcorrelacao = "0,0156";
                        }

                        //guardando os valores da descricao.
                        var salvarDescricao = new ColetaModel.Descricao
                        {
                            os = os,
                            rev = Rev,
                            qtd_recebida = qtd_recebida,
                            qtd_ensaiada = qtd_ensaiada,
                            capacidade_copo = capacidade_copo,
                            quant_manga = quant_manga,
                            capacidade_manga = capacidade_manga,
                            executador = executador,
                            auxiliador = auxiliador,
                            resp_conf = res_conf,
                            fator_correlacao = fatcorrelacao,
                            especificada = massamin,

                        };

                        _context.Add(salvarDescricao);
                        await _context.SaveChangesAsync();
                        TempData["Mensagem"] = "Dados salvos com sucesso!!";
                        return RedirectToAction(nameof(Index), new { os, orcamento, Rev });

                    }

                    return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                }
                else
                {
                    TempData["Mensagem"] = "Não Foi possivel salvar os dados";
                    return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error", ex.Message);
                throw;
            }
            return View("Index");
        }




        [HttpPost]
        public async Task<IActionResult> SalvarAspectosVisuais(int os, int orcamento, int rev, int osData, [Bind("quatro_dois_um_Atende,quatro_dois_um_Resul," +
            "quatro_dois_dois_Atende,quatro_dois_dois_Resul,quatro_dois_tres_Atende,quatro_dois_tres_Resul,data_de_inicio,data_de_termino,executador,auxiliador,resp_conf")] ColetaModel.Aspectosvisuais salvar)
        {
            try
            {
                var Editardados = _context.copos_aspectosvisuais.Where(x => x.os == os && x.Rev == rev).FirstOrDefault();

                if (ModelState.IsValid)
                {
                    //verificando se os campos estao vazios.

                    // verificando se existe dados para editar, 
                    if (Editardados != null)
                    {

                        var quatro_dois_um_Atende = salvar.quatro_dois_um_Atende;
                        var quatro_dois_dois_Atende = salvar.quatro_dois_dois_Atende;
                        var quatro_dois_tres_Atende = salvar.quatro_dois_tres_Atende;

                        // verificando e alterando primeiro resultado
                        if (quatro_dois_um_Atende == "Sim")
                        {
                            Editardados.quatro_dois_um_Resul = "C";
                        }
                        else if (quatro_dois_um_Atende == "Não")
                        {
                            Editardados.quatro_dois_um_Resul = "NC";
                        }
                        else
                        {
                            Editardados.quatro_dois_um_Resul = "Na";
                        }

                        // verificando e alterando segundo resultado
                        if (quatro_dois_dois_Atende == "Sim")
                        {
                            Editardados.quatro_dois_dois_Resul = "C";
                        }
                        else if (quatro_dois_dois_Atende == "Não")
                        {
                            Editardados.quatro_dois_dois_Resul = "NC";
                        }
                        else
                        {
                            Editardados.quatro_dois_dois_Resul = "Na";
                        }

                        // verificando e alterando terceiro resultado
                        if (quatro_dois_tres_Atende == "Sim")
                        {
                            Editardados.quatro_dois_tres_Resul = "C";
                        }
                        else if (quatro_dois_tres_Atende == "Não")
                        {
                            Editardados.quatro_dois_tres_Resul = "NC";
                        }
                        else
                        {
                            Editardados.quatro_dois_tres_Resul = "Na";
                        }


                        Editardados.quatro_dois_um_Atende = salvar.quatro_dois_um_Atende;
                        Editardados.quatro_dois_dois_Atende = salvar.quatro_dois_dois_Atende;
                        Editardados.quatro_dois_tres_Atende = salvar.quatro_dois_tres_Atende;
                        Editardados.data_de_inicio = salvar.data_de_inicio;
                        Editardados.data_de_termino = salvar.data_de_termino;


                        await _context.SaveChangesAsync();
                        TempData["Mensagem"] = "Dados Editado com Sucesso";
                        return RedirectToAction(nameof(Index), new { os, orcamento, rev });
                    }
                    else
                    {

                        // salvando valores caso nao exista para editar
                        var quatro_dois_um_Atende = salvar.quatro_dois_um_Atende;
                        var quatro_dois_um_Resul = salvar.quatro_dois_um_Resul;
                        var quatro_dois_dois_Atende = salvar.quatro_dois_dois_Atende;
                        var quatro_dois_dois_Resul = salvar.quatro_dois_dois_Resul;
                        var quatro_dois_tres_Atende = salvar.quatro_dois_tres_Atende;
                        var quatro_dois_tres_Resul = salvar.quatro_dois_tres_Resul;
                        var data_de_inicio = salvar.data_de_inicio;
                        var data_de_termino = salvar.data_de_termino;
                        string executador = salvar.executador;
                        string auxiliador = salvar.auxiliador;
                        string resp_conf = salvar.resp_conf;



                        if (quatro_dois_um_Atende == null || quatro_dois_dois_Atende == null || quatro_dois_tres_Atende == null)
                        {
                            TempData["Mensagem"] = "Preencha todos os campos para salvar";
                            return RedirectToAction(nameof(Index), new { os, orcamento, rev });
                        }
                        else
                        {
                            // verifcando valor recebido do checkbox do primeiro valor 
                            if (quatro_dois_um_Atende == "Sim")
                            {
                                 quatro_dois_um_Resul = "C";

                            }
                            else if (quatro_dois_um_Atende == "Não")
                            {
                              quatro_dois_um_Resul = "NC";
                            }
                            else
                            {
                                quatro_dois_um_Resul = "Na";
                            }

                            //verificando do segundo valor o checkbox
                            if (quatro_dois_dois_Atende == "Sim")
                            {
                                quatro_dois_dois_Resul = "C";

                            }
                            else if (quatro_dois_dois_Atende == "Não")
                            {
                               quatro_dois_dois_Resul = "NC";
                            }
                            else
                            {
                                quatro_dois_dois_Resul = "Na";
                            }

                            // verificando do terceiro valor do checkbox
                            if (quatro_dois_tres_Atende == "Sim")
                            {
                               quatro_dois_tres_Resul = "C";

                            }
                            else if (quatro_dois_tres_Atende == "Não")
                            {
                                quatro_dois_tres_Resul = "NC";
                            }
                            else
                            {
                                quatro_dois_tres_Resul = "Na";
                            }

                            var salvarDados = new ColetaModel.Aspectosvisuais
                            {
                                os = os,
                                orcamento = orcamento,
                                Rev = rev,
                                quatro_dois_um_Atende = quatro_dois_um_Atende,
                                quatro_dois_um_Resul = quatro_dois_um_Resul,
                                quatro_dois_dois_Atende = quatro_dois_dois_Atende,
                                quatro_dois_dois_Resul = quatro_dois_dois_Resul,
                                quatro_dois_tres_Atende = quatro_dois_tres_Atende,
                                quatro_dois_tres_Resul = quatro_dois_tres_Resul,
                                data_de_inicio = data_de_inicio,
                                data_de_termino = data_de_termino,
                                executador = executador,
                                auxiliador = auxiliador,
                                resp_conf = resp_conf
                            };

                            _context.Add(salvarDados);
                            await _context.SaveChangesAsync();
                            TempData["Mensagem"] = "Dados salvos com sucesso!!";
                            return RedirectToAction(nameof(Index), new { os, orcamento, rev });
                        }
                    }
                }
                else
                {
                    TempData["Mensagem"] = "Não Foi possivel salvar os dadsos";
                    return RedirectToAction(nameof(Index), new { os, orcamento, rev });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error", ex.Message);
                throw;
            }
            return View("Index");
        }


        [HttpPost]
        public async Task<IActionResult> SalvarMarcacao(int os, int Rev, string orcamento, int osData, [Bind("a_Contem_informacao, a_Estão_relevo, a_Caracteres_visiveis, " +
            "a_forma_indelevel,a_Evidencia,b_Contem_informacao,b_Estao_relevo,b_Caracteres_visiveis,b_forma_indelevel,b_Evidencia, c_Contem_informacao,c_Estao_relevo, c_Caracteres_visiveis, c_forma_indelevel,c_Evidencia, a_resultados, b_resultados, c_resultados, data_de_inicio, data_de_termino,executador,auxiliador, resp_conf")] ColetaModel.Marcacao salvar, string info, string lote, string validade, string observacoes)
        {
            try
            {

                if (orcamento != null)
                {
                    //recebendo valor para ver se existe algo gravado sobre essa os e rev,
                    var EditarMarcacao = _context.copos_marcacao.Where(x => x.os == os && x.rev == Rev).FirstOrDefault();
                    var EditarMaterias = _context.copos_materiais.Where(x => x.os == os && x.rev == Rev).FirstOrDefault();

                    // verificando se existe valor 
                    if (EditarMarcacao != null)
                    {
                        if (salvar.a_Contem_informacao == "Sim" && salvar.a_Estão_relevo == "Sim" && salvar.a_Caracteres_visiveis == "Sim")
                        {
                            salvar.a_resultados = "C";
                        }
                        else if (salvar.a_Contem_informacao == "Não" || salvar.a_Estão_relevo == "Não" || salvar.a_Caracteres_visiveis == "Não")
                        {
                            salvar.a_resultados = "NC";
                        }
                        else
                        {
                            salvar.a_resultados = "Na";
                        }

                        // verificando segundo linha para passar resultado.
                        if (salvar.b_Contem_informacao == "Sim" && salvar.b_Estao_relevo == "Sim" && salvar.b_Caracteres_visiveis == "Sim")
                        {
                            salvar.b_resultados = "C";
                        }
                        else if (salvar.b_Contem_informacao == "Não" || salvar.b_Estao_relevo == "Não" || salvar.b_Caracteres_visiveis == "Não")
                        {
                            salvar.b_resultados = "NC";
                        }
                        else
                        {
                            salvar.b_resultados = "Na";
                        }

                        // verificando segundo linha para passar resultado.
                        if (salvar.c_Contem_informacao == "Sim" && salvar.c_Estao_relevo == "Sim" && salvar.c_Caracteres_visiveis == "Sim")
                        {
                            salvar.c_resultados = "C";
                        }
                        else if (salvar.c_Contem_informacao == "Não" || salvar.c_Estao_relevo == "Não" || salvar.c_Caracteres_visiveis == "Não")
                        {
                            salvar.c_resultados = "NC";
                        }
                        else
                        {
                            salvar.c_resultados = "Na";
                        }
                        EditarMarcacao.a_Contem_informacao = salvar.a_Contem_informacao;
                        EditarMarcacao.a_Estão_relevo = salvar.a_Estão_relevo;
                        EditarMarcacao.a_Caracteres_visiveis = salvar.a_Caracteres_visiveis;
                        EditarMarcacao.a_forma_indelevel = salvar.a_forma_indelevel;
                        EditarMarcacao.a_Evidencia = salvar.a_Evidencia;
                        EditarMarcacao.b_Contem_informacao = salvar.b_Contem_informacao;
                        EditarMarcacao.b_Estao_relevo = salvar.b_Estao_relevo;
                        EditarMarcacao.b_Caracteres_visiveis = salvar.b_Caracteres_visiveis;
                        EditarMarcacao.b_forma_indelevel = salvar.b_forma_indelevel;
                        EditarMarcacao.b_Evidencia = salvar.b_Evidencia;
                        EditarMarcacao.c_Contem_informacao = salvar.c_Contem_informacao;
                        EditarMarcacao.c_Estao_relevo = salvar.c_Estao_relevo;
                        EditarMarcacao.c_Caracteres_visiveis = salvar.c_Caracteres_visiveis;
                        EditarMarcacao.c_forma_indelevel = salvar.c_forma_indelevel;
                        EditarMarcacao.c_Evidencia = salvar.c_Evidencia;
                        EditarMarcacao.a_resultados = salvar.a_resultados;
                        EditarMarcacao.b_resultados = salvar.b_resultados;
                        EditarMarcacao.c_resultados = salvar.c_resultados;
                        EditarMarcacao.data_de_inicio = salvar.data_de_inicio;
                        EditarMarcacao.data_de_termino = salvar.data_de_termino;

                        await _context.SaveChangesAsync();
                        TempData["Mensagem"] = "Dados Editado Com Sucesso!";

                    }
                    else
                    {
                        // recebendo valores para gravar no banco de dados.
                        var a_Contem_informacao = salvar.a_Contem_informacao;
                        var a_Estão_relevo = salvar.a_Estão_relevo;
                        var a_Caracteres_visiveis = salvar.a_Caracteres_visiveis;
                        var a_forma_indelevel = salvar.a_forma_indelevel;
                        var a_Evidencia = salvar.a_Evidencia;
                        var b_Contem_informacao = salvar.b_Contem_informacao;
                        var b_Estao_relevo = salvar.b_Estao_relevo;
                        var b_Caracteres_visiveis = salvar.b_Caracteres_visiveis;
                        var b_forma_indelevel = salvar.b_forma_indelevel;
                        var b_Evidencia = salvar.b_Evidencia;
                        var c_Contem_informacao = salvar.c_Contem_informacao;
                        var c_Estao_relevo = salvar.c_Estao_relevo;
                        var c_Caracteres_visiveis = salvar.c_Caracteres_visiveis;
                        var c_forma_indelevel = salvar.c_forma_indelevel;
                        var c_Evidencia = salvar.c_Evidencia;
                        var a_resultados = salvar.a_resultados;
                        var b_resultados = salvar.b_resultados;
                        var c_resultados = salvar.c_resultados;
                        var data_de_inicio = salvar.data_de_inicio;
                        var data_de_termino = salvar.data_de_termino;
                        string executador = salvar.executador;
                        string auxiliador = salvar.auxiliador;
                        string resp_conf = salvar.resp_conf;



                        if (a_Contem_informacao == null || a_Estão_relevo == null ||
                            a_Caracteres_visiveis == null || a_forma_indelevel == null
                            || a_Evidencia == null || b_Contem_informacao == null || b_Estao_relevo == null || b_Caracteres_visiveis == null
                            || b_forma_indelevel == null || b_Evidencia == null || c_Contem_informacao == null || c_Estao_relevo == null
                            || c_Caracteres_visiveis == null || c_forma_indelevel == null || c_Evidencia == null)
                        {
                            TempData["Mensagem"] = "Preencha todos os campos para salvar";
                            return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                        }
                        else
                        {
                            // verificando primeira linha para passar resultado.
                            if (a_Contem_informacao == "Sim" && a_Estão_relevo == "Sim" && a_Caracteres_visiveis == "Sim")
                            {
                                a_resultados = "C";
                            }
                            else if (a_Contem_informacao == "Não" || a_Estão_relevo == "Não" || a_Caracteres_visiveis == "Não")
                            {
                                a_resultados = "NC";
                            }
                            else
                            {
                                a_resultados = "Na";
                            }

                            // verificando segundo linha para passar resultado.
                            if (b_Contem_informacao == "Sim" && b_Estao_relevo == "Sim" && b_Caracteres_visiveis == "Sim")
                            {
                                b_resultados = "C";
                            }
                            else if (b_Contem_informacao == "Não" || b_Estao_relevo == "Não" || b_Caracteres_visiveis == "Não")
                            {
                                b_resultados = "NC";
                            }
                            else
                            {
                                b_resultados = "Na";
                            }

                            // verificando segundo linha para passar resultado.
                            if (c_Contem_informacao == "Sim" && c_Estao_relevo == "Sim" && c_Caracteres_visiveis == "Sim")
                            {
                                c_resultados = "C";
                            }
                            else if (c_Contem_informacao == "Não" || c_Estao_relevo == "Não" || c_Caracteres_visiveis == "Não")
                            {
                                c_resultados = "NC";
                            }
                            else
                            {
                                c_resultados = "Na";
                            }

                            var SalvarMarcacao = new ColetaModel.Marcacao
                            {
                                os = os,
                                rev = Rev,
                                a_Contem_informacao = a_Contem_informacao,
                                a_Estão_relevo = a_Estão_relevo,
                                a_Caracteres_visiveis = a_Caracteres_visiveis,
                                a_forma_indelevel = a_forma_indelevel,
                                a_Evidencia = a_Evidencia,
                                b_Contem_informacao = b_Contem_informacao,
                                b_Estao_relevo = b_Estao_relevo,
                                b_Caracteres_visiveis = b_Caracteres_visiveis,
                                b_forma_indelevel = b_forma_indelevel,
                                b_Evidencia = b_Evidencia,
                                c_Contem_informacao = c_Contem_informacao,
                                c_Estao_relevo = c_Estao_relevo,
                                c_Caracteres_visiveis = c_Caracteres_visiveis,
                                c_forma_indelevel = c_forma_indelevel,
                                c_Evidencia = c_Evidencia,
                                a_resultados = a_resultados,
                                b_resultados = b_resultados,
                                c_resultados = c_resultados,
                                data_de_inicio = data_de_inicio,
                                data_de_termino = data_de_termino,
                                executador = executador,
                                auxiliador = auxiliador,
                                resp_conf = resp_conf
                            };

                            _context.Add(SalvarMarcacao);
                            await _context.SaveChangesAsync();
                        }

                    }

                    //verificar se existe materiais.
                    if (EditarMaterias != null)
                    {
                        EditarMaterias.info = info;
                        EditarMaterias.lote = lote;
                        EditarMaterias.validade = validade;
                        EditarMaterias.observacoes = observacoes;

                        await _context.SaveChangesAsync();
                        TempData["Mensagem"] = "Dados Editado Com Sucesso";
                        return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                    }
                    else
                    {
                        if (info == null || lote == null || validade == null ||
                            observacoes == null)
                        {
                            TempData["Mensagem"] = "Preencha todos os campos para salvar";
                            return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                        }
                        else
                        {
                            // se caso nao exista valor, salvar no banco
                            var SalvarMateriais = new ColetaModel.Materiais
                            {
                                os = os,
                                rev = Rev,
                                info = info,
                                lote = lote,
                                validade = validade,
                                observacoes = observacoes
                            };
                            _context.Add(SalvarMateriais);
                            await _context.SaveChangesAsync();
                            TempData["Mensagem"] = "Dados  Gravados Com Sucesso";
                            return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                        }
                    }

                }
                else
                {
                    TempData["Mensagem"] = "Não Foi possivel salvar os dadsos";
                    return RedirectToAction(nameof(Index), new { os, orcamento, Rev });

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error", ex.Message);
                throw;
            }
            return View("Index");

        }

        [HttpPost]
        public async Task<IActionResult> SalvarEmbalagem(int os, int rev, string orcamento, int osData, [Bind("As_mangas_estão_invioláveis, Estão_protegidos_saco_plástico, Capacidade_total, Capacidade_total_Evidencia," +
            " Quantidade_de_copos, Quantidade_copos_Evidencia, Rastreabilidade, Resultados, data_de_inicio, data_de_termino,executador,auxiliador,resp_conf")] ColetaModel.Embalagem salvar)
        {
            try
            {
                var editarDados = _context.copos_embalagem.Where(x => x.os == os && x.rev == rev).FirstOrDefault();

                if (orcamento != null)
                {
                    ///VERIFICANDO SE EXISTE DADOS PARA EDITAR
                    if (editarDados != null)
                    {
                        editarDados.As_mangas_estão_invioláveis = salvar.As_mangas_estão_invioláveis;
                        editarDados.Estão_protegidos_saco_plástico = salvar.Estão_protegidos_saco_plástico;
                        editarDados.Capacidade_total = salvar.Capacidade_total;
                        editarDados.Capacidade_total_Evidencia = salvar.Capacidade_total_Evidencia;
                        editarDados.Quantidade_de_copos = salvar.Quantidade_de_copos;
                        editarDados.Quantidade_copos_Evidencia = salvar.Quantidade_copos_Evidencia;
                        editarDados.Rastreabilidade = salvar.Rastreabilidade;
                        editarDados.Resultados = salvar.Resultados;
                        editarDados.data_de_inicio = salvar.data_de_inicio;
                        editarDados.data_de_termino = salvar.data_de_termino;


                        await _context.SaveChangesAsync();
                        TempData["Mensagem"] = "Dados editado com sucesso";
                        return RedirectToAction(nameof(Index), new { os, orcamento, rev });
                    }
                    else
                    {

                        //CASO NAO EXISTA NADA, ELE GRAVA OS DADOS
                        var As_mangas_estão_invioláveis = salvar.As_mangas_estão_invioláveis;
                        var Estão_protegidos_saco_plástico = salvar.Estão_protegidos_saco_plástico;
                        var Capacidade_total = salvar.Capacidade_total;
                        var Capacidade_total_Evidencia = salvar.Capacidade_total_Evidencia;
                        var Quantidade_de_copos = salvar.Quantidade_de_copos;
                        var Quantidade_copos_Evidencia = salvar.Quantidade_copos_Evidencia;
                        var Rastreabilidade = salvar.Rastreabilidade;
                        var Resultados = salvar.Resultados;
                        var data_de_inicio = salvar.data_de_inicio;
                        var data_de_termino = salvar.data_de_termino;
                        string executador = salvar.executador;
                        string auxiliador = salvar.auxiliador;
                        string resp_conf = salvar.resp_conf;


                        if (As_mangas_estão_invioláveis == null || Estão_protegidos_saco_plástico == null ||
                            Capacidade_total == null || Capacidade_total_Evidencia == null || Quantidade_de_copos == null
                            || Quantidade_copos_Evidencia == null || Rastreabilidade == null || Resultados == null)
                        {
                            TempData["Mensagem"] = "Preencha todos os campos para salvar";
                            return RedirectToAction(nameof(Index), new { os, orcamento, rev });
                        }
                        else
                        {
                            var SalvarEmbalagem = new Embalagem
                            {

                                os = os,
                                rev = rev,
                                orcamento = orcamento,
                                As_mangas_estão_invioláveis = As_mangas_estão_invioláveis,
                                Estão_protegidos_saco_plástico = Estão_protegidos_saco_plástico,
                                Capacidade_total = Capacidade_total,
                                Capacidade_total_Evidencia = Capacidade_total_Evidencia,
                                Quantidade_de_copos = Quantidade_de_copos,
                                Quantidade_copos_Evidencia = Quantidade_copos_Evidencia,
                                Rastreabilidade = Rastreabilidade,
                                Resultados = Resultados,
                                data_de_inicio = data_de_inicio,
                                data_de_termino = data_de_termino,
                                executador = executador,
                                auxiliador = auxiliador,
                                resp_conf = resp_conf,


                            };
                            _context.Add(SalvarEmbalagem);
                            await _context.SaveChangesAsync();
                            TempData["Mensagem"] = "Dados salvos com sucesso!!";
                            return RedirectToAction(nameof(Index), new { os, orcamento, rev });
                        }
                    }
                }

                else
                {
                    TempData["Mensagem"] = "Não foi possível salvar os dados";
                    return RedirectToAction(nameof(Index), new { os, orcamento, rev });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error", ex.Message);
                throw;
            }

        }



        [HttpPost]
        public async Task<IActionResult> SalvarMassa(int os, int osDescricao, int osData, string rsi, string rci, string massamin, string fatcorrelacao, int rev, string orcamento,
          List<string> massa, List<string> peso,
          ColetaModel.Descricao descricaocopos, [Bind("incerteza, data_de_inicio, data_de_termino,executador,auxiliador,resp_conf")] ColetaModel.Tablemassa tablemassa)
        {
            try
            {

                osDescricao = os;

                if (orcamento != null)
                {

                    var EditarMassa = _context.copos_massa.Where(x => x.os == os && x.rev == rev).ToList();

                    if (EditarMassa == null || EditarMassa.Count == 0)
                    {
                        var pegarValoresDescricao = _context.copos_descricao
                       .Where(os => os.os == osDescricao)
                        .Select(os => new
                        {
                            os.capacidade_copo,
                            os.quant_manga,
                            os.capacidade_manga,
                            os.especificada,
                            os.fator_correlacao,

                        })
                        .FirstOrDefault();

                        var pegarValoresDatas = _context.copo_datas
                      .Where(os => os.os == osData)
                       .Select(os => new
                       {
                           os.data_de_início,
                           os.data_de_termino,

                       })
                       .FirstOrDefault();

                        // verificando cada lista preenchida se tem --- , se receber --- o valor é passado como 0,
                        // porem salva como --- no banco e numeros para salvar no banco de dados.
                        List<double> numeros = peso.Select(s => s == "---" ? 0 : double.Parse(s)).ToList();
                        double quantidadeValoresNaoZero = numeros.Count(numero => numero != 0);


                        // Calcula a média aritmética

                        double soma = numeros.Sum();
                        double Resultfinal = soma / (quantidadeValoresNaoZero * 10);


                        //Fazendo a conta qdo RSI (se o obtida é >= ao Especificada,
                        //então o resultado é "CONFORME", se não é "NÃO CONFORME)".

                        double especificada;
                        especificada = Double.Parse(pegarValoresDescricao.especificada);

                        if (Resultfinal >= especificada)
                        {
                            rsi = "C";
                        }
                        else
                        {
                            rsi = "NC";
                        }

                        //PEGAR VALOR DO BACNO DE DADOS REFERENTE A INCERTEZA. PARA SALVAR....
                        var pegarValorIncerteza = _incertezacontext.copos_incertezas
                                                  .Where(x => x.descricao == "Determinação da Massa do Copo")
                                                  .OrderByDescending(x => x.data_cadastro)
                                                  .FirstOrDefault();
                        //pegando os valores enviados pelo html.
                        var data_de_inicio = tablemassa.data_de_inicio;
                        var data_de_termino = tablemassa.data_de_termino;
                        string executador = tablemassa.executador;
                        string auxiliador = tablemassa.auxiliador;
                        string resp_conf = tablemassa.resp_conf;
                        //Fazendo a conta qdo RCI (Se o Obtida(Resultfinal) - a incerteza(verificarincerteza)
                        //é >= ao especificada, entao é "CONFORME", se não é "NÃO CONFORME".

                        double verificarincerteza;
                        verificarincerteza = Double.Parse(pegarValorIncerteza.valor);

                        double verificarRci;
                        verificarRci = Resultfinal - verificarincerteza;

                        if (verificarRci >= especificada)
                        {
                            rci = "C";
                        }
                        else
                        {
                            rci = "NC";
                        }


                        //Salvando na Tablemassa.

                        var compressaoDados = new Tablemassa
                        {
                            os = os,
                            orcamento = orcamento,
                            capcopo = pegarValoresDescricao.capacidade_copo,
                            quantmanga = pegarValoresDescricao.quant_manga,
                            capmanga = pegarValoresDescricao.capacidade_manga,
                            fatcorrelacao = pegarValoresDescricao.fator_correlacao,
                            obtida = Resultfinal.ToString("0.000"),
                            especificada = pegarValoresDescricao.especificada,
                            incerteza =  pegarValorIncerteza.valor,
                            rsi = rsi,
                            rci = rci,
                            data_de_inicio = data_de_inicio,
                            data_de_termino = data_de_termino,
                            executador = executador,
                            auxiliador = auxiliador,
                            resp_conf = resp_conf

                        };
                        _context.Add(compressaoDados);

                        await _context.SaveChangesAsync();

                        //Percorrendo a lista de resultados de peso e massa e salvando na tabela copos_massa.
                        for (int i = 0; i < massa.Count; i++)
                        {
                            double pesoValor;
                            if (double.TryParse(peso[i], out pesoValor))
                            {
                                var item = new ColetaModel.Massa
                                {
                                    os = os,
                                    rev = rev,
                                    massa = massa[i],
                                    peso = peso[i]
                                };

                                _context.Add(item);
                                await _context.SaveChangesAsync();
                            }
                            else if (peso[i] == "---")
                            {
                                var itemEspecial = new ColetaModel.Massa
                                {
                                    os = os,
                                    rev = rev,
                                    massa = massa[i],
                                    peso = peso[i]
                                };

                                _context.Add(itemEspecial);
                                await _context.SaveChangesAsync();
                            }

                        };
                        TempData["Mensagem"] = "Dados salvos com sucesso!!";
                        return RedirectToAction(nameof(Index), new { os, orcamento, rev });

                    }
                    else
                    {
                        //percorrendo o peso que vem do parametro
                        for (int j = 0; j < massa.Count; j++)
                        {
                            // para pegar todas os pesos e salvar eles editados
                            var pesoatual = EditarMassa.FirstOrDefault(x => x.massa == massa[j]);
                            if (pesoatual != null)
                            {
                                double pesoValor;
                                if (double.TryParse(peso[j], out pesoValor))
                                {
                                    pesoatual.peso = peso[j];
                                }
                                else if (peso[j] == "---")
                                {
                                    pesoatual.peso = peso[j];
                                }
                                await _context.SaveChangesAsync();
                            }
                        };


                        var pegarValoresDescricao = _context.copos_descricao
                        .Where(os => os.os == osDescricao)
                        .Select(os => new
                       {
                           os.capacidade_copo,
                           os.quant_manga,
                           os.capacidade_manga,
                           os.especificada,
                           os.fator_correlacao,

                       })
                       .FirstOrDefault();

                        double capacidadeCopo;
                        capacidadeCopo = Double.Parse(pegarValoresDescricao.capacidade_copo);
                        var EditarTable = _context.copos_tablemassa.Where(a => a.os == os && a.Rev == rev).FirstOrDefault();

                        

                        // Editando o obtida, que é o valor média dos números dividida por 10
                        List<double> numeros = peso.Select(s => s == "---" ? 0 : double.Parse(s)).ToList();
                        double soma = numeros.Sum();
                        double mediaAritmetica = soma / numeros.Count;
                        double Resultfinal = mediaAritmetica / 10;

                        // pegando o valor de especificada que ja esta salvo na tabela do table massa e transformando ele em double, assim eu calculo os novos rci e rsi
                        var pegarValorEspecificada = _context.copos_tablemassa.Where(os => os.os == osDescricao).Select(os => new { os.especificada, }).FirstOrDefault();
                        double verificarespecificada = Double.Parse(pegarValorEspecificada.especificada);

                        if (Resultfinal >= verificarespecificada)
                        {
                            rsi = "C";
                        }
                        else
                        {
                            rsi = "NC";
                        }
                        //pegando os valores enviados pelo html.


                        var incerteza = tablemassa.incerteza;
                        var data_de_inicio = tablemassa.data_de_inicio;
                        var data_de_termino = tablemassa.data_de_termino;

                        //Fazendo a conta qdo RCI (Se o Obtida(Resultfinal) - a incerteza(verificarincerteza)
                        //é >= ao especificada, entao é "CONFORME", se não é "NÃO CONFORME".

                        double verificarincerteza;
                        verificarincerteza = Double.Parse(incerteza);

                        double verificarRsi;
                        verificarRsi = Resultfinal - verificarincerteza;

                        if (verificarRsi >= verificarespecificada)
                        {
                            rci = "C";
                        }
                        else
                        {
                            rci = "NC";
                        }


                        EditarTable.obtida = Resultfinal.ToString("0.000");
                        EditarTable.incerteza = incerteza;
                        EditarTable.rsi = rsi;
                        EditarTable.rci = rci;
                        EditarTable.data_de_inicio = data_de_inicio;
                        EditarTable.data_de_termino = data_de_termino;
                        EditarTable.especificada = pegarValoresDescricao.especificada;
                        EditarTable.fatcorrelacao = pegarValoresDescricao.fator_correlacao;
                      

                        await _context.SaveChangesAsync();
                        TempData["Mensagem"] = "Editado com sucesso!!";
                        return RedirectToAction(nameof(Index), new { os, orcamento, rev });
                    }
                }
                else
                {
                    TempData["Mensagem"] = "Não foi possível salvar os dados";
                    return RedirectToAction(nameof(Index), new { os, orcamento, rev });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error", ex.Message);
                throw;
            }
        }


        [HttpPost]
        public async Task<IActionResult> InstrumentosMassa(int os, string orcamento, int Rev, string codigo, List<ColetaModel.instrumentosMassa> instrumentos)
        {
            try
            {
                if (orcamento != null)
                {
                    if (codigo != null || string.IsNullOrEmpty(codigo))
                    {
                        string codigoPesquisa = codigo.ToUpper();
                        var pesquisarInstrumentos = _bancocontext.cad_instr
                            .Where(x => x.Codigo == codigoPesquisa && x.laboratorio == "ESP")
                            .FirstOrDefault();

                        if (pesquisarInstrumentos != null)
                        {

                            var salvarInstrumentosCond = new ColetaModel.instrumentosMassa
                            {
                                os = os,
                                orcamento = orcamento,
                                rev = Rev,
                                codigo = codigoPesquisa,
                                descricao = pesquisarInstrumentos.descricaoins,
                                certificado = pesquisarInstrumentos.NC,
                                validade = pesquisarInstrumentos.data2,
                                ativo = 1
                            };
                            _context.Add(salvarInstrumentosCond);

                            await _context.SaveChangesAsync();

                            TempData["Mensagem"] = "DADOS SALVO COM SUCESSO";
                            return RedirectToAction(nameof(Index), new { os, orcamento, Rev });

                        }
                        else
                        {
                            TempData["Mensagem"] = "Não Foi encontrado nenhum codigo. ";
                            return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                        }
                    }
                    else
                    {
                        TempData["Mensagem"] = "Não foi inserido nenhum codigo ";
                        return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                    }

                }
                else
                {
                    TempData["Mensagem"] = "NÃO FOI POSSIVEL SALVAR OS DADOS ";
                    return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error", ex.Message);
                throw;
            }
        }

        public async Task<IActionResult> ExcInstrumentosMassa(int os, int Rev, string orcamento)
        {
            try
            {
                //VERIFICANDO O ULTIMO DADO NA TABELA E PASSANDO O ATIVO PARA 0.
                var apagarInstrumento = _context.instrumentos_massa
                                        .Where(x => x.os == os && x.rev == Rev && x.ativo == 1)
                                        .OrderBy(x => x.Id)
                                        .LastOrDefault();

                if (apagarInstrumento != null)
                {
                    apagarInstrumento.ativo = 0;
                    await _context.SaveChangesAsync();

                    TempData["Mensagem"] = " Instrumento excluído com sucesso";
                    return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                }
                else
                {
                    TempData["Mensagem"] = "Não foi possivel excluir codigo";
                    return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error", ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> InstrumentosCompressao(int os, int Rev, string orcamento, string codigo, List<ColetaModel.instrumentosCompressao> instrumentos)
        {
            try
            {
                if (orcamento != null)
                {
                    if (codigo != null || string.IsNullOrEmpty(codigo))
                    {
                        string codigoPesquisa = codigo.ToUpper();
                        var pesquisarInstrumentos = _bancocontext.cad_instr
                            .Where(x => x.Codigo == codigoPesquisa && x.laboratorio == "ESP")
                            .FirstOrDefault();

                        if (pesquisarInstrumentos != null)
                        {

                            var salvarInstrumentosCompressao = new ColetaModel.instrumentosCompressao
                            {
                                os = os,
                                orcamento = orcamento,
                                rev = Rev,
                                codigo = codigoPesquisa,
                                descricao = pesquisarInstrumentos.descricaoins,
                                certificado = pesquisarInstrumentos.NC,
                                validade = pesquisarInstrumentos.data2,
                                ativo = 1
                            };
                            _context.Add(salvarInstrumentosCompressao);

                            await _context.SaveChangesAsync();

                            TempData["Mensagem"] = "DADOS SALVO COM SUCESSO";
                            return RedirectToAction(nameof(Index), new { os, orcamento, Rev });

                        }
                        else
                        {
                            TempData["Mensagem"] = "Não Foi encontrado nenhum codigo. ";
                            return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                        }
                    }
                    else
                    {
                        TempData["Mensagem"] = "Não foi inserido nenhum codigo ";
                        return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                    }

                }
                else
                {
                    TempData["Mensagem"] = "NÃO FOI POSSIVEL SALVAR OS DADOS ";
                    return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error", ex.Message);
                throw;
            }
        }

        public async Task<IActionResult> ExcInstrumentosCompressao(int os, int Rev, string orcamento)
        {
            try
            {
                //VERIFICANDO O ULTIMO DADO NA TABELA E PASSANDO O ATIVO PARA 0.
                var apagarInstrumento = _context.instrumentos_compressao
                                        .Where(x => x.os == os && x.rev == Rev && x.ativo == 1)
                                        .OrderBy(x => x.Id)
                                        .LastOrDefault();

                if (apagarInstrumento != null)
                {
                    apagarInstrumento.ativo = 0;
                    await _context.SaveChangesAsync();

                    TempData["Mensagem"] = " Instrumento excluído com sucesso";
                    return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                }
                else
                {
                    TempData["Mensagem"] = "Não foi possivel excluir codigo";
                    return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error", ex.Message);
                throw;
            }
        }




        [HttpPost]
        public async Task<IActionResult> EnsaioDeCompressaoAndAmostras(int osDescricao, int os, string orcamento, int osData, int rev, string rsi, string rci, string capacidade_especificada, string capacidadeCopo, string valor_min_especificado, List<int> amostra, List<string> resistencia, ColetaModel.Descricao descricaoCopos, [Bind("Incerteza,data_de_inicio,data_de_termino,executador,auxiliador,resp_conf")] ColetaModel.Compressao dadosCompressao)
        {
            try
            {
                osDescricao = os;


                //verificando se esta tudo ok ao tentar salvar os dados.
                if (orcamento != null)
                {

                    //verificando se existe dados na tabela do banco. para editar
                    var editarAmostra = _context.copos_amostra.Where(x => x.os == os && x.rev == rev).ToList();
                    var editatDescricao = _context.copos_compressao.Where(x => x.os == os && x.rev == rev).FirstOrDefault();


                    if (editarAmostra == null || editarAmostra.Count == 0)
                    {
                        //conexao com a amostra.
                        List<Amostra> amostras = new List<Amostra>();

                        //criando variavel para pegar menor valor de resistencia.

                        double menor_valor_resistencia = double.MaxValue;

                        //percorrendo a amostra.
                        for (int i = 0; i < amostra.Count; i++)
                        {
                            var dados = new ColetaModel.Amostra
                            {
                                os = os,
                                rev = rev,
                                amostra = amostra[i],
                                resistencia = resistencia[i]
                            };

                            //convertentdo e pegando o menor valor de resistencia que vem da matriz.
                            if (double.TryParse(resistencia[i], out double resistenciaAtual) && resistenciaAtual < menor_valor_resistencia)
                            {
                                menor_valor_resistencia = resistenciaAtual;
                            }

                            //salvando os dados de amostra e resistencia
                            _context.Add(dados);
                            await _context.SaveChangesAsync();
                        }
                        //pegando valores de capacidade de copo,especificado, para salvar na tabela.
                        var pegar_valor_capacidade = _context.copos_descricao
                            .Where(os => os.os == osDescricao)
                            .Select(os => new { os.capacidade_copo })
                            .FirstOrDefault();

                        capacidadeCopo = pegar_valor_capacidade.capacidade_copo;
                        int capacidade_convertida = int.Parse(capacidadeCopo);

                        if (capacidade_convertida >= 0 && capacidade_convertida <= 149)
                        {
                            capacidade_especificada = "0-149 ml";
                            valor_min_especificado = "1,6";
                        }
                        else if (capacidade_convertida >= 150 && capacidade_convertida <= 300)
                        {
                            capacidade_especificada = "150-300 ml";
                            valor_min_especificado = "0,8";
                        }
                        else if (capacidade_convertida >= 301 && capacidade_convertida <= 330)
                        {
                            capacidade_especificada = "301-330 ml";
                            valor_min_especificado = "1,1";
                        }
                        else if (capacidade_convertida >= 331 && capacidade_convertida <= 440)
                        {
                            capacidade_especificada = "331-440 ml";
                            valor_min_especificado = "1,2";
                        }
                        else
                        {
                            capacidade_especificada = ">441";
                            valor_min_especificado = "1,4";
                        }



                        //buscando o ultimo valor registrado na incerteza referente a esse item.
                        var pegarValorIncerteza = _incertezacontext.copos_incertezas
                                                  .Where(x => x.descricao == "Determinação da Resistência à Compressão Lateral do Copo")
                                                  .OrderByDescending(x => x.data_cadastro)
                                                  .FirstOrDefault();

                        // pegando dados do html
                        var data_de_inicio = dadosCompressao.data_de_inicio;
                        var data_de_termino = dadosCompressao.data_de_termino;
                        string executador = dadosCompressao.executador;
                        string auxiliador = dadosCompressao.auxiliador;
                        string resp_conf = dadosCompressao.resp_conf;

                        var compressaoDados = new ColetaModel.Compressao
                        {
                            os = os,
                            rev = rev,
                            Capacidade = capacidade_convertida.ToString(),
                            Capacidade_especificada = capacidade_especificada,
                            Valor_min_especificado = valor_min_especificado,
                            Valor_min_obtido = menor_valor_resistencia,
                            Incerteza = pegarValorIncerteza.valor,
                            data_de_inicio = data_de_inicio,
                            data_de_termino = data_de_termino,
                            executador = executador,
                            auxiliador = auxiliador,
                            resp_conf = resp_conf
                        };

                        _context.Add(compressaoDados);
                        await _context.SaveChangesAsync();
                        // verificando a amostra para receber o valor de resistencia, para fazer o calculo de rsi e rci.

                        var amostrasExistente = _context.copos_amostra
                                                .Where(a => a.os == os && a.rev == rev && amostra.Contains(a.amostra))
                                                .ToList();

                        // chamando minha tabela pegando a os, para salvar rsi e rci na coluna.
                        var compressaoExistente = _context.copos_compressao
                                               .Where(a => a.os == os && a.rev == rev)
                                               .ToList();

                        //fazendo um for para salvar nc ou c para cada resistencia.na coluna rsi e rci
                        for (int i = 0; i < amostrasExistente.Count; i++)
                        {

                            var Rsi = double.Parse(resistencia[amostrasExistente[i].amostra - 1]);
                            double Valor_min_especificado = double.Parse(valor_min_especificado);

                            double incerteza = double.Parse(pegarValorIncerteza.valor);

                            //verificando o rsi, para passar o valor.
                            if (Rsi <= Valor_min_especificado)
                            {
                                amostrasExistente[i].rsi = "NC";
                            }
                            else
                            {
                                amostrasExistente[i].rsi = "C";
                            }

                            //verificando o rci, para passar o valor.
                            if ((Rsi - incerteza) <= Valor_min_especificado)
                            {
                                amostrasExistente[i].rci = "NC";
                            }
                            else
                            {
                                amostrasExistente[i].rci = "C";
                            }
                        }
                        // percorrendo a tabela compressao para passar na tabela, se esta "c" ou "nc".
                        int contador = 0;
                        int contadorRci = 0;
                        for (int j = 0; j < compressaoExistente.Count; j++)
                        {

                            if (amostrasExistente[j].rsi == "NC")
                            {
                                contador++;
                            }

                            if (contador >= 1)
                            {
                                compressaoExistente[j].rsi = "NC";
                            }
                            else
                            {
                                compressaoExistente[j].rsi = "C";
                            }

                            //verificando quantidade de nc do rc.
                            if (amostrasExistente[j].rci == "NC")
                            {
                                contadorRci++;
                            }

                            if (contadorRci >= 1)
                            {
                                compressaoExistente[j].rci = "NC";
                            }
                            else
                            {
                                compressaoExistente[j].rci = "C";
                            }
                        }

                        await _context.SaveChangesAsync();

                        TempData["Mensagem"] = "SALVO COM SUCESSO!";
                        return RedirectToAction(nameof(Index), new { os, orcamento, rev });
                    }
                    else
                    {
                        double menor_valor_resistencia = double.MaxValue;

                        //percorrendo a amostra que vem de parametro
                        for (int i = 0; i < amostra.Count; i++)
                        {
                            // para pegar todas as amostras e salvar nas resistencias.
                            var resistenciaAtual = editarAmostra.FirstOrDefault(x => x.amostra == amostra[i]);

                            //atualizando valor de resistencia que usuario editar.
                            resistenciaAtual.resistencia = resistencia[i];

                            //editando o menor valor de resistencia que vem da matriz.
                            if (double.TryParse(resistencia[i], out double resistenciaAtualEditada) && resistenciaAtualEditada < menor_valor_resistencia)
                            {
                                menor_valor_resistencia = resistenciaAtualEditada;
                            }

                            await _context.SaveChangesAsync();
                        }

                        // pegando dados do html
                        var data_de_inicio = dadosCompressao.data_de_inicio;
                        var data_de_termino = dadosCompressao.data_de_termino;

                        //editando dados da tabela compressao.
                        editatDescricao.Valor_min_obtido = menor_valor_resistencia;
                        var incerteza = editatDescricao.Incerteza;
                        editatDescricao.Incerteza = incerteza;

                        // verificando a amostra para atualizar o valor de resistencia, para fazer o calculo de rsi e rci.
                        var editarAmostraExistente = _context.copos_amostra
                                                .Where(a => a.os == os && a.rev == rev && amostra.Contains(a.amostra))
                                                .ToList();

                        //CHAMANDO TABELA PARA PEGAR VALOR DE VALOR MINIMO ESPECIFICADO.
                        var editarCompressaoExistente = _context.copos_compressao
                                            .Where(a => a.os == os && a.rev == rev)
                                            .ToList().FirstOrDefault();

                        var editarRci = _context.copos_compressao
                                          .Where(a => a.os == os && a.rev == rev)
                                          .ToList();


                        // PERCORRENDO TABELA DE AMOSTRA PARA EDITAR VALOR RSI E RCI
                        for (int i = 0; i < editarAmostraExistente.Count; i++)
                        {

                            var Rsi = double.Parse(resistencia[editarAmostraExistente[i].amostra - 1]);
                            double Valor_min_especificado = double.Parse(editarCompressaoExistente.Valor_min_especificado);
                            double editarIncerteza = double.Parse(incerteza);
                            editarCompressaoExistente.data_de_inicio = data_de_inicio;
                            editarCompressaoExistente.data_de_termino = data_de_termino;


                            //verificando o rsi, para editar o valor.
                            if (Rsi <= Valor_min_especificado)
                            {
                                editarAmostraExistente[i].rsi = "NC";
                            }
                            else
                            {
                                editarAmostraExistente[i].rsi = "C";
                            }

                            //editando valor de rci
                            if ((Rsi - editarIncerteza) <= Valor_min_especificado)
                            {
                                editarAmostraExistente[i].rci = "NC";
                            }
                            else
                            {
                                editarAmostraExistente[i].rci = "C";
                            }
                        }

                        // percorrendo a tabela compressao para editar na tabela, se esta "c" ou "nc".
                        int contador = 0;
                        int contadorRci = 0;
                        for (int j = 0; j < editarRci.Count; j++)
                        {
                            if (editarAmostraExistente[j].rsi == "NC")
                            {
                                contador++;
                            }

                            if (contador > 1)
                            {
                                editarRci[j].rsi = "NC";
                            }
                            else
                            {
                                editarRci[j].rsi = "C";
                            }

                            //verificando quantidade de nc do rc.
                            if (editarAmostraExistente[j].rci == "NC")
                            {
                                contadorRci++;
                            }

                            if (contadorRci > 1)
                            {
                                editarRci[j].rci = "NC";
                            }
                            else
                            {
                                editarRci[j].rci = "C";
                            }
                        }
                        await _context.SaveChangesAsync();

                        TempData["Mensagem"] = "Dados Editado com Sucesso";
                        return RedirectToAction(nameof(Index), new { os, orcamento, rev });
                    }
                }
                else
                {
                    TempData["Mensagem"] = "ERRO AO SALVAR DADOS";
                    return RedirectToAction(nameof(Index), new { os, orcamento, rev });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error", ex.Message);
                throw;
            }
        }

        public async Task<IActionResult> condicionamentoAmostra(int os, string orcamento, int Rev, [Bind("ini_dat_acond,ter_data_acond,tem_min_encon,temp_max_encont,ini_hora_acond,term_hora_acond,umid_min_encon,umid_max_encon,im_utilizado,responsavel")] ColetaModel.CondicionamentoMinimo salvarCondMin, [Bind("ini_dat_acond_max,ter_data_acond_max,tem_min_encon_max,temp_max_encont_max,ini_hora_acond_max,term_hora_acond_max,umid_min_encon_max,umid_max_encon_max,im_utilizado_max,responsavel_max")] ColetaModel.CondicMaximo salvarCondMax)
        {
            try
            {
                if (orcamento != null)
                {
                    var editarDadosCondicionamentoMin = _context.copos_codicionamento_minimo
                       .Where(x => x.os == os && x.rev == Rev)
                       .FirstOrDefault();
                    var editarDadosCondicionamentoMax = _context.copos_codicionamento_maximo
                       .Where(x => x.os == os && x.rev == Rev)
                       .FirstOrDefault();

                    if (editarDadosCondicionamentoMin == null && editarDadosCondicionamentoMax == null)
                    {
                        //GUARDANDO OS VALORES NAS VARIAVEIS DA COND MINIMA 4HORAS
                        var ini_dat_acond = salvarCondMin.ini_dat_acond;
                        var ter_data_acond = salvarCondMin.ter_data_acond;
                        var tem_min_encon = salvarCondMin.tem_min_encon;
                        var temp_max_encont = salvarCondMin.temp_max_encont;
                        var ini_hora_acond = salvarCondMin.ini_hora_acond;
                        var term_hora_acond = salvarCondMin.term_hora_acond;
                        var umid_min_encon = salvarCondMin.umid_min_encon;
                        var umid_max_encon = salvarCondMin.umid_max_encon;
                        var im_utilizado = salvarCondMin.im_utilizado;
                        var responsavel = salvarCondMin.responsavel;

                        //SALVAR DADOS NO BANCO DE DADOS. DA COND MINIMA 4HORAS
                        var SalvarDadosMin = new CondicionamentoMinimo
                        {
                            os = os,
                            rev = Rev,
                            orcamento = orcamento,
                            ini_dat_acond = ini_dat_acond,
                            ter_data_acond = ter_data_acond,
                            tem_min_encon = tem_min_encon,
                            temp_max_encont = temp_max_encont,
                            ini_hora_acond = ini_hora_acond,
                            term_hora_acond = term_hora_acond,
                            umid_min_encon = umid_min_encon,
                            umid_max_encon = umid_max_encon,
                            im_utilizado = im_utilizado,
                            responsavel = responsavel
                        };



                        //GUARDADO OS VALORES NA TABELA CONDICIONAMENTO MAXIMO, QUE É DIA TODO.
                        var ini_dat_acond_condMax = salvarCondMax.ini_dat_acond_max;
                        var ter_data_acond_condMax = salvarCondMax.ter_data_acond_max;
                        var tem_min_encon_condMax = salvarCondMax.tem_min_encon_max;
                        var temp_max_encont_condMax = salvarCondMax.temp_max_encont_max;
                        var ini_hora_acond_condMax = salvarCondMax.ini_hora_acond_max;
                        var term_hora_acond_condMax = salvarCondMax.term_hora_acond_max;
                        var umid_min_encon_condMax = salvarCondMax.umid_min_encon_max;
                        var umid_max_encon_condMax = salvarCondMax.umid_max_encon_max;
                        var im_utilizado_condMax = salvarCondMax.im_utilizado_max;
                        var responsavel_condMax = salvarCondMax.responsavel_max;
                        //verificando se tem dados da primeira telava vazia


                        var SalvarDadosMax = new CondicMaximo
                        {
                            os = os,
                            rev = Rev,
                            orcamento = orcamento,
                            ini_dat_acond_max = ini_dat_acond_condMax,
                            ter_data_acond_max = ter_data_acond_condMax,
                            tem_min_encon_max = tem_min_encon_condMax,
                            temp_max_encont_max = temp_max_encont_condMax,
                            ini_hora_acond_max = ini_hora_acond_condMax,
                            term_hora_acond_max = term_hora_acond_condMax,
                            umid_min_encon_max = umid_min_encon_condMax,
                            umid_max_encon_max = umid_max_encon_condMax,
                            im_utilizado_max = im_utilizado_condMax,
                            responsavel_max = responsavel_condMax
                        };
                        //verificando dados se estao vazios
                        if (ini_dat_acond == null || ter_data_acond == null || tem_min_encon == null || temp_max_encont == null || ini_hora_acond == null || term_hora_acond == null || im_utilizado == null || responsavel == null || ini_dat_acond_condMax == null || ter_data_acond_condMax == null || tem_min_encon_condMax == null || temp_max_encont_condMax == null || ini_hora_acond_condMax == null || term_hora_acond_condMax == null || im_utilizado_condMax == null || responsavel_condMax == null)
                        {
                            TempData["Mensagem"] = "Campos vazios";
                            return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                        }

                        //se ocorrer tudo bem, salvar dados
                        _context.Add(SalvarDadosMin);
                        await _context.SaveChangesAsync();

                        _context.Add(SalvarDadosMax);
                        await _context.SaveChangesAsync();



                        TempData["Mensagem"] = "Dados Salvo Com Sucesso!";
                        return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                    }
                    else
                    {
                        //guardado valor da tabela cond minima (4horas) se caso usuario editar 
                        editarDadosCondicionamentoMin.ini_dat_acond = salvarCondMin.ini_dat_acond;
                        editarDadosCondicionamentoMin.ter_data_acond = salvarCondMin.ter_data_acond;
                        editarDadosCondicionamentoMin.tem_min_encon = salvarCondMin.tem_min_encon;
                        editarDadosCondicionamentoMin.temp_max_encont = salvarCondMin.temp_max_encont;
                        editarDadosCondicionamentoMin.ini_hora_acond = salvarCondMin.ini_hora_acond;
                        editarDadosCondicionamentoMin.term_hora_acond = salvarCondMin.term_hora_acond;
                        editarDadosCondicionamentoMin.umid_min_encon = salvarCondMin.umid_min_encon;
                        editarDadosCondicionamentoMin.im_utilizado = salvarCondMin.im_utilizado;
                        editarDadosCondicionamentoMin.responsavel = salvarCondMin.responsavel;

                        //GUARDANDO VALOR DA TABE COND MAXIMA SE USUARIO EDITAR ALGUM VALOR..
                        editarDadosCondicionamentoMax.ter_data_acond_max = salvarCondMax.ini_dat_acond_max;
                        editarDadosCondicionamentoMax.ter_data_acond_max = salvarCondMax.ter_data_acond_max;
                        editarDadosCondicionamentoMax.tem_min_encon_max = salvarCondMax.tem_min_encon_max;
                        editarDadosCondicionamentoMax.temp_max_encont_max = salvarCondMax.temp_max_encont_max;
                        editarDadosCondicionamentoMax.ini_hora_acond_max = salvarCondMax.ini_hora_acond_max;
                        editarDadosCondicionamentoMax.term_hora_acond_max = salvarCondMax.term_hora_acond_max;
                        editarDadosCondicionamentoMax.umid_min_encon_max = salvarCondMax.umid_min_encon_max;
                        editarDadosCondicionamentoMax.umid_max_encon_max = salvarCondMax.umid_max_encon_max;
                        editarDadosCondicionamentoMax.im_utilizado_max = salvarCondMax.im_utilizado_max;
                        editarDadosCondicionamentoMax.responsavel_max = salvarCondMax.responsavel_max;

                        //SALVANDO DADOS EDITADO..
                        await _context.SaveChangesAsync();
                        TempData["Mensagem"] = "Dados Editado Com Sucesso!";

                        return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                    }
                }
                else
                {
                    TempData["Mensagem"] = "Não foi possivel salvar os dados";
                    return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error", ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> InstrumentosCondicionamento(int os, string orcamento, int Rev, string codigo, List<ColetaModel.instrumentosCond> instrumentos)
        {
            try
            {
                if (orcamento != null)
                {
                    if (codigo != null || string.IsNullOrEmpty(codigo))
                    {
                        string codigoPesquisa = codigo.ToUpper();
                        var pesquisarInstrumentos = _bancocontext.cad_instr
                            .Where(x => x.Codigo == codigoPesquisa && x.laboratorio == "ESP")
                            .FirstOrDefault();

                        if (pesquisarInstrumentos != null)
                        {

                            var salvarInstrumentosCond = new ColetaModel.instrumentosCond
                            {
                                os = os,
                                orcamento = orcamento,
                                rev = Rev,
                                codigo = codigoPesquisa,
                                descricao = pesquisarInstrumentos.descricaoins,
                                certificado = pesquisarInstrumentos.NC,
                                validade = pesquisarInstrumentos.data2,
                                ativo = 1
                            };
                            _context.Add(salvarInstrumentosCond);

                            await _context.SaveChangesAsync();

                            TempData["Mensagem"] = "DADOS SALVO COM SUCESSO";
                            return RedirectToAction(nameof(Index), new { os, orcamento, Rev });

                        }
                        else
                        {
                            TempData["Mensagem"] = "Não Foi encontrado nenhum codigo. ";
                            return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                        }
                    }
                    else
                    {
                        TempData["Mensagem"] = "Não foi inserido nenhum codigo ";
                        return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                    }

                }
                else
                {
                    TempData["Mensagem"] = "NÃO FOI POSSIVEL SALVAR OS DADOS ";
                    return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error", ex.Message);
                throw;
            }
        }

        public async Task<IActionResult> ExcInstrumentosCondicionamento(int os, int Rev, string orcamento)
        {
            try
            {
                //VERIFICANDO O ULTIMO DADO NA TABELA E PASSANDO O ATIVO PARA 0.
                var apagarInstrumento = _context.instrumentos_cond
                                        .Where(x => x.os == os && x.rev == Rev && x.ativo == 1)
                                        .OrderBy(x => x.Id)
                                        .LastOrDefault();

                if (apagarInstrumento != null)
                {
                    apagarInstrumento.ativo = 0;
                    await _context.SaveChangesAsync();

                    TempData["Mensagem"] = " Instrumento excluído com sucesso";
                    return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                }
                else
                {
                    TempData["Mensagem"] = "Não foi possivel excluir codigo";
                    return RedirectToAction(nameof(Index), new { os, orcamento, Rev });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error", ex.Message);
                throw;
            }
        }



        [HttpPost]
        public async Task<IActionResult> FinalizarColeta(int os, string orcamento, int rev, [Bind("data_de_fin, responsavel,revisao")] ColetaModel.Finalizado salvar)
        {
            try
            {
                if (orcamento != null)
                {
                    var data_de_fin = salvar.data_de_fin;
                    var responsavel = salvar.responsavel;
                    var revisao = salvar.revisao;


                    //guardando os valores
                    var salvarDados = new Finalizado
                    {
                        os = os,
                        orcamento = orcamento,
                        rev = rev,
                        data_de_fin = data_de_fin,
                        responsavel = responsavel,
                        revisao = revisao
                    };
                    _context.Add(salvarDados);
                    await _context.SaveChangesAsync();
                    TempData["Mensagem"] = "Coleta Finalizada!";
                    return RedirectToAction(nameof(Index), new { os, orcamento, rev });

                }
                else
                {
                    TempData["Mensagem"] = "Não foi possivel Finalizar a Coleta";
                    return RedirectToAction(nameof(Index), new { os, orcamento, rev });
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error", ex.Message);
                throw;
            }
        }



    }
}