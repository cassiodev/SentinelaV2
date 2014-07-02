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
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.EstadoId = new SelectList(_Contexto.Estado.ToList(), "EstadoId", "UF");
            ViewBag.TipoEventoId = new SelectList(_Contexto.TipoEvento.Where(t => t.Ativo).ToList(), "TipoEventoId", "Nome");
            ViewBag.Adicionais = _Contexto.Adicional.Where(a => a.Ativo).ToList();
            ViewBag.Cardapios = _Contexto.Cardapio.Where(c => c.Ativo).ToList();
            ViewBag.Local = _Contexto.Local.Where(l => l.Ativo).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult Index(Evento evento, FormCollection frm)
        {
            evento.Cliente.Telefone = evento.Cliente.Telefone.RemoveMaskTel();
            evento.Cliente.Celular = evento.Cliente.Celular.RemoveMaskTel();

            if (evento.LocalId != 0)
            {
                var capLocal = _Contexto.Local.Find(evento.LocalId).Capacidade;

                if (capLocal < evento.Convidados + evento.Criancas)
                {
                    ModelState.AddModelError("Convidados", "Capacidade máxima do local: " + capLocal);
                    ModelState.AddModelError("Criancas", "Capacidade máxima do local: " + capLocal);
                }
                if (evento.Convidados + evento.Criancas == 0)
                {
                    ModelState.AddModelError("Convidados", "Mínimo 1 convidado");
                }    
            }

            

            if (ModelState.IsValid)
            {

                var cliente = _Contexto.Cliente.FirstOrDefault(c => string.Equals(evento.Cliente.Pessoa.Email, c.Pessoa.Email));

                if (cliente != null)
                {
                    evento.Cliente = cliente;
                }

                var selectValues = frm.GetValues("Adicionais");

                if (selectValues != null)
                    foreach (int item in selectValues.Select(i => Convert.ToInt32(i)))
                    {
                        evento.Adicional.Add(_Contexto.Adicional.Find(item));
                    }
                evento.DataAlteracao = DateTime.Now;
                evento.DataCadastro = DateTime.Now;

                _Contexto.Evento.Add(evento);
                _Contexto.SaveChanges();
                TempData["message"] = "Evento registrado com sucesso! Aguarde o contato.";
                return RedirectToAction("Index");
            }

            TempData["message"] = "Não foi possível completar sua solicitação, por favor, verifique os campos com erro.";


            if (evento.Cliente.CidadeId != -1 && evento.Cliente.CidadeId != 0)
            {
                var estadoId = _Contexto.Cidade.Find(evento.Cliente.CidadeId).EstadoId;
                ViewBag.EstadoId = new SelectList(_Contexto.Estado.ToList(), "EstadoId", "UF", estadoId);
                ViewBag.CidadeId = new SelectList(_Contexto.Cidade.Where(c => c.EstadoId == estadoId).ToList(), "CidadeId", "Nome", evento.Cliente.CidadeId);
            }
            else
            {
                ViewBag.EstadoId = new SelectList(_Contexto.Estado.ToList(), "EstadoId", "UF");
            }
            
            ViewBag.TipoEventoId = new SelectList(_Contexto.TipoEvento.ToList(), "TipoEventoId", "Nome");
            ViewBag.Adicionais = _Contexto.Adicional.Where(a => a.Ativo).ToList();
            ViewBag.Cardapios = _Contexto.Cardapio.Where(c => c.Ativo).ToList();
            ViewBag.Local = _Contexto.Local.Where(l => l.Ativo).ToList();

            return View(evento);
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

        public ActionResult DetalhesCardapio(int? id)
        {
            Cardapio cardapio = _Contexto.Cardapio.Find(id);
            if (cardapio == null || !cardapio.Ativo)
            {
                return HttpNotFound();
            }

            var Itens = cardapio.CardapioRefeicaoItem
                .Where(r=>r.Refeicao.Ativo)
                .GroupBy(r => r.Refeicao)
                .Select(r => new ViewRefeicao 
                { 
                    Refeicao = r.Key.Nome, 
                    Itens = String.Join(", ",cardapio.CardapioRefeicaoItem.Where(re => re.RefeicaoId == r.Key.RefeicaoId && re.Item.Ativo).OrderBy(re => re.Item.Nome).Select(i => i.Item.Nome)) 
                });

            ViewBag.Refeicoes = Itens.ToList();

            return PartialView("_DetalhesCardapio",cardapio);
        }

        public ActionResult DetalhesLocal(int? id)
        {
            Local local = _Contexto.Local.Find(id);
            if (local == null || !local.Ativo)
            {
                return HttpNotFound();
            }

            
            return PartialView("_DetalhesLocal",local);
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
    public class ViewRefeicao
    {
        public string Refeicao { get; set; }
        public string Itens { get; set; }

    }
}
