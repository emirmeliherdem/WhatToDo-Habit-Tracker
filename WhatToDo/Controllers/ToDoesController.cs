﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WhatToDo.Models;

namespace WhatToDo.Controllers
{
    public class ToDoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        // GET: ToDoes
        public ActionResult Index()
        {
            return View();
        }

        private IEnumerable<ToDo> GetToDoes()
        {
            string currentUserId = User.Identity.GetUserId();
            // find the user with the given user id
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            return db.ToDos.ToList().Where(x => x.User == currentUser);
        }

        public ActionResult BuildToDoTable()
        {
            return PartialView("_ToDoTable", GetToDoes());
        }

        //// GET: ToDoes/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ToDo toDo = db.ToDos.Find(id);
        //    if (toDo == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(toDo);
        //}

        //// GET: ToDoes/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ToDoes/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Description,IsDone")] ToDo toDo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string currentUserId = User.Identity.GetUserId();
        //        // find the user with the given user id
        //        ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
        //        // match current to do with the user
        //        toDo.User = currentUser;
        //        db.ToDos.Add(toDo);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(toDo);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AJAXCreate([Bind(Include = "Id,Description")] ToDo toDo)
        {
            if (ModelState.IsValid && toDo.Description != null)
            {
                string currentUserId = User.Identity.GetUserId();
                // find the user with the given user id
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                // match current to do with the user
                toDo.User = currentUser;
                toDo.IsDone = false;
                db.ToDos.Add(toDo);
                db.SaveChanges();

                ModelState.Clear();
            }

            System.Diagnostics.Debug.WriteLine("******To Do Created! Id: " + toDo.Id + " Desc: " + toDo.Description);

            return PartialView("_ToDoTable", GetToDoes());
        }

        //// GET: ToDoes/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ToDo toDo = db.ToDos.Find(id);
        //    if (toDo == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(toDo);
        //}

        //// POST: ToDoes/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Description,IsDone")] ToDo toDo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(toDo).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(toDo);
        //}

        [HttpPost]
        public ActionResult AJAXEdit(int? id, bool value)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }

            // set IsDone value to the given bool value & update database
            toDo.IsDone = value;
            db.Entry(toDo).State = EntityState.Modified;
            db.SaveChanges();
            return PartialView("_ToDoTable", GetToDoes());
        }

        //// GET: ToDoes/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ToDo toDo = db.ToDos.Find(id);
        //    if (toDo == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(toDo);
        //}

        //// POST: ToDoes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    ToDo toDo = db.ToDos.Find(id);
        //    db.ToDos.Remove(toDo);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //// POST: ToDoes/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ToDo toDo = db.ToDos.Find(id);
        //    if (toDo == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    db.ToDos.Remove(toDo);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        // POST: ToDoes/AJAXDelete/5
        public ActionResult AJAXDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }

            db.ToDos.Remove(toDo);
            db.SaveChanges();
            return PartialView("_ToDoTable", GetToDoes());
        }

        public ActionResult ClearAll()
        {
            IEnumerable<ToDo> toDoes = GetToDoes();
            foreach (var toDo in toDoes)
			{
				db.ToDos.Remove(toDo);
			}
			db.SaveChanges();
			return RedirectToAction("Index");
        }

        public ActionResult ClearDone()
        {
            IEnumerable<ToDo> doneToDoes = GetToDoes().Where(x => x.IsDone == true); ;
            foreach (var toDo in doneToDoes)
            {
                db.ToDos.Remove(toDo);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
