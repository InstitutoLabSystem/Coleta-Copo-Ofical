using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;

namespace Copo_Coleta.Models
{
    public class ColetaModel
    {
        public class Datas
        {
            [Key]
            public int Id { get; set; }

            [Required(ErrorMessage = "Campo Obrigatorio.")]
            public DateTime data_de_início { get; set; }
            [Required(ErrorMessage = "Campo Obrigatorio.")]
            public DateTime data_de_termino { get; set; }
             
            public int editar {get; set; }
            public int os { get; set; }
            public string orcamento { get; set; }
            public int Rev { get; set; }
        }

        public class Descricao
        {
            [Key]

            public int os { get; set; }
            public int rev { get; set; }
          
            public int qtd_recebida { get; set; }
       
            public int qtd_ensaiada { get; set; }
         
            public string capacidade_copo { get; set; }
    
            public string quant_manga { get; set; }
      
            public string capacidade_manga { get; set; }

        }

        public class Aspectosvisuais
        {
            [Key]
            public int Id { get; set; }
            public int os { get; set; }
            public int Rev { get; set; }
            public int orcamento { get; set; }

            public string? quatro_dois_um_Atende { get; set; }
            public string? quatro_dois_um_Resul { get; set; }
            public string? quatro_dois_dois_Atende { get; set; }
            public string? quatro_dois_dois_Resul { get; set; }
            public string? quatro_dois_tres_Atende { get; set; }
            public string? quatro_dois_tres_Resul { get; set; }

            [Required(ErrorMessage = "Campo Obrigatorio.")]
            public DateTime data_de_inicio { get; set; }
            [Required(ErrorMessage = "Campo Obrigatorio.")]
            public DateTime data_de_termino { get; set; }


        }

        public class Massa
        {
            [Key]
            public int Id { get; set; }
            public int os { get; set; }
            public int rev { get; set; }
            public string? massa { get; set; }
            public string? peso { get; set; }
        

        }

        public class Amostra
        {
            [Key]
            public int Id { get; set; }
            public int os { get; set; }
            public int rev { get; set; }
            public int amostra { get; set; }
            public string resistencia { get; set; }
            public string rsi { get; set; }
            public string rci { get; set; }
        }


        public class Compressao
        {
            [Key]
            public int os { get; set; }
            public int rev { get; set; }
            public string? Capacidade { get; set; }
            public string? Capacidade_especificada { get; set; }
            public string? Valor_min_obtido { get; set; }
            public string? Valor_min_especificado { get; set; }
            public string? Incerteza { get; set; }
            public string? rsi { get; set; }
            public string? rci { get; set; }
            [Required(ErrorMessage = "Campo Obrigatorio.")]
            public DateTime data_de_inicio { get; set; }
            [Required(ErrorMessage = "Campo Obrigatorio.")]
            public DateTime data_de_termino { get; set; }
        }

        public class Tablemassa
        {
            [Key]

            public int os { get; set; }
            public int Rev { get; set; }
            public string orcamento { get; set; }
            public string capcopo { get; set; }
            public string quantmanga { get; set; }

            public string capmanga { get; set; }
            public string fatcorrelacao { get; set; }
            public string obtida { get; set; }
            public string especificada { get; set; }
            public string incerteza { get; set; }
            public string rsi { get; set; }
            public string rci { get; set; }

            [Required(ErrorMessage = "Campo Obrigatorio.")]
            public DateTime data_de_inicio { get; set; }
            [Required(ErrorMessage = "Campo Obrigatorio.")]
            public DateTime data_de_termino { get; set; }

        }

        public class Marcacao

        {
            [Key]
            public int os { get; set; }
            public int rev { get; set; }
            public string? a_Contem_informacao { get; set; }
            public string? a_Estão_relevo { get; set; }
            public string? a_Caracteres_visiveis { get; set; }
            public string? a_forma_indelevel { get; set; }
            public string? a_Evidencia { get; set; }
            public string? b_Contem_informacao { get; set; }
            public string? b_Estao_relevo { get; set; }
            public string? b_Caracteres_visiveis { get; set; }
            public string? b_forma_indelevel { get; set; }
            public string? b_Evidencia { get; set; }
            public string? c_Contem_informacao { get; set; }
            public string? c_Estao_relevo { get; set; }
            public string? c_Caracteres_visiveis { get; set; }

            public string? c_forma_indelevel { get; set; }
            public string? c_Evidencia { get; set; }
            public string? a_resultados { get; set; }
            public string? b_resultados { get; set; }
            public string? c_resultados { get; set; }
            [Required(ErrorMessage = "Campo Obrigatorio.")]
            public DateTime data_de_inicio { get; set; }
            [Required(ErrorMessage = "Campo Obrigatorio.")]
            public DateTime data_de_termino { get; set; }

        }

        public class Materiais
        {
            [Key]
            public int Id { get; set; }
            public int os { get; set; }
            public int rev { get; set; }
            public string info { get; set; }
           
            public string lote { get; set; }
           
            public string validade { get; set; }
            
            public string observacoes { get; set; }

        }

        public class Embalagem
        {
            [Key]
            public int os { get; set; }
            public int rev { get; set; }
      
            public string orcamento { get; set; }
          
            public string As_mangas_estão_invioláveis { get; set; }
      

            public string Estão_protegidos_saco_plástico { get; set; }
           
            public string Capacidade_total { get; set; }
           
            public string Capacidade_total_Evidencia { get; set; }
  

            public string Quantidade_de_copos { get; set; }
       
            public string Quantidade_copos_Evidencia { get; set; }
    
            public string Rastreabilidade { get; set; }
     
            public string Resultados { get; set; }

            [Required(ErrorMessage = "Campo Obrigatorio.")]
            public DateTime data_de_inicio { get; set; }
            [Required(ErrorMessage = "Campo Obrigatorio.")]
            public DateTime data_de_termino { get; set; }
        }

        public class Instrumentos
        {
            [Key]
            public int Id { get; set; }
            public int os { get; set; }
            public int rev { get; set; }
        
            public string orcamento { get;  set; }
      
            public string codigo { get; set; }
     
            public string descricao { get; set; }
     
            public string certificado { get; set; }
            public DateTime? validade { get; set; }
  
            public int ativo { get; set; }
        }

        public class CondicionamentoMinimo
        {
            [Key]
            public int Id { get; set; }

            public int os { get; set; }
            public int rev { get; set; }
            public string orcamento { get; set; }
            public DateTime ini_dat_acond { get; set; }
            public DateTime ter_data_acond { get; set; }
            public string tem_min_encon { get; set; }
            public string temp_max_encont { get; set; }
            public TimeSpan ini_hora_acond { get; set; }
            public TimeSpan term_hora_acond { get; set; }
            public string? umid_min_encon { get; set; }
            public string? umid_max_encon { get; set; }
            public string im_utilizado { get; set; }
            public string responsavel { get; set; }

        }
        public class CondicMaximo
        {
            [Key]
            public int Id { get; set; }

            public int os { get; set; }
            public int rev { get; set; }
            public string orcamento { get; set; }
            public DateTime ini_dat_acond_max { get; set; }
            public DateTime ter_data_acond_max { get; set; }
            public string tem_min_encon_max { get; set; }
            public string temp_max_encont_max { get; set; }
            public TimeSpan ini_hora_acond_max { get; set; }
            public TimeSpan term_hora_acond_max { get; set; }
            public string? umid_min_encon_max { get; set; }
            public string? umid_max_encon_max { get; set; }
            public string im_utilizado_max { get; set; }
            public string responsavel_max { get; set; }

        }
    }
}
