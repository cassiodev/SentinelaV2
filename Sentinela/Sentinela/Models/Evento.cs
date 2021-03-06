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
    
    public partial class Evento
    {
        public Evento()
        {
            this.Agenda = new HashSet<Agenda>();
            this.Adicional = new HashSet<Adicional>();
        }
    
        public int EventoId { get; set; }
        public int ClienteId { get; set; }
        public int LocalId { get; set; }
        public int CardapioId { get; set; }
        public int TipoEventoId { get; set; }
        public System.DateTime DataCadastro { get; set; }
        public System.DateTime DataEvento { get; set; }
        public System.DateTime DataAlteracao { get; set; }
        public int Situacao { get; set; }
        public int Convidados { get; set; }
        public System.TimeSpan Periodo { get; set; }
        public Nullable<int> Criancas { get; set; }
    
        public virtual ICollection<Agenda> Agenda { get; set; }
        public virtual Cardapio Cardapio { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual TipoEvento TipoEvento { get; set; }
        public virtual Local Local { get; set; }
        public virtual ICollection<Adicional> Adicional { get; set; }
    }
}
