using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System.Configuration;
using Sentinela.Models;

namespace Sentinela.Controllers
{
    public class TipoEventoController : MasterController
    {
        

        //
        // GET: /TipoEvento/

        public ActionResult Index(int? page)
        {


            int pageSize =  Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            int pageNumber = (page ?? 1);



            return View(_Contexto.TipoEvento.OrderBy(t => t.TipoEventoId).ToPagedList(pageNumber,pageSize));
        }

        //
        // GET: /TipoEvento/Details/5

        public ActionResult Details(int id = 0)
        {
            TipoEvento tipoevento = _Contexto.TipoEvento.Find(id);
            if (tipoevento == null)
            {
                return HttpNotFound();
            }
            return View(tipoevento);
        }

        //
        // GET: /TipoEvento/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /TipoEvento/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TipoEvento tipoevento)
        {
            if (ModelState.IsValid)
            {
                _Contexto.TipoEvento.Add(tipoevento);
                _Contexto.SaveChanges();
                TempData["message"] = "Adicionado com sucesso!";
                return RedirectToAction("Index");
            }

            return View(tipoevento);
        }

        //
        // GET: /TipoEvento/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TipoEvento tipoevento = _Contexto.TipoEvento.Find(id);
            if (tipoevento == null)
            {
                return HttpNotFound();
            }
            return View(tipoevento);
        }

        //
        // POST: /TipoEvento/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TipoEvento tipoevento)
        {
            if (ModelState.IsValid)
            {
                _Contexto.Entry(tipoevento).State = EntityState.Modified;
                _Contexto.SaveChanges();
                TempData["message"] = "Alteração feita com sucesso!";
                return RedirectToAction("Index");
            }
            return View(tipoevento);
        }

        //
        // GET: /TipoEvento/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TipoEvento tipoevento = _Contexto.TipoEvento.Find(id);
            if (tipoevento == null)
            {
                return HttpNotFound();
            }
            return View(tipoevento);
        }

        //
        // POST: /TipoEvento/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoEvento tipoevento = _Contexto.TipoEvento.Find(id);
            _Contexto.TipoEvento.Remove(tipoevento);
            _Contexto.SaveChanges();
            TempData["message"] = "Exclusão feita com sucesso!";
            return RedirectToAction("Index");
        }

        
    }
}