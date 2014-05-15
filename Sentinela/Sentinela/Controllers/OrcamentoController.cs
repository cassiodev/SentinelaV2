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
    public class OrcamentoController : MasterController
    {
        

        //
        // GET: /Orcamento/

        public ActionResult Index(int? page)
        {


            int pageSize =  Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            int pageNumber = (page ?? 1);



            var orcamento = _Contexto.Orcamento.OrderBy(o => o.OrcamentoId).Include(o => o.Cliente).Include(o => o.Local).Include(o => o.TipoEvento);
            return View(orcamento.ToPagedList(pageNumber,pageSize));
        }

        //
        // GET: /Orcamento/Details/5

        public ActionResult Details(int id = 0)
        {
            Orcamento orcamento = _Contexto.Orcamento.Find(id);
            if (orcamento == null)
            {
                return HttpNotFound();
            }
            return View(orcamento);
        }

        //
        // GET: /Orcamento/Create

        public ActionResult Create()
        {
            ViewBag.ClienteId = new SelectList(_Contexto.Cliente, "ClienteId", "Logradouro");
            ViewBag.LocalId = new SelectList(_Contexto.Local, "LocalId", "Nome");
            ViewBag.TipoEventoId = new SelectList(_Contexto.TipoEvento, "TipoEventoId", "TipoEventoId");
            return View();
        }

        //
        // POST: /Orcamento/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Orcamento orcamento)
        {
            if (ModelState.IsValid)
            {
                _Contexto.Orcamento.Add(orcamento);
                _Contexto.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteId = new SelectList(_Contexto.Cliente, "ClienteId", "Logradouro", orcamento.ClienteId);
            ViewBag.LocalId = new SelectList(_Contexto.Local, "LocalId", "Nome", orcamento.LocalId);
            ViewBag.TipoEventoId = new SelectList(_Contexto.TipoEvento, "TipoEventoId", "TipoEventoId", orcamento.TipoEventoId);
            return View(orcamento);
        }

        //
        // GET: /Orcamento/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Orcamento orcamento = _Contexto.Orcamento.Find(id);
            if (orcamento == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteId = new SelectList(_Contexto.Cliente, "ClienteId", "Logradouro", orcamento.ClienteId);
            ViewBag.LocalId = new SelectList(_Contexto.Local, "LocalId", "Nome", orcamento.LocalId);
            ViewBag.TipoEventoId = new SelectList(_Contexto.TipoEvento, "TipoEventoId", "TipoEventoId", orcamento.TipoEventoId);
            return View(orcamento);
        }

        //
        // POST: /Orcamento/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Orcamento orcamento)
        {
            if (ModelState.IsValid)
            {
                _Contexto.Entry(orcamento).State = EntityState.Modified;
                _Contexto.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteId = new SelectList(_Contexto.Cliente, "ClienteId", "Logradouro", orcamento.ClienteId);
            ViewBag.LocalId = new SelectList(_Contexto.Local, "LocalId", "Nome", orcamento.LocalId);
            ViewBag.TipoEventoId = new SelectList(_Contexto.TipoEvento, "TipoEventoId", "TipoEventoId", orcamento.TipoEventoId);
            return View(orcamento);
        }

        //
        // GET: /Orcamento/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Orcamento orcamento = _Contexto.Orcamento.Find(id);
            if (orcamento == null)
            {
                return HttpNotFound();
            }
            return View(orcamento);
        }

        //
        // POST: /Orcamento/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orcamento orcamento = _Contexto.Orcamento.Find(id);
            _Contexto.Orcamento.Remove(orcamento);
            _Contexto.SaveChanges();
            return RedirectToAction("Index");
        }

        
    }
}