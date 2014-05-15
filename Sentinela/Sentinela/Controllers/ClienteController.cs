﻿using System;
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
    public class ClienteController : MasterController
    {
        

        //
        // GET: /Cliente/

        public ActionResult Index(int? page)
        {


            int pageSize =  Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            int pageNumber = (page ?? 1);



            var cliente = _Contexto.Cliente.OrderBy(c => c.ClienteId).Include(c => c.Cidade).Include(c => c.Pessoa);
            return View(cliente.ToPagedList(pageNumber,pageSize));
        }

        //
        // GET: /Cliente/Details/5

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
            ViewBag.ClienteId = new SelectList(_Contexto.Pessoa, "PessoaId", "Email");
            return View();
        }

        //
        // POST: /Cliente/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _Contexto.Cliente.Add(cliente);
                _Contexto.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CidadeId = new SelectList(_Contexto.Cidade, "CidadeId", "Nome", cliente.CidadeId);
            ViewBag.ClienteId = new SelectList(_Contexto.Pessoa, "PessoaId", "Email", cliente.ClienteId);
            return View(cliente);
        }

        //
        // GET: /Cliente/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Cliente cliente = _Contexto.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.EstadoId = new SelectList(_Contexto.Estado, "EstadoId", "UF");
            ViewBag.CidadeId = new SelectList(_Contexto.Cidade, "CidadeId", "Nome", cliente.CidadeId);
            ViewBag.ClienteId = new SelectList(_Contexto.Pessoa, "PessoaId", "Email", cliente.ClienteId);
            return View(cliente);
        }

        //
        // POST: /Cliente/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _Contexto.Entry(cliente).State = EntityState.Modified;
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
            return RedirectToAction("Index");
        }

        
    }
}