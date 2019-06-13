using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BluedeskUpload.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BluedeskUpload.Controllers
{
    public class UploadController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Customer")]
        // GET: Upload
        public ActionResult Index(string sortOrder, string searchString, HttpPostedFileBase postedFile)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "omschrijving_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var uploads = from u in db.Uploads
                          select u;

            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.SearchString = searchString;
                uploads = uploads.Where(s => s.Bestand.Contains(searchString) || s.Omschrijving.Contains(searchString) || s.Bedrijfsnaam.Contains(searchString) || s.Naam.Contains(searchString) || s.Email.Contains(searchString) || s.Telefoon.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "omschrijving_desc":
                    uploads = uploads.OrderByDescending(u => u.Bestand);
                    break;
                case "Date":
                    uploads = uploads.OrderBy(u => u.Datum);
                    break;
                case "date_desc":
                    uploads = uploads.OrderByDescending(u => u.Datum);
                    break;
                default:
                    uploads = uploads.OrderBy(u => u.Bestand);
                    break;
            }

            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
                ViewBag.Message = "File uploaded successfully.";
            }

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            return View(uploads.Where(c => c.Gebruiker.Id == currentUser.Id).ToList());
        }

        // GET: Back to index
        public ActionResult BackToIndex(int? id, Upload upload)
        {
            return RedirectToAction("Index", "Upload");
        }

        // GET: Upload/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Upload upload = db.Uploads.Find(id);
            if (upload == null)
            {
                return HttpNotFound();
            }
            return View(upload);
        }

        // GET: Upload
        public ActionResult Download(int? id)
        {
            Upload upload = db.Uploads.Find(id);
            var filename = Encryption.Encrypt(upload.Bestand, 13);
            string fullPath = Server.MapPath("~/Uploads/" + filename);

            // Get the contentType of file
            var mimeType = MimeMapping.GetMimeMapping(upload.Bestand);

            // Return the file for download
            return File(fullPath, mimeType, upload.Bestand);
        }

        [Authorize(Roles = "Customer, Admin")]
        // GET: Upload/Create
        public ActionResult Create()
        {
            var upload = new UploadView();
            upload.Gebruikers = LoadUsers();

            return View(upload);
        }

        private IEnumerable<SelectListItem> LoadUsers()
        {
            var list = new List<SelectListItem>();
            foreach (var user in db.Users.Where(u => u.UserRole == "Customer"))
            {
                list.Add(new SelectListItem() { Text = user.UserName, Value = user.UserName });
            }

            return list;
        }

        [Authorize(Roles = "Customer, Admin")]
        // POST: Upload/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UploadId,Datum,Bestand,Omschrijving,Bedrijfsnaam,Naam,Email,Telefoon")] Upload upload, HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var filename = Encryption.Encrypt(postedFile.FileName, 13);
                postedFile.SaveAs(path + Path.GetFileName(filename));
                ViewBag.Message = "File uploaded successfully.";
            }

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            upload.Gebruiker = currentUser;
            upload.Bestand = postedFile.FileName;

            if (ModelState.IsValid)
            {
                db.Uploads.Add(upload);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(upload);
        }

        // GET: Upload/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Upload upload = db.Uploads.Find(id);
            if (upload == null)
            {
                return HttpNotFound();
            }
            return View(upload);
        }

        // POST: Upload/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UploadId,Datum,Bestand,Omschrijving,Bedrijfsnaam,Naam,Email,Telefoon")] Upload upload)
        {
            if (ModelState.IsValid)
            {
                db.Entry(upload).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(upload);
        }

        // GET: Upload/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Upload upload = db.Uploads.Find(id);
            if (upload == null)
            {
                return HttpNotFound();
            }
            return View(upload);
        }

        // POST: Upload/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Upload upload = db.Uploads.Find(id);
            var filename = Encryption.Encrypt(upload.Bestand, 13);
            string fullPath = Server.MapPath("~/Uploads/" + filename);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            db.Uploads.Remove(upload);
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