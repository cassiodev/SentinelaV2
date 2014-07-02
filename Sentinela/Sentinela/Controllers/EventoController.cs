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
using Sentinela.Core;
using System.Linq.Expressions;


namespace Sentinela.Controllers
{
    [Authorize]
    public class EventoController : MasterController
    {
        

        //
        // GET: /Evento/

        public ActionResult Index(int? page)
        {


            int pageSize =  Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            int pageNumber = (page ?? 1);

            FiltroGenerico<Evento> filtro = new FiltroGenerico<Evento>();

            filtro.AddCampo("Nome","Nome Cliente", Tipo.String, (arg1) => campo => campo.Cliente.Pessoa.Nome.Contains((string)arg1));
            filtro.AddCampo("DeDataRecebido", "Data Recebimento(De)", Tipo.DateTime, (arg1) => campo => campo.DataCadastro >= (DateTime)arg1);
            filtro.AddCampo("AteDataRecebido", "Data Recebimento(Até)", Tipo.DateTime, (arg1) => campo => campo.DataCadastro <= (DateTime)arg1);
            var eventos = _Contexto.Evento.OrderByDescending(o => o.EventoId).Include(o => o.Cliente).Include(o => o.Local).Include(o => o.TipoEvento);
            
            eventos = filtro.Filtrar(eventos, Request);

            ViewBag.Filtro = filtro.GetHtml("/Evento/Index");
            
            return View(eventos.ToPagedList(pageNumber,pageSize));
        }

        //
        // GET: /Evento/Detalhes/5

        public ActionResult Details(int id = 0)
        {
            Evento orcamento = _Contexto.Evento.Find(id);
            if (orcamento == null)
            {
                return HttpNotFound();
            }
            return View(orcamento);
        }

        
        //
        // GET: /Orcamento/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Evento evento = _Contexto.Evento.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocalId = new SelectList(_Contexto.Local, "LocalId", "Nome", evento.LocalId);
            ViewBag.TipoEventoId = new SelectList(_Contexto.TipoEvento, "TipoEventoId", "Nome", evento.TipoEventoId);
            ViewBag.CardapioId = new SelectList(_Contexto.Cardapio, "CardapioId", "Nome", evento.CardapioId);
            ViewBag.Adicionais = new MultiSelectList(_Contexto.Adicional, "AdicionalId", "Nome", evento.Adicional.Select(a => a.AdicionalId));
            return View(evento);
        }

        //
        // POST: /Orcamento/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Evento evento, IEnumerable<int> Adicionais, FormCollection form)
        {
            var capLocal = _Contexto.Local.Find(evento.LocalId).Capacidade;

            if (!evento.Criancas.HasValue)
                evento.Criancas = 0;

            if (capLocal < evento.Convidados + evento.Criancas)
            {
                ModelState.AddModelError("Convidados", "Capacidade máxima do local: " + capLocal);
                ModelState.AddModelError("Criancas", "Capacidade máxima do local: " + capLocal);
            }
            if (evento.Convidados + evento.Criancas == 0)
            {
                ModelState.AddModelError("Convidados", "Mínimo 1 convidado");
            }

            var _evento = _Contexto.Evento.Find(evento.EventoId);

            if (ModelState.IsValid && _evento != null)
            {

                _evento.Adicional.Clear();
                if (Adicionais != null)
                    foreach (var item in Adicionais)
                        _evento.Adicional.Add(_Contexto.Adicional.Find(item));

                _evento.ClienteId = evento.ClienteId;
                _evento.LocalId = evento.LocalId;
                _evento.DataEvento = evento.DataEvento;
                _evento.Convidados = evento.Convidados;
                _evento.Periodo = evento.Periodo;
                _evento.TipoEventoId = evento.TipoEventoId;
                _evento.CardapioId = evento.CardapioId;
                _evento.DataAlteracao = DateTime.Now;

                _Contexto.SaveChanges();
                TempData["message"] = "Alteração feita com sucesso!";
                return RedirectToAction("Index");
            }
            ViewBag.LocalId = new SelectList(_Contexto.Local.Where(e => e.Ativo), "LocalId", "Nome", evento.LocalId);
            ViewBag.TipoEventoId = new SelectList(_Contexto.TipoEvento.Where(e => e.Ativo), "TipoEventoId", "Nome", evento.TipoEventoId);
            ViewBag.CardapioId = new SelectList(_Contexto.Cardapio.Where(e => e.Ativo), "CardapioId", "Nome", evento.CardapioId);
            ViewBag.Adicionais = new MultiSelectList(_Contexto.Adicional.Where(e => e.Ativo), "AdicionalId", "Nome", evento.Adicional.Select(a => a.AdicionalId));
            return View(evento);
        }

        //
        // GET: /Orcamento/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Evento orcamento = _Contexto.Evento.Find(id);
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
            Evento orcamento = _Contexto.Evento.Find(id);
            _Contexto.Evento.Remove(orcamento);
            _Contexto.SaveChanges();
            TempData["message"] = "Exclusão feita com sucesso!";

            return RedirectToAction("Index");
        }

        
    }
}