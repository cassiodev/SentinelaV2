using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Sentinela.Models
{
    public class UsuarioModelView
    {
        public int PessoaId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email obrigatório")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Nome obrigatório")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Nome muito longo ou muito curto")]
        public string Nome { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Senha é obrigatório")]
        [DataType(DataType.Password, ErrorMessage = "Digite uma senha válida")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deverá possuir no minimo 6 e no máximo 100 caracteres.")]
        public string Senha { get; set; }

        [Display(Name="Confirma Senha")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Senha é obrigatório")]
        [DataType(DataType.Password, ErrorMessage = "Digite uma senha válida")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deverá possuir no minimo 6 e no máximo 100 caracteres.")]
        [Compare("Senha", ErrorMessage="Campo senha e confirma senha devem ser iguais")]
        public string ConfirmaSenha { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Login é obrigatório")]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "O login deverá possuir no minimo 6 e no máximo 40 caracteres.")]
        public string Login { get; set; }

        public int UsuarioId { get; set; }

        public bool Ativo { get; set; }
    }



    [MetadataType(typeof(UsuarioMetaData))]
    public partial class Usuario
    {

    }
    public class UsuarioMetaData
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Senha é obrigatório")]
        [DataType(DataType.Password, ErrorMessage = "Digite uma senha válida")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deverá possuir no minimo 6 e no máximo 100 caracteres.")]
        public string Senha { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Login é obrigatório")]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "O login deverá possuir no minimo 6 e no máximo 40 caracteres.")]
        public string Login { get; set; }


        
    }
    [MetadataType(typeof(PessoaMetaData))]

    public partial class Pessoa
    {

    }
    public class PessoaMetaData
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email obrigatório")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Nome obrigatório")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Nome muito longo ou muito curto")]
        public string Nome { get; set; }
    }

    [MetadataType(typeof(CategoriaMetaData))]
    public partial class Categoria
    {

    }
    public class CategoriaMetaData
    {
        [Required(AllowEmptyStrings = false,ErrorMessage = "Nome obrigatório")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Nome muito longo ou muito curto")]
        public string Nome { get; set; }
    }

    [MetadataType(typeof(ClienteMetaData))]
    public partial class Cliente
    {

    }
    public class ClienteMetaData
    {
        [CPF(ErrorMessage="CPF inválido")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Campo cidade é obrigatorio")]
        public int CidadeId { get; set; }

        
        [StringLength(200, ErrorMessage="Máximo de caracteres permitido: 200.")]
        public string Logradouro { get; set; }

        [Range(0, int.MaxValue)]
        public Nullable<int> Numero { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Número inválido")]
        [StringLength(14, ErrorMessage = "Máximo de caracteres permitido: 14.")]
        public string Celular { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Número inválido")]
        [StringLength(14, ErrorMessage = "Máximo de caracteres permitido: 14.")]
        public string Telefone { get; set; }

    }


    [MetadataType(typeof(AdicionalMetaData))]
    public partial class Adicional
    {

    }
    public class AdicionalMetaData
    {
        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo nome é obrigatorio")]
        [StringLength(200, ErrorMessage = "Máximo de caracteres permitido: 200.")]
        public string Nome { get; set; }

        [StringLength(8000, ErrorMessage = "Máximo de caracteres permitido: 8000.")]
        public string Descricao { get; set; }
    }


    [MetadataType(typeof(CardapioMetaData))]
    public partial class Cardapio
    {

    }
    public class CardapioMetaData
    {
        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo nome é obrigatorio")]
        [StringLength(200, ErrorMessage = "Máximo de caracteres permitido: 200.")]
        public string Nome { get; set; }

        [StringLength(5000, ErrorMessage = "Máximo de caracteres permitido: 5000.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo nome é descrição")]
        public string Descricao { get; set; }
    }


    [MetadataType(typeof(CidadeMetaData))]
    public partial class Cidade
    {

    }
    public class CidadeMetaData
    {
        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo nome é obrigatorio")]
        [StringLength(200, ErrorMessage = "Máximo de caracteres permitido: 200.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo estado é obrigatorio")]
        public int EstadoId { get; set; }

    }


    [MetadataType(typeof(AgendaMetaData))]
    public partial class Agenda
    {

    }
    public class AgendaMetaData
    {
        [Required(ErrorMessage = "Campo tipo é obrigatorio")]
        public int TipoEventoId { get; set; }

        [Required(ErrorMessage = "Campo local é obrigatorio")]
        public int LocalId { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo data é obrigatorio")]
        public System.DateTime Data { get; set; }

        [StringLength(8000, ErrorMessage = "Máximo de caracteres permitido: 8000.")]
        public string Observacao { get; set; }
    }


    [MetadataType(typeof(ItemMetaData))]
    public partial class Item
    {

    }
    public class ItemMetaData
    {
        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo nome é obrigatorio")]
        [StringLength(200, ErrorMessage = "Máximo de caracteres permitido: 200.")]
        public string Nome { get; set; }
    }


    [MetadataType(typeof(LocalMetaData))]
    public partial class Local
    {

    }
    public class LocalMetaData
    {

        public int LocalId { get; set; }

        [Required(ErrorMessage = "Campo cidade é obrigatorio")]
        public int CidadeId { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo nome é obrigatorio")]
        [StringLength(200, ErrorMessage = "Máximo de caracteres permitido: 200.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo tamanho é obrigatorio")]
        [Range(0, 999999)]
        public decimal Tamanho { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo logradouro é obrigatorio")]
        [StringLength(200, ErrorMessage = "Máximo de caracteres permitido: 200.")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "Campo número é obrigatorio")]
        [Range(0, int.MaxValue)]
        public int Numero { get; set; }
        public bool Ativo { get; set; }

        [Required(ErrorMessage = "Campo capacidade é obrigatorio")]
        [Range(0, int.MaxValue)]
        public int Capacidade { get; set; }

    }


    [MetadataType(typeof(EventoMetaData))]
    public partial class Evento
    {

    }
    public class EventoMetaData
    {

        [Required(ErrorMessage = "Campo cliente é obrigatorio")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "Campo local é obrigatorio")]
        public int LocalId { get; set; }

        [Required(ErrorMessage = "Campo cardápio é obrigatorio")]
        public int CardapioId { get; set; }

        [Required(ErrorMessage = "Campo tipo é obrigatorio")]
        public int TipoEventoId { get; set; }


        [Required(ErrorMessage = "Campo Data é obrigatorio")]
        [DataType(DataType.Date, ErrorMessage = "O valor deve ser uma data")]
        [DisplayFormat(ApplyFormatInEditMode = true,DataFormatString ="{0:dd.MM.yyyy}")]
        public System.DateTime DataEvento { get; set; }

        [Required(ErrorMessage = "Campo convidados é obrigatorio")]
        [Range(0, int.MaxValue)]
        public int Convidados { get; set; }

        [Required(ErrorMessage = "Campo período é obrigatorio")]
        [DataType(DataType.Time, ErrorMessage = "Tipo de dados incorreto.")]
        public System.TimeSpan Periodo { get; set; }

        [Range(0, int.MaxValue)]
        public Nullable<int> Criancas { get; set; }

    }


    [MetadataType(typeof(RefeicaoMetaData))]
    public partial class Refeicao
    {

    }
    public class RefeicaoMetaData
    {
        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo nome é obrigatorio")]
        [StringLength(200, ErrorMessage = "Máximo de caracteres permitido: 200.")]
        public string Nome { get; set; }
    }
    [MetadataType(typeof(TipoEventoMetaData))]
    public partial class TipoEvento
    {

    }
    public class TipoEventoMetaData
    {
        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo nome é obrigatorio")]
        [StringLength(200, ErrorMessage = "Máximo de caracteres permitido: 200.")]
        public string Nome { get; set; }
    }


    
}