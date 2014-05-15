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
    public class EventoController : MasterController
    {
        

        //
        // GET: /Evento/

        public ActionResult Index(int? ano, int? localId)
        {
            ViewBag.Local = _Contexto.Local.Where(l => l.Ativo).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetEventos(int? ano, int? localId)
        {

            var query = _Contexto.Evento.Where(e => e.Orcamento.DataEvento.Year == ano && (localId.HasValue && e.Orcamento.LocalId == localId.Value))
                                            .ToList()
                                            .Select(e => new
                                            {
                                                id = e.EventoId,
                                                dia = e.Data.Day,
                                                mes = e.Data.Month,
                                                cor = "rgb(255, 187, 187)",
                                                tipo = e.Orcamento.TipoEvento.Nome,
                                                observacao = e.Observacao,
                                                cliente = e.Orcamento != null ? String.Format("<a href='/Cliente/{0}' target='_blank'>{1}</a>", e.Orcamento.Cliente.ClienteId, e.Orcamento.Cliente.Pessoa.Nome) : ""

                                            });
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /Evento/Details/5

        public ActionResult Details(int id = 0)
        {
            Evento evento = _Contexto.Evento.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }

        //
        // GET: /Evento/Create

        public ActionResult Create(string data)
        {

            return View(new Evento() {Data = Convert.ToDateTime(data) });
        }

        //
        // POST: /Evento/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Evento evento)
        {
            if (evento.OrcamentoId != 0)
            {
                //Valida orçamento
                evento.Orcamento = _Contexto.Orcamento.Find(evento.OrcamentoId);
                if (evento.Orcamento == null)
                    return Json(new {erro = true, msg = "Orçamento não encontrado!" });
            }

            //Valida data
            if (_Contexto.Evento.Any(e => string.Equals(e.Data.ToShortDateString(), evento.Data.ToShortDateString())))
                return Json(new { erro = true, msg = "Data não disponível" });


            if (ModelState.IsValid)
            {
                _Contexto.Evento.Add(evento);
                _Contexto.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrcamentoId = new SelectList(_Contexto.Orcamento, "OrcamentoId", "Situacao", evento.OrcamentoId);
            return View(evento);
        }

        //
        // GET: /Evento/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Evento evento = _Contexto.Evento.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrcamentoId = new SelectList(_Contexto.Orcamento, "OrcamentoId", "Situacao", evento.OrcamentoId);
            return View(evento);
        }

        //
        // POST: /Evento/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Evento evento)
        {
            if (ModelState.IsValid)
            {
                _Contexto.Entry(evento).State = EntityState.Modified;
                _Contexto.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrcamentoId = new SelectList(_Contexto.Orcamento, "OrcamentoId", "Situacao", evento.OrcamentoId);
            return View(evento);
        }

        //
        // GET: /Evento/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Evento evento = _Contexto.Evento.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }

        //
        // POST: /Evento/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Evento evento = _Contexto.Evento.Find(id);
            _Contexto.Evento.Remove(evento);
            _Contexto.SaveChanges();
            return RedirectToAction("Index");
        }

        
    }
}