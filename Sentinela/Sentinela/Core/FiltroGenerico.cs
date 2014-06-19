using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Linq.Expressions;

namespace Sentinela.Core
{
    public class FiltroGenerico<T>
    {
        private Dictionary<string, Func<object, Expression<Func<T, bool>>>> objFiltros = new Dictionary<string, Func<object, Expression<Func<T, bool>>>>();
        public void addFiltro(string cod, Func<object, Expression<Func<T, bool>>> expr)
        {
            objFiltros.Add(cod, expr);
        }

    }
}