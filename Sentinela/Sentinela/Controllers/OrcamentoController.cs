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
    public class OrcamentoController : MasterController
    {
        

        //
        // GET: /Orcamento/

        public ActionResult Index(int? page)
        {


            int pageSize =  Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            int pageNumber = (page ?? 1);

            FiltroGenerico<Orcamento> filtro = new FiltroGenerico<Orcamento>();

            filtro.AddCampo("Nome","Nome Cliente", Tipo.String, (arg1) => campo => campo.Cliente.Pessoa.Nome.Contains((string)arg1));
            filtro.AddCampo("DeDataRecebido", "Data Recebimento(De)", Tipo.DateTime, (arg1) => campo => campo.DataOrcamento >= (DateTime)arg1);
            filtro.AddCampo("AteDataRecebido", "Data Recebimento(Até)", Tipo.DateTime, (arg1) => campo => campo.DataOrcamento <= (DateTime)arg1);
            var orcamentos = _Contexto.Orcamento.OrderByDescending(o => o.OrcamentoId).Include(o => o.Cliente).Include(o => o.Local).Include(o => o.TipoEvento);
            
            orcamentos = filtro.Filtrar(orcamentos, Request);

            ViewBag.Filtro = filtro.GetHtml("/Orcamento/Index");
            
            return View(orcamentos.ToPagedList(pageNumber,pageSize));
        }

        //
        // GET: /Orcamento/Detalhes/5

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
        // GET: /Orcamento/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Orcamento orcamento = _Contexto.Orcamento.Find(id);
            if (orcamento == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocalId = new SelectList(_Contexto.Local, "LocalId", "Nome", orcamento.LocalId);
            ViewBag.TipoEventoId = new SelectList(_Contexto.TipoEvento, "TipoEventoId", "Nome", orcamento.TipoEventoId);
            ViewBag.CardapioId = new SelectList(_Contexto.Cardapio, "CardapioId", "Nome", orcamento.CardapioId);
            ViewBag.Adicionais = new MultiSelectList(_Contexto.Adicional, "AdicionalId", "Nome", orcamento.Adicional.Select(a => a.AdicionalId));
            return View(orcamento);
        }

        //
        // POST: /Orcamento/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Orcamento orcamento, IEnumerable<int> Adicionais, FormCollection form)
        {
                var _orcamento = _Contexto.Orcamento.Find(orcamento.OrcamentoId);

            if (ModelState.IsValid && _orcamento != null)
            {

                _orcamento.Adicional.Clear();
                if (Adicionais != null)
                    foreach (var item in Adicionais)
                        _orcamento.Adicional.Add(_Contexto.Adicional.Find(item));

                _orcamento.ClienteId = orcamento.ClienteId;
                _orcamento.LocalId = orcamento.LocalId;
                _orcamento.DataEvento = orcamento.DataEvento;
                _orcamento.Convidados = orcamento.Convidados;
                _orcamento.Periodo = orcamento.Periodo;
                _orcamento.TipoEventoId = orcamento.TipoEventoId;
                _orcamento.CardapioId = orcamento.CardapioId;
                _orcamento.DataAlteracao = DateTime.Now;

                _Contexto.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocalId = new SelectList(_Contexto.Local, "LocalId", "Nome", orcamento.LocalId);
            ViewBag.TipoEventoId = new SelectList(_Contexto.TipoEvento, "TipoEventoId", "Nome", orcamento.TipoEventoId);
            ViewBag.CardapioId = new SelectList(_Contexto.Cardapio, "CardapioId", "Nome", orcamento.CardapioId);
            ViewBag.Adicional = new MultiSelectList(_Contexto.Adicional, "AdicionalId", "Nome", orcamento.Adicional.Select(a => a.AdicionalId));
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