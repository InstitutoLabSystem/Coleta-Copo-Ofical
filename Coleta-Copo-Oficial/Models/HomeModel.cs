using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

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

        public class Usuario
        {
            [Key]
            public string Nome_Usuario { get; set; }
            public string Senha_Usuario { get; set; }
            public string nomecompleto { get; set; }
            public string cargo { get; set; }
            public string setor { get; set; }
            public string laboratorio { get; set; }
            public int ativo { get; set; }


        }

    }
}

