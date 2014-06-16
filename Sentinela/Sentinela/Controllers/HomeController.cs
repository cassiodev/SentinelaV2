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
            ViewBag.TipoEventoId = new SelectList(_Contexto.TipoEvento.ToList(), "TipoEventoId", "Nome");
            ViewBag.Adicionais = _Contexto.Adicional.Where(a => a.Ativo).ToList();
            ViewBag.Cardapios = _Contexto.Cardapio.Where(c => c.Ativo).ToList();
            ViewBag.Local = _Contexto.Local.Where(l => l.Ativo).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult Index(Orcamento orcamento, FormCollection frm)
        {

            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    TempData["message"] += "\n" + error.ErrorMessage;
                }
            }

            if (ModelState.IsValid)
            {

                var cliente = _Contexto.Cliente.FirstOrDefault(c => string.Equals(orcamento.Cliente.Pessoa.Email, c.Pessoa.Email));

                if (cliente != null)
                {
                    orcamento.Cliente = cliente;
                }

                var selectValues = frm.GetValues("Adicionais");

                if (selectValues != null)
                    foreach (int item in selectValues.Select(i => Convert.ToInt32(i)))
                    {
                        orcamento.Adicional.Add(_Contexto.Adicional.Find(item));
                    }
                orcamento.DataAlteracao = DateTime.Now;
                orcamento.DataOrcamento = DateTime.Now;

                _Contexto.Orcamento.Add(orcamento);
                _Contexto.SaveChanges();
                TempData["message"] = "Evento registrado com sucesso! Aguarde o contato.";
                return RedirectToAction("Index");
            }

            

            ViewBag.EstadoId = new SelectList(_Contexto.Estado.ToList(), "EstadoId", "UF");
            ViewBag.TipoEventoId = new SelectList(_Contexto.TipoEvento.ToList(), "TipoEventoId", "Nome");
            ViewBag.Adicionais = _Contexto.Adicional.Where(a => a.Ativo).ToList();
            ViewBag.Cardapios = _Contexto.Cardapio.Where(c => c.Ativo).ToList();
            ViewBag.Local = _Contexto.Local.Where(l => l.Ativo).ToList();

            return View(orcamento);
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

            var Itens = cardapio.CardapioRefeicaoItem.GroupBy(r => r.Refeicao)
                .Select(r => new ViewRefeicao { Refeicao = r.Key.Nome, Itens = String.Join(", ",cardapio.CardapioRefeicaoItem.Where(re => re.RefeicaoId == r.Key.RefeicaoId && re.Item.Ativo).OrderBy(re => re.Item.Nome).Select(i => i.Item.Nome)) });

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
    }
    public class ViewRefeicao
    {
        public string Refeicao { get; set; }
        public string Itens { get; set; }

    }
}
