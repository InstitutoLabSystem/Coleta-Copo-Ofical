using System.ComponentModel.DataAnnotations;

namespace Coleta_Copo_Oficial.Models
{
    public class ColetaIncertezaModel
    {
        public class buscarIncerteza
        {
            [Key]
            public int Id_controle { get; set; }
            public string cime { get; set; }
            public DateTime data_cadastro { get; set; }
            public string valor { get; set; }
            public string descricao { get; set; }
            public string norma { get; set; }

        }
    }
}
