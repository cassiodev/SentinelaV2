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
    
    public partial class Cardapio
    {
        public Cardapio()
        {
            this.CardapioRefeicaoItem = new HashSet<CardapioRefeicaoItem>();
            this.Evento = new HashSet<Evento>();
            this.Imagem = new HashSet<Imagem>();
        }
    
        public int CardapioId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
    
        public virtual ICollection<CardapioRefeicaoItem> CardapioRefeicaoItem { get; set; }
        public virtual ICollection<Evento> Evento { get; set; }
        public virtual ICollection<Imagem> Imagem { get; set; }
    }
}
