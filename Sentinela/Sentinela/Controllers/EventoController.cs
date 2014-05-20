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
            ViewBag.Ano = ano ?? DateTime.Now.Year;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetEventos(int? ano, int? localId)
        {

            var query = _Contexto.Evento.Where(e => e.Data.Year == ano && e.LocalId == localId.Value)
                                            .ToList()
                                            .Select(e => new
                                            {
                                                id = e.EventoId,
                                                dia = e.Data.Day,
                                                mes = e.Data.Month,
                                                cor = "rgb(255, 187, 187)",
                                                tipo = e.TipoEvento.Nome,
                                                observacao = e.Observacao,
                                                cliente = e.Orcamento != null ? String.Format("<a href='/Cliente/{0}' target='_blank'>{1}</a>", e.Orcamento.Cliente.ClienteId, e.Orcamento.Cliente.Pessoa.Nome) : ""

                                            });
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Evento/Create

        public ActionResult Create(int? localId, string dataEvento)
        {
            
            Evento evento = new Evento() { Data = DateTime.Parse(dataEvento) };
            ViewBag.Local = _Contexto.Local.ToList();
            ViewBag.TipoEventoId = new SelectList(_Contexto.TipoEvento.ToList(), "TipoEventoId", "Nome");
            return View(evento);
        }

        //
        // POST: /Evento/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Evento evento)
        {
            try
            {
                #region Validacao

                if (evento.OrcamentoId != 0 && evento.OrcamentoId != null)
                {
                    //Valida orçamento
                    evento.Orcamento = _Contexto.Orcamento.Find(evento.OrcamentoId);
                    if (evento.Orcamento == null)
                        throw new Exception("Orçamento não encontrado.");
                }

                //Valida data
                if (_Contexto.Evento.Any(e => e.Data == evento.Data && e.LocalId == evento.LocalId))
                    throw new Exception("Data não disponível" );


                #endregion

                if (ModelState.IsValid)
                {
                    _Contexto.Evento.Add(evento);
                    _Contexto.SaveChanges();
                    return Json(new { erro = false, msg = "Evento adicionado com sucesso!", ano = evento.Data.Year });
                }

                string msg = "Erro na validação de dados: ";

                foreach (var item in ModelState.Values)
                {
                    foreach (var erros in item.Errors)
                    {
                        msg += erros.ErrorMessage;
                    }
                }

                throw new Exception(msg);
            }
            catch (Exception ex)
            {
                return Json(new { erro = true, msg = ex.Message });
            }
            
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
            ViewBag.TipoEventoId = _Contexto.TipoEvento.ToList();
            ViewBag.Local = _Contexto.Local.ToList();
            return View(evento);
        }

        //
        // POST: /Evento/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Evento evento)
        {
            try
            {
                #region Validacao

                if (evento.OrcamentoId != 0 && evento.OrcamentoId != null)
                {
                    //Valida orçamento
                    evento.Orcamento = _Contexto.Orcamento.Find(evento.OrcamentoId);
                    if (evento.Orcamento == null)
                        throw new Exception("Orçamento não encontrado.");
                }

                //Valida data
                if (_Contexto.Evento.Any(e => e.Data == evento.Data && e.LocalId == evento.LocalId))
                    throw new Exception("Data não disponível");


                #endregion



                var _evento = _Contexto.Evento.Find(evento.EventoId);
                _evento.Observacao = evento.Observacao;
                _evento.OrcamentoId = evento.OrcamentoId;
                _evento.TipoEventoId = evento.TipoEventoId;

                _Contexto.SaveChanges();


                return Json(new { erro = false, msg = "Evento alterado com sucesso!", ano = _evento.Data.Year });
            }
            catch (Exception ex)
            {
                return Json(new { erro = true, msg = ex.Message });
            }

            
        }


        //
        // POST: /Evento/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Evento evento = _Contexto.Evento.Find(id);
                _Contexto.Evento.Remove(evento);
                _Contexto.SaveChanges();
                return Json(new { erro = false, msg = "Evento desmarcado com sucesso!", ano = evento.Data.Year });
            }
            catch (Exception ex)
            {
                return Json(new { erro = true, msg = ex.Message });
            }
            
        }

        
    }
}