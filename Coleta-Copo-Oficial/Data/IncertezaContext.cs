using Coleta_Copo_Oficial.Models;
using Copo_Coleta.Data;
using Microsoft.EntityFrameworkCore;

namespace Coleta_Copo_Oficial.Data
{
    public class IncertezaContext : DbContext
    {
        public IncertezaContext(DbContextOptions<IncertezaContext> options) : base(options)
        {
        }
        public DbSet<ColetaIncertezaModel.buscarIncerteza> copos_incertezas { get; set; }
    }
}
