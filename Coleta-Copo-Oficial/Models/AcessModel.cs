using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace Coleta_Copo_Oficial.Models
{
    public class AcessModel
    {
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
