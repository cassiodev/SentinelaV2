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
    public class CidadeController : MasterController
    {
        

        //
        // GET: /Cidade/

        public ActionResult Index(int? page)
        {


            int pageSize =  Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            int pageNumber = (page ?? 1);



            var cidade = _Contexto.Cidade.OrderBy(c => c.CidadeId).Include(c => c.Estado);
            return View(cidade.ToPagedList(pageNumber,pageSize));
        }


        //
        // GET: /Cidade/Create

        public ActionResult Create()
        {
            ViewBag.EstadoId = new SelectList(_Contexto.Estado, "EstadoId", "UF");
            return View();
        }

        //
        // POST: /Cidade/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cidade cidade)
        {
            if (ModelState.IsValid)
            {
                _Contexto.Cidade.Add(cidade);
                _Contexto.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EstadoId = new SelectList(_Contexto.Estado, "EstadoId", "UF", cidade.EstadoId);
            return View(cidade);
        }

        //
        // GET: /Cidade/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Cidade cidade = _Contexto.Cidade.Find(id);
            if (cidade == null)
            {
                return HttpNotFound();
            }
            ViewBag.EstadoId = new SelectList(_Contexto.Estado, "EstadoId", "UF", cidade.EstadoId);
            return View(cidade);
        }

        //
        // POST: /Cidade/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cidade cidade)
        {
            if (ModelState.IsValid)
            {
                _Contexto.Entry(cidade).State = EntityState.Modified;
                _Contexto.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EstadoId = new SelectList(_Contexto.Estado, "EstadoId", "UF", cidade.EstadoId);
            return View(cidade);
        }

        //
        // GET: /Cidade/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Cidade cidade = _Contexto.Cidade.Find(id);
            if (cidade == null)
            {
                return HttpNotFound();
            }
            return View(cidade);
        }

        //
        // POST: /Cidade/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cidade cidade = _Contexto.Cidade.Find(id);
            _Contexto.Cidade.Remove(cidade);
            _Contexto.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetCidades(int? estadoId)
        {
            if (!estadoId.HasValue)
                return new EmptyResult();
            else
                return Json(_Contexto.Cidade.Where(e => e.EstadoId == estadoId.Value).Select(e => new { id = e.CidadeId, text = e.Nome }).ToList(), JsonRequestBehavior.AllowGet);
        }
        
    }
}