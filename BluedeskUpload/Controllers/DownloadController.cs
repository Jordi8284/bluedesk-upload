using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BluedeskUpload.Models;

namespace BluedeskUpload.Controllers
{
    public class DownloadController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Download
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "bedrijfsnaam_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "naam_desc" : "Naam";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "email_desc" : "Email";
            var uploads = from u in db.Uploads
                          select u;

            if (!String.IsNullOrEmpty(searchString))
            {
                uploads = uploads.Where(s => s.Omschrijving.Contains(searchString));
                uploads = uploads.Where(s => s.Bedrijfsnaam.Contains(searchString));
                uploads = uploads.Where(s => s.Naam.Contains(searchString));
                uploads = uploads.Where(s => s.Email.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "bedrijfsnaam_desc":
                    uploads = uploads.OrderByDescending(u => u.Bedrijfsnaam);
                    break;
                case "Date":
                    uploads = uploads.OrderBy(u => u.Datum);
                    break;
                case "date_desc":
                    uploads = uploads.OrderByDescending(u => u.Datum);
                    break;
                case "naam_desc":
                    uploads = uploads.OrderByDescending(u => u.Naam);
                    break;
                case "email_desc":
                    uploads = uploads.OrderByDescending(u => u.Email);
                    break;
                default:
                    uploads = uploads.OrderBy(u => u.Bedrijfsnaam);
                    break;
            }

            var downloads = db.Downloads.Include(d => d.Upload);
            return View(downloads.ToList());
        }

        // GET: Download/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Download download = db.Downloads.Find(id);
            if (download == null)
            {
                return HttpNotFound();
            }
            return View(download);
        }

        // GET: Download/Create
        public ActionResult Create()
        {
            ViewBag.UploadId = new SelectList(db.Uploads, "UploadId", "Bestand");
            return View();
        }

        // POST: Download/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DownloadId,UploadId")] Download download)
        {
            if (ModelState.IsValid)
            {
                db.Downloads.Add(download);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UploadId = new SelectList(db.Uploads, "UploadId", "Bestand", download.UploadId);
            return View(download);
        }

        // GET: Download/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Download download = db.Downloads.Find(id);
            if (download == null)
            {
                return HttpNotFound();
            }
            ViewBag.UploadId = new SelectList(db.Uploads, "UploadId", "Bestand", download.UploadId);
            return View(download);
        }

        // POST: Download/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DownloadId,UploadId")] Download download)
        {
            if (ModelState.IsValid)
            {
                db.Entry(download).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UploadId = new SelectList(db.Uploads, "UploadId", "Bestand", download.UploadId);
            return View(download);
        }

        // GET: Download/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Download download = db.Downloads.Find(id);
            if (download == null)
            {
                return HttpNotFound();
            }
            return View(download);
        }

        // POST: Download/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Download download = db.Downloads.Find(id);
            db.Downloads.Remove(download);
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
