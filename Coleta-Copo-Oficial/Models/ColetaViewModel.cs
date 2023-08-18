using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using static Copo_Coleta.Models.ColetaModel;
using static Copo_Coleta.Models.HomeModel;

namespace Copo_Coleta.Models
{
    public class ColetaViewModel 
    {

        public Datas oDatas {get; set;} 
        public Descricao oDescricao { get; set;}
        public Aspectosvisuais oAspectosvisuais { get; set;}
        public List<Massa> oMassa { get; set;}    
        public Tablemassa oTablemassa { get; set;}
        public Compressao oCompressao { get; set;}
        public List<Amostra> oAmostra { get; set;}
        public Embalagem oEmbalagem { get; set;} 
        public  List<Instrumentos> oInstrumentos { get; set;}
        public List<Marcacao> oMarcacao { get; set;} 
        public Materiais oMateriais { get; set;}
    }
}
