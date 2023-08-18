using Microsoft.EntityFrameworkCore;
using Copo_Coleta.Models;
using System.Linq;


namespace Copo_Coleta.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
        {
        }
        // exemplo aqui fica minha tabela, no caso essa nao existe, so estou passando de exemplo.

        public DbSet<HomeModel.OrdemServico> ordemservico_copylab { get; set; }
        public DbSet<HomeModel.TrazerInstrumentos> cad_instr { get; set; }



    }
}


