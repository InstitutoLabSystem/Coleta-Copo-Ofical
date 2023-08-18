using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Copo_Coleta.Models
{
    public class HomeModel
    {
        public class OrdemServico

        {
            [Key]
            public int Rev { get; set; }
            public string orcamento { get; set; }
            public int item { get; set; }
            public int codigo { get; set; }
            public string mes { get; set; }
            public string ano { get; set; }

        }
        public class TrazerInstrumentos
        {
            [Key]
            public string Codigo { get; set; }
            public string descricaoins { get; set; }
            public string NC { get; set; }
            public DateTime? data2 { get; set; }
            public string laboratorio { get; set; }
        }

    }
}
