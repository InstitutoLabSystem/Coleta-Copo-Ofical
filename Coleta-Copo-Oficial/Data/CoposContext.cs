using Copo_Coleta.Models;
using Microsoft.EntityFrameworkCore;


namespace Copo_Coleta.Data
{
    public class CoposContext : DbContext
    {

        public CoposContext(DbContextOptions<CoposContext> options) : base(options)
        {
        }
        // exemplo aqui fica minha tabela, no caso essa nao existe, so estou passando de exemplo.

        public DbSet<ColetaModel.Datas> copo_datas { get; set; }

        public DbSet<ColetaModel.Aspectosvisuais> copos_aspectosvisuais { get; set; }

        public DbSet<ColetaModel.Massa> copos_massa { get; set; }
        public DbSet<ColetaModel.Amostra> copos_amostra { get; set; }
        public DbSet<ColetaModel.Descricao> copos_descricao { get; set; }
        public DbSet<ColetaModel.Compressao> copos_compressao { get; set; }

        public DbSet<ColetaModel.Tablemassa> copos_tablemassa { get; set; }

        public DbSet<ColetaModel.Marcacao> copos_marcacao { get; set; }

        public DbSet<ColetaModel.Materiais> copos_materiais { get; set; }

        public DbSet<ColetaModel.Embalagem> copos_embalagem { get; set; }
        public DbSet<ColetaModel.CondicionamentoMinimo> copos_codicionamento_minimo { get; set; }
        public DbSet<ColetaModel.CondicMaximo> copos_codicionamento_maximo { get; set; }

        public DbSet<ColetaModel.Finalizado> copos_finalizado { get; set; }
        public DbSet<ColetaModel.instrumentosCond> instrumentos_cond { get; set; }
        public DbSet<ColetaModel.instrumentosMassa> instrumentos_massa { get; set; }
        public DbSet<ColetaModel.instrumentosCompressao> instrumentos_compressao { get; set; }

    }
}

