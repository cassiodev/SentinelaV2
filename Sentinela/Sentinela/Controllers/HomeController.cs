using Sentinela.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sentinela.Controllers
{
    public class HomeController : MasterController
    {
        public ActionResult Index()
        {
            ViewBag.EstadoId = new SelectList(_Contexto.Estado.ToList(), "EstadoId", "UF");
            ViewBag.TipoEventoId = new SelectList(_Contexto.TipoEvento.ToList(), "TipoEventoId", "Nome");
            ViewBag.Adicionais = _Contexto.Adicional.Where(a => a.Ativo).ToList();
            ViewBag.Cardapios = _Contexto.Cardapio.Where(c => c.Ativo).ToList();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
