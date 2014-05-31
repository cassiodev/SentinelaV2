using Sentinela.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace Sentinela.Controllers
{
    public class MasterController : Controller
    {
        //
        // GET: /Master/
        protected Contexto _Contexto = new Contexto();

        protected bool IsLogado()
        {
            
            return HttpContext.User.Identity.IsAuthenticated;
        }
        protected override void Dispose(bool disposing)
        {
            _Contexto.Dispose();
            base.Dispose(disposing);
        }
    }
}
