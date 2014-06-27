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
    [Authorize]
    public class AgendaController : MasterController
    {
        

        //
        // GET: /agenda/

        public ActionResult Index(int? ano, int? localId)
        {
            ViewBag.Local = _Contexto.Local.Where(l => l.Ativo).ToList();
            ViewBag.Ano = ano ?? DateTime.Now.Year;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAgenda(int? ano, int? localId)
        {

            var query = _Contexto.Agenda.Where(e => e.Data.Year == ano && e.LocalId == localId.Value)
                                            .ToList()
                                            .Select(e => new
                                            {
                                                id = e.AgendaId,
                                                dia = e.Data.Day,
                                                mes = e.Data.Month,
                                                cor = "rgb(255, 187, 187)",
                                                tipo = e.TipoEvento.Nome,
                                                observacao = e.Observacao,
                                                cliente = e.Evento != null ? String.Format("<a href='/Cliente/{0}' target='_blank'>{1}</a>", e.Evento.Cliente.ClienteId, e.Evento.Cliente.Pessoa.Nome) : ""

                                            });
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /agenda/Create

        public ActionResult Create(int? localId, string dataEvento)
        {

            Agenda agenda = new Agenda() { Data = DateTime.Parse(dataEvento), LocalId = localId.GetValueOrDefault() };
            ViewBag.Local = _Contexto.Local.ToList();
            ViewBag.TipoEventoId = new SelectList(_Contexto.TipoEvento.ToList(), "TipoEventoId", "Nome");
            return View(agenda);
        }

        //
        // POST: /agenda/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Agenda agenda)
        {
            try
            {
                #region Validacao

                if (agenda.EventoId != 0 && agenda.EventoId != null)
                {
                    //Valida orçamento
                    agenda.Evento = _Contexto.Evento.Find(agenda.EventoId);
                    if (agenda.Evento == null)
                        throw new Exception("Evento não encontrado.");
                }

                //Valida data
                if (_Contexto.Agenda.Any(e => e.Data == agenda.Data && e.LocalId == agenda.LocalId))
                    throw new Exception("Data não disponível" );


                #endregion

                if (ModelState.IsValid)
                {
                    _Contexto.Agenda.Add(agenda);
                    _Contexto.SaveChanges();
                    return Json(new { erro = false, msg = "Evento marcado com sucesso!", ano = agenda.Data.Year });
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
        // GET: /agenda/Edit/5

        public ActionResult Edit(int id = 0)
        {

            Agenda agenda = _Contexto.Agenda.Find(id);


            if (agenda == null)
            {
                return HttpNotFound();
            }
            ViewBag.TipoEventoId = _Contexto.TipoEvento.ToList();
            ViewBag.Local = _Contexto.Local.ToList();
            return View(agenda);
        }

        //
        // POST: /agenda/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Agenda agenda)
        {
            try
            {
                #region Validacao

                if (agenda.EventoId != 0 && agenda.EventoId != null)
                {
                    //Valida orçamento
                    agenda.Evento = _Contexto.Evento.Find(agenda.EventoId);
                    if (agenda.Evento == null)
                        throw new Exception("Evento não encontrado.");
                }

                //Valida data
                if (_Contexto.Agenda.Any(e => e.Data == agenda.Data && e.LocalId == agenda.LocalId))
                    throw new Exception("Data não disponível");


                #endregion



                var _agenda = _Contexto.Agenda.Find(agenda.AgendaId);
                _agenda.Observacao = agenda.Observacao;
                _agenda.AgendaId = agenda.AgendaId;
                _agenda.TipoEventoId = agenda.TipoEventoId;

                _Contexto.SaveChanges();


                return Json(new { erro = false, msg = "Agendamento alterado com sucesso!", ano = _agenda.Data.Year });
            }
            catch (Exception ex)
            {
                return Json(new { erro = true, msg = ex.Message });
            }

            
        }


        //
        // POST: /agenda/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Agenda agenda = _Contexto.Agenda.Find(id);
                _Contexto.Agenda.Remove(agenda);
                _Contexto.SaveChanges();
                return Json(new { erro = false, msg = "Agendamento desmarcado com sucesso!", ano = agenda.Data.Year });
            }
            catch (Exception ex)
            {
                return Json(new { erro = true, msg = ex.Message });
            }
            
        }

        
    }
}