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

        public FiltroGenerico<T> AddCampo(string nome, Tipo tipo, Func<object, Expression<Func<T, bool>>> expressao)
        {
            return AddCampo(nome,nome, tipo,expressao);
        }
        public FiltroGenerico<T> AddCampo(string nome,string label, Tipo tipo, Func<object, Expression<Func<T, bool>>> expressao)
        {
            return AddCampo(nome,label, tipo, "", expressao);
        }

        public FiltroGenerico<T> AddCampo(string nome,string label, Tipo tipo, string valor1, Func<object,Expression<Func<T, bool>>> expressao)
        {
            var campo = new Campo<T>();
            campo.Nome = nome;
            campo.Label = label;
            campo.Expressao = expressao;
            campo.Tipo = tipo;
            campo.Valor = valor1;

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
                                
                Campos.FirstOrDefault(c => c.Nome.Equals(key)).Valor = regra;
            }
            return existeFiltros;
        }
        public IQueryable<T> Filtrar(IQueryable<T> dataSource, HttpRequestBase request)
        {
            if (AtribuirValores(request))
            {
                foreach (var campo in Campos.Where(c => !string.IsNullOrEmpty(c.Valor)))
                {
                    switch (campo.Tipo)
                    {
                        case Tipo.String:
                            dataSource = dataSource.Where(campo.Expressao.Invoke(campo.Valor));
                            break;
                        case Tipo.DateTime:
                            dataSource = dataSource.Where(campo.Expressao.Invoke(Convert.ToDateTime(campo.Valor)));
                            break;
                        case Tipo.Numeric:
                            dataSource = dataSource.Where(campo.Expressao.Invoke(Convert.ToDouble(campo.Valor)));
                            break;
                        default:
                            break;
                    }
                }
            } 
            
            return dataSource;
        }

        
        public IEnumerable<T> Ordenar(IEnumerable<T> source,string tipo, string sortBy, string sortDirection)
        {
            var param = Expression.Parameter(typeof(T), "item");

            var sortExpression = Expression.Lambda<Func<T, object>>
                (Expression.Convert(Expression.Property(param, sortBy), typeof(object)), param);

            switch (sortDirection.ToLower())
            {
                case "asc":
                    return source.AsQueryable<T>().OrderBy<T, object>(sortExpression);
                default:
                    return source.AsQueryable<T>().OrderByDescending<T, object>(sortExpression);

            }
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
                            </td>
                         </tr>",
                               item.Label, item.Nome + filtroKey,item.Valor);
                        break;
                    case Tipo.DateTime:
                        html += String.Format(
                        @"<tr>
                            <td>
                                {0}
                            </td>
                            <td>
                                <input type=""text"" class=""data"" name=""{1}"" value=""{2}""/>
                            </td>
                         </tr>",
                               item.Label, item.Nome + filtroKey, item.Valor);
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
                               item.Label, item.Nome + filtroKey, item.Valor); ;
                        break;
                }

            }

            html +=
                @"      <tr><td colspan=""2""><input type=""submit"" style=""float:right;"" value=""Aplicar Filtro"" /></td></tr>
                    </tbody>
                </table>
            </form>";
            return html;
        }
        
    }
    public class GenericSorter<T>
    {
        
    }
    public class Campo<T>
    {
        public string Nome { get; set; }
        public Tipo Tipo { get; set; }
        public Operacao Operacao { get; set; }
        public string Valor { get; set; }
        public Func<object,Expression<Func<T, bool>>> Expressao { get; set; }

        public string Label { get; set; }
    }
}