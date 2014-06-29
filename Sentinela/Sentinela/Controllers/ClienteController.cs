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

namespace Sentinela.Controllers
{
    [Authorize]

    public class ClienteController : MasterController
    {
        

        //
        // GET: /Cliente/

        public ActionResult Index(int? page)
        {


            int pageSize =  Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            int pageNumber = (page ?? 1);

            FiltroGenerico<Cliente> filtro = new FiltroGenerico<Cliente>();

            filtro.AddCampo("Nome",Tipo.String,arg => prop => prop.Pessoa.Nome.ToLower().Contains(((string)arg).ToLower()));
            var cliente = _Contexto.Cliente.OrderBy(c => c.ClienteId).Include(c => c.Cidade).Include(c => c.Pessoa);
            cliente = filtro.Filtrar(cliente, Request);

            ViewBag.Filtro = filtro.GetHtml("/Cliente/Index");
            
            return View(cliente.ToPagedList(pageNumber,pageSize));
        }

        //
        // GET: /Cliente/Detalhes/5

        public ActionResult Details(int id = 0)
        {
            Cliente cliente = _Contexto.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        //
        // GET: /Cliente/Create

        public ActionResult Create()
        {
            ViewBag.CidadeId = new SelectList(_Contexto.Cidade, "CidadeId", "Nome");
            ViewBag.EstadoId = new SelectList(_Contexto.Estado, "EstadoId", "UF");

            return View();
        }

        //
        // POST: /Cliente/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cliente cliente)
        {
            cliente.Telefone = cliente.Telefone.RemoveMaskTel();
            cliente.Celular = cliente.Celular.RemoveMaskTel();
            if (ModelState.IsValid)
            {
                _Contexto.Cliente.Add(cliente);
                _Contexto.SaveChanges();
                TempData["message"] = "Adicionado com sucesso!";
                return RedirectToAction("Index");
            }

            ViewBag.CidadeId = new SelectList(_Contexto.Cidade, "CidadeId", "Nome", cliente.CidadeId);
            return View(cliente);
        }

        //
        // GET: /Cliente/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Cliente cliente = _Contexto.Cliente.Include("Cidade").Include("Cidade.Estado").FirstOrDefault(f => f.ClienteId == id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.EstadoId = new SelectList(_Contexto.Estado, "EstadoId", "UF",cliente.Cidade.EstadoId);
            ViewBag.CidadeId = new SelectList(_Contexto.Cidade.Where(c => c.EstadoId == cliente.Cidade.EstadoId), "CidadeId", "Nome", cliente.CidadeId);
            return View(cliente);
        }

        //
        // POST: /Cliente/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cliente cliente)
        {
            cliente.Telefone = cliente.Telefone.RemoveMaskTel();
            cliente.Celular = cliente.Celular.RemoveMaskTel();

            if (ModelState.IsValid)
            {
                _Contexto.Entry(cliente).State = EntityState.Modified;
                TempData["message"] = "Alteração feita com sucesso!";
                _Contexto.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CidadeId = new SelectList(_Contexto.Cidade, "CidadeId", "Nome", cliente.CidadeId);
            ViewBag.ClienteId = new SelectList(_Contexto.Pessoa, "PessoaId", "Email", cliente.ClienteId);
            return View(cliente);
        }

        //
        // GET: /Cliente/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Cliente cliente = _Contexto.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        //
        // POST: /Cliente/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = _Contexto.Cliente.Find(id);
            _Contexto.Cliente.Remove(cliente);
            _Contexto.SaveChanges();
            TempData["message"] = "Exclusão feita com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetClientes(string nome)
        {
            int id;

            if (int.TryParse(nome, out id))
            {
                return Json(_Contexto.Cliente.Where(c => c.ClienteId == id)
                                            .Select(c => new 
                                            {   
                                                id = c.ClienteId,
                                                text = c.Pessoa.Nome
                                            }), JsonRequestBehavior.AllowGet);
            }

            return Json(_Contexto.Cliente.Where(c => c.Pessoa.Nome.ToUpper().Contains(nome.ToUpper()))
                                            .Select(c => new 
                                            {   
                                                id = c.ClienteId,
                                                text = c.Pessoa.Nome
                                            }), JsonRequestBehavior.AllowGet);
        }
        
    }
}