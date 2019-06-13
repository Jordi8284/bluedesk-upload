using System;
using System.Data;
using System.Data.Entity;
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
    [Authorize(Roles="Admin")]
    public class DownloadController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Download
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "omschrijving_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var downloads = db.Downloads.Include(g => g.Upload);

            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.SearchString = searchString;
                downloads = downloads.Where(s => s.Upload.Bestand.Contains(searchString) || s.Upload.Omschrijving.Contains(searchString) || s.Upload.Bedrijfsnaam.Contains(searchString) || s.Upload.Naam.Contains(searchString) || s.Upload.Email.Contains(searchString) || s.Upload.Telefoon.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "omschrijving_desc":
                    downloads = downloads.OrderByDescending(u => u.Upload.Bestand);
                    break;
                case "Date":
                    downloads = downloads.OrderBy(u => u.Upload.Datum);
                    break;
                case "date_desc":
                    downloads = downloads.OrderByDescending(u => u.Upload.Datum);
                    break;
                default:
                    downloads = downloads.OrderBy(u => u.Upload.Bestand);
                    break;
            }

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            return View(downloads.Where(c => c.Gebruiker.Id == currentUser.Id).ToList());
        }

        // GET: Back to index
        public ActionResult BackToIndex(int? id, Download download)
        {
            return RedirectToAction("Index", "Download");
        }

        // GET: Download/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Download download = db.Downloads.Where(g => g.DownloadId == id).Include(g => g.Upload).FirstOrDefault();
            if (download == null)
            {
                return HttpNotFound();
            }
            return View(download);
        }

        // GET: Upload
        public ActionResult Download(int? id)
        {
            Download download = db.Downloads.Find(id);
            Upload upload = db.Uploads.Find(download.UploadId);
            var filename = Encryption.Encrypt(upload.Bestand, 13);
            // var filename = Convert.ToBase64String(Encoding.UTF8.GetBytes(Encryption.EncryptDecrypt(upload.Bestand, 13)));
            string fullPath = Server.MapPath("~/Uploads/" + filename);

            // Get the contentType of file
            var mimeType = MimeMapping.GetMimeMapping(download.Upload.Bestand);

            // Return the file for download
            return File(fullPath, mimeType, download.Upload.Bestand);
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
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            download.Gebruiker = currentUser;

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
            Download download = db.Downloads.Where(g => g.DownloadId == id).Include(g => g.Upload).FirstOrDefault();
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