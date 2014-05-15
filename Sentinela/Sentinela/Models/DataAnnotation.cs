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

        [Required(ErrorMessage = "Nome obrigatório", AllowEmptyStrings = false)]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Nome muito longo ou muito curto")]
        public string Nome { get; set; }
    }


    [MetadataType(typeof(CategoriaMetaData))]
    public partial class Categoria
    {

    }
    public class CategoriaMetaData
    {
        [Required(ErrorMessage = "Nome obrigatório", AllowEmptyStrings = false)]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Nome muito longo ou muito curto")]
        public string Nome { get; set; }
    }


    [MetadataType(typeof(LocalMetaData))]
    public partial class Local
    {

    }
    public class LocalMetaData
    {
        [Required(ErrorMessage = "Campo obrigatório", AllowEmptyStrings = false)]
        
        public decimal Tamanho { get; set; }
    }
}