using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Linq.Expressions;

namespace Sentinela.Core
{
    public enum Operacao { Equals, Contains, Upper, Lower }
    public enum Tipo {String, DateTime, Numeric }
    public class FiltroGenerico<T>
    {
        private const char separador = '|';
        private const string filtroKey = "Filtro";

        private List<Campo<T>> Campos = new List<Campo<T>>();

        public FiltroGenerico<T> AddCampo(string nome, Tipo tipo, Func<object, object, Expression<Func<T, bool>>> expressao)
        {
            return AddCampo(nome, tipo, "","", expressao);
        }
        public FiltroGenerico<T> AddCampo(string nome, Tipo tipo,string valor1, Func<object, object, Expression<Func<T, bool>>> expressao)
        {
            return AddCampo(nome, tipo, valor1, "", expressao);
        }
        public FiltroGenerico<T> AddCampo(string nome,Tipo tipo, string valor1, string valor2, Func<object, object,Expression<Func<T, bool>>> expressao)
        {
            var campo = new Campo<T>();
            campo.Nome = nome;
            campo.Expressao = expressao;
            campo.Tipo = tipo;
            campo.Valor1 = valor1;
            campo.Valor2 = valor2;

            Campos.Add(campo);

            return this;
        }
        private bool AtribuirValores(HttpRequestBase request)
        {
            var filtros = request.QueryString.AllKeys.Where(t => t.EndsWith(filtroKey));

            bool existeFiltros = false;

            foreach (var filtro in filtros)
            {
                existeFiltros = true;
                var regra = request.QueryString[filtro];
                string key = filtro.Substring(0, filtro.Length - filtroKey.Length);

                if (regra.Contains(separador.ToString()))
                {
                    var regras = regra.Split(separador);
                    Campos.FirstOrDefault(c => c.Nome.Equals(key)).Valor1 = regras.First();
                    Campos.FirstOrDefault(c => c.Nome.Equals(key)).Valor2 = regras.Last();
                }
                else
                {
                    Campos.FirstOrDefault(c => c.Nome.Equals(key)).Valor1 = regra;
                }
            }
            return existeFiltros;
        }
        public IQueryable<T> AplicarFiltro(IQueryable<T> dataSource, HttpRequestBase request)
        {
            if (AtribuirValores(request))
            {
                foreach (var campo in Campos.Where(c => !string.IsNullOrEmpty(c.Valor1)))
                {                
                    dataSource = dataSource.Where(campo.Expressao.Invoke(campo.Valor1,campo.Valor2));
                }
            } 
            
            return dataSource;
        }

        public string GetHtml(string formAction)
        {
            string html =String.Format(
                @"<form action=""{0}"" method=""get"">
                    <table>
                        <thead>
                            <tr>
                                <th colspan=""2"">
                                    Filtro
                                </th>
                            </tr>
                        </thead>
                        <tbody>",formAction);

            foreach (var item in Campos)
            {
                switch (item.Tipo)
                {
                    case Tipo.Numeric:
                        html += String.Format(
                        @"<tr>
                            <td>
                                {0}
                            </td>
                            <td>
                                <input type=""number"" name=""{1}"" value=""{2}"" />
                                <input type=""number"" name=""{3}"" value=""{4}"" />
                            </td>
                         </tr>",
                               item.Nome, item.Nome + filtroKey,item.Valor1, item.Nome + filtroKey, item.Valor2);
                        break;
                    case Tipo.DateTime:
                        html += String.Format(
                        @"<tr>
                            <td>
                                {0}
                            </td>
                            <td>
                                <input type=""date"" placeholder=""De"" name=""De{1}"" value=""{2}""/>
                                <input type=""date"" placeholder=""Ate"" name=""Ate{3}"" value=""{4}""/>
                            </td>
                         </tr>",
                               item.Nome, item.Nome + filtroKey,item.Valor1, item.Nome + filtroKey, item.Valor2);
                        break;
                    case Tipo.String:
                    default:
                        html += String.Format(
                        @"<tr>
                            <td>
                                {0}
                            </td>
                            <td>
                                <input type=""text"" name=""{1}"" value=""{2}""/>
                            </td>
                         </tr>",
                               item.Nome, item.Nome + filtroKey, item.Valor1); ;
                        break;
                }

            }

            html +=
                @"      <tr><td colspan=""2""><input type=""submit"" value=""Aplicar Filtro"" /></td></tr>
                    </tbody>
                </table>
            </form>";
            return html;
        }
        
    }

    public class Campo<T>
    {
        public string Nome { get; set; }
        public Tipo Tipo { get; set; }
        public Operacao Operacao { get; set; }
        public string Valor1 { get; set; }
        public string Valor2 { get; set; }
        public Func<object,object,Expression<Func<T, bool>>> Expressao { get; set; }
    }
}