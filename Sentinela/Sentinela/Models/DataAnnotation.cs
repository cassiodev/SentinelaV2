using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Sentinela.Models
{
    [MetadataType(typeof(UsuarioMetaData))]
    public partial class Usuario
    {

    }
    public class UsuarioMetaData
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Senha é obrigatório")]
        [DataType(DataType.Password, ErrorMessage = "Digite uma senha válida")]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "A senha deverá possuir 6 ou mais caracteres.")]
        public string Senha { get; set; }
        
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

        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo cidade é obrigatorio")]
        [StringLength(200, ErrorMessage = "Máximo de caracteres permitido: 200.")]
        public int CidadeId { get; set; }

        
        [StringLength(200, ErrorMessage="Máximo de caracteres permitido: 200.")]
        public string Logradouro { get; set; }

        [Range(0, int.MaxValue)]
        public Nullable<int> Numero { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Número inválido")]
        public string Celular { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Número inválido")]
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

        [StringLength(200, ErrorMessage = "Máximo de caracteres permitido: 8000.")]
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

        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo estado é obrigatorio")]
        public int EstadoId { get; set; }

    }


    [MetadataType(typeof(EventoMetaData))]
    public partial class Evento
    {

    }
    public class EventoMetaData
    {
        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo tipo é obrigatorio")]
        public int TipoEventoId { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo local é obrigatorio")]
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

        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo cidade é obrigatorio")]
        public int CidadeId { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo nome é obrigatorio")]
        [StringLength(200, ErrorMessage = "Máximo de caracteres permitido: 200.")]
        public string Nome { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo tamanho é obrigatorio")]
        [DataType(DataType.Currency, ErrorMessage = "Tipo de dados incorreto.")]
        public decimal Tamanho { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo logradouro é obrigatorio")]
        [StringLength(200, ErrorMessage = "Máximo de caracteres permitido: 200.")]
        public string Logradouro { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo número é obrigatorio")]
        [Range(0, int.MaxValue)]
        public int Numero { get; set; }
        public bool Ativo { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo capacidade é obrigatorio")]
        [Range(0, int.MaxValue)]
        public int Capacidade { get; set; }

    }


    [MetadataType(typeof(OrcamentoMetaData))]
    public partial class Orcamento
    {

    }
    public class OrcamentoMetaData
    {
        public int OrcamentoId { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo cliente é obrigatorio")]
        public int ClienteId { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo local é obrigatorio")]
        public int LocalId { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo cardápio é obrigatorio")]
        public int CardapioId { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo tipo é obrigatorio")]
        public int TipoEventoId { get; set; }


        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo Data é obrigatorio")]
        [DataType(DataType.Date, ErrorMessage="O valor deve ser uma data")]
        public System.DateTime DataEvento { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo convidados é obrigatorio")]
        [Range(0, int.MaxValue)]
        public int Convidados { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Campo período é obrigatorio")]
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




    
}