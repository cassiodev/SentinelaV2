//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sentinela.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Imagem
    {
        public int ImagemId { get; set; }
        public Nullable<int> LocalId { get; set; }
        public Nullable<int> CardapioId { get; set; }
        public string Url { get; set; }
    
        public virtual Cardapio Cardapio { get; set; }
        public virtual Local Local { get; set; }
    }
}
