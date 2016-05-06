﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdeaSite.Models;
using System.Configuration;
using System.IO;
using System.Net.Mail;

namespace IdeaSite.Controllers
{
    public class IdeasController : Controller
    {
        private IdeaSiteContext db = new IdeaSiteContext();

        internal static void SendEmail(MailAddress fromAddress, MailAddress toAddress, string subject, string body)
        {
            MailMessage msg = new MailMessage();
            msg.From = fromAddress;
            msg.To.Add(toAddress);
            msg.Body = body;
            msg.IsBodyHtml = true;
            msg.Subject = subject;
            SmtpClient smt = new SmtpClient("smtp-mail.outlook.com ");
            smt.Port = 587;
            smt.Credentials = new NetworkCredential("teamzed@outlook.com", "T3@m_Z3d");
            smt.EnableSsl = true;
            //smt.Send(msg);
        }

        //home index
        public ActionResult Home()
        {
            return View();
        }

        public IEnumerable<Idea> SearchByTerms(string searchBy, string search)
        {
            IEnumerable<Idea> results = new List<Idea>();
            string[] searchTerms;
            searchTerms = search.Trim().Split(' ');
            for (int i = 0; i < searchTerms.Length; ++i)
            {
                var term = searchTerms[i];
                if (searchBy == "Title") { results = results.Concat(db.Ideas.Where(x => x.title.Contains(term)).ToList()); }
                else if (searchBy == "Description") { results = results.Concat(db.Ideas.Where(x => x.body.Contains(term)).ToList()); }
                else if (searchBy == "All")
                {
                    results = results.Concat(db.Ideas.Where(x => x.title.Contains(term)).ToList());
                    results = results.Concat(db.Ideas.Where(x => x.body.Contains(term)).ToList());
                }
            }

            var ideas = db.Ideas.ToList();
            List<int[]> matches = new List<int[]>();
            foreach (Idea idea in results)
            {
                foreach (var match in matches)
                {
                    if (idea.ID == match[0]) { ++match[1]; }
                }
            }
            // I think these are the same

            foreach (Idea idea in ideas)
            {
                int[] match = new int[2];
                match[0] = idea.ID;
                match[1] = 0;
                matches.Add(match);
            }
            matches.OrderBy(x => x[1]);
            matches = matches.OrderBy(x => x[1]).ToList();
            matches.Reverse();
            results.Distinct();
            IEnumerable<Idea> finalResults = new List<Idea>();
            for (int i = 0; i < matches.Count(); ++i)
            {
                foreach (var idea in results)
                {
                    if (idea.ID == matches[i][0])
                    {
                        finalResults = finalResults.Concat(db.Ideas.Where(x => x.ID == idea.ID).ToList());
                    }
                }
            }
            finalResults = finalResults.Distinct();
            return finalResults;
        }

        /*
        public IEnumerable<Idea> MatchSearchResults()
        {

        }
        */


        // GET: Ideas
        public ActionResult Index(string searchBy, string search, string sortByStatus)
        {
            IEnumerable<Idea> results;
            if (search != null) { results = SearchByTerms(searchBy, search); }
            else { results = db.Ideas.ToList(); }
            if (sortByStatus != "All") { results = results.Where(x => x.statusCode == sortByStatus); }
            results = results.Reverse();
            return View(results);
        }

        // GET: Ideas/Details/
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Idea idea = db.Ideas.Find(id);

            if (idea == null)
            {
                return HttpNotFound();
            }

            return View(idea);
        }

        // GET: Ideas/Create

        public ActionResult Create()
        {
            Idea tempIdea = TempData["Idea"] as Idea;

            return View(tempIdea);
        }

        // POST: Ideas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,title,body,cre_date,cre_user,statusCode,denialReason")] Idea idea, IEnumerable<HttpPostedFileBase> attachments)
        {
            if (ModelState.IsValid)
            {

                idea.cre_user = "Administrator";
                idea.cre_date = DateTime.Now;
                db.Ideas.Add(idea);

                try {
                    db.SaveChanges();
                }

                catch
                {
                    TempData["Idea"] = idea;
                    TempData["Message"] = "Title must be a unique value";
                    return View(idea);
                }

                var appSettings = ConfigurationManager.AppSettings;

                // store path to server location of the attachment storage
                var connectionInfo = appSettings["serverPath"];

                // combine the server location and the name of the new folder to be created
                var storagePath = string.Format(@"{0}{1}_{2}", connectionInfo, idea.ID, idea.title);

                DirectoryInfo di = Directory.CreateDirectory(storagePath);
                if (attachments != null)
                {
                    try
                    {
                        // loop through the uploads and pull out each attachment from it.
                        for (int i = 0; i < attachments.Count(); ++i)
                        {
                            if (attachments.ElementAt(i) != null && attachments.ElementAt(i).ContentLength > 0)
                            {
                                // store the name of the attachment
                                var attachmentName = Path.GetFileName(attachments.ElementAt(i).FileName);

                                // create new object to reference the loaction of the new attachment and the ID of the idea to which it belongs.
                                var attachment = new Models.Attachment
                                {
                                    storageLocation = string.Format("{0}\\", storagePath),
                                    name = attachmentName,
                                    cre_date = DateTime.Now,
                                    ideaID = idea.ID,
                                    delete = false
                                };

                                attachments.ElementAt(i).SaveAs(string.Format("{0}\\{1}", storagePath, attachmentName));

                                db.Attachments.Add(attachment);
                                db.SaveChanges();
                            }
                        }
                    }

                    catch
                    {
                        TempData["Idea"] = idea;
                        TempData["Message"] = "One or more attachments failed to upload";
                        return RedirectToAction("Create");
                    }
                }

                // Compose an email to send to PPMO Group
                string subject = string.Format("New Idea Submission: {0}", idea.title);

                string body = string.Format("{0} has submitted an Idea on Great Ideas:" +
                    "<br/><br/>{1}:" +
                    "<br/>{2}" +
                    "<br/><br/>Please go to <a href=\"http://localhost:52398/Ideas/Approval/{2}\">Great Ideas</a> to submit approval.",
                    idea.cre_user, idea.title, idea.body);

                MailAddress from = new MailAddress("teamzed@outlook.com");
                MailAddress to = new MailAddress("rws10@live.com");


                SendEmail(from, to, subject, body);

                TempData["Message"] = "Your idea has been successfully created.";
                return RedirectToAction("Index");
            }

            return View(idea);
        }

        // GET: Ideas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Idea idea = db.Ideas.Find(id);

            idea.statusCode = "Submitted";

            /*idea.statusCodes = new[]
            {
                new SelectListItem { Value = "Added", Text = "Added" },
                new SelectListItem { Value = "Archived", Text = "Archived" },
                new SelectListItem { Value = "Project Submission", Text = "Project Submission" },
            };*/

            if (idea == null)
            {
                return HttpNotFound();
            }

            /*IEnumerable<Models.Attachment> attachments = new List<Models.Attachment>();
            attachments = db.Attachments.Where(attachment => attachment.ideaID == idea.ID).ToList();

            ViewBag.attachments = attachments;
            
            var appSettings = ConfigurationManager.AppSettings;

            // store path to server location of the attachment storage
            var connectionInfo = appSettings["serverPath"];

            // combine the server location and the name of the new folder to be created
            var ideaFolder = string.Format(@"{0}{1}_{2}", connectionInfo, idea.ID, idea.title);
            DirectoryInfo dir = new DirectoryInfo(ideaFolder);

            // Store the attachments from the desired attachment folder
            if (dir.Exists)
            {
                var attachments = dir.GetFiles();
                ViewBag.attachments = attachments;
                ViewBag.path = ideaFolder;
            }
            else
            {
                ViewBag.attachments = null;
                ViewBag.path = null;
            }
            */

            return View(idea);
        }

        // POST: Ideas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,title,body,cre_date,cre_user,statusCode,denialReason")] Idea idea, IEnumerable<HttpPostedFileBase> attachments)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(idea).State = EntityState.Modified;
                var currentIdea = db.Ideas.FirstOrDefault(p => p.ID == idea.ID);
                if (currentIdea == null)
                {
                    return HttpNotFound();
                }

                currentIdea.title = idea.title;
                currentIdea.body = idea.body;
                currentIdea.statusCode = idea.statusCode;
                currentIdea.statusCodes = idea.statusCodes;
                currentIdea.denialReason = idea.denialReason;

                try
                {
                    db.SaveChanges();
                }

                catch
                {
                    TempData["Idea"] = idea;
                    TempData["Message"] = "Title must be a unique value";
                    return View(idea);
                }

                string subject = string.Format("An idea has been edited: {0}", idea.title);

                string body = string.Format("{0} has Edited an Idea on Great Ideas:" +
                    "<br/><br/>{1}:" +
                    "<br/>{2}" +
                    "<br/><br/>Please go to <a href=\"http://localhost:52398/Ideas/Approval/{3}\">Great Ideas</a> to submit approval.",
                    idea.cre_user, idea.title, idea.body, idea.ID);

                MailAddress from = new MailAddress("teamzed@outlook.com");
                MailAddress to = new MailAddress("rws10@live.com");


                SendEmail(from, to, subject, body);

                var appSettings = ConfigurationManager.AppSettings;

                // store path to server location of the attachment storage
                var connectionInfo = appSettings["serverPath"];

                // combine the server location and the name of the new folder to be created
                var storagePath = string.Format(@"{0}{1}_{2}", connectionInfo, idea.ID, idea.title);

                if (!Directory.Exists(storagePath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(storagePath);
                }
                try
                {
                    // loop through the uploads and pull out each attachment from it.
                    for (int i = 0; i < attachments.Count(); ++i)
                    {
                        if (attachments.ElementAt(i) != null && attachments.ElementAt(i).ContentLength > 0)
                        {
                            // store the name of the attachment
                            var attachmentName = Path.GetFileName(attachments.ElementAt(i).FileName);

                            // create new object to reference the loaction of the new attachment and the ID of the idea to which it belongs.
                            var attachment = new Models.Attachment
                            {
                                storageLocation = string.Format("{0}\\", storagePath),
                                name = attachmentName,
                                cre_date = DateTime.Now,
                                ideaID = idea.ID,
                                delete = false
                            };

                            attachments.ElementAt(i).SaveAs(string.Format("{0}\\{1}", storagePath, attachmentName));

                            db.Attachments.Add(attachment);
                            db.SaveChanges();
                        }
                    }
                }

                catch
                {
                    TempData["Message"] = "The attachments failed to upload";
                    return RedirectToAction("Edit");
                }

                TempData["Message"] = "Your idea has been successfully created.";
                return RedirectToAction("Index");
            }
            //ViewBag.attachments = attachments;
            return View(idea);
        }

        // GET: Ideas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Idea idea = db.Ideas.Find(id);
            if (idea == null)
            {
                return HttpNotFound();
            }
            return View(idea);
        }

        // POST: Ideas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Idea idea = db.Ideas.Find(id);

            /* Create a list of the attachments associated with the idea to allow the deletion of the files 
             * associated with them. Once all of the files are gone from each files directory, the directory is 
             * deleted*/
            IEnumerable<Models.Attachment> attachments = new List<Models.Attachment>();
            attachments = db.Attachments.Where(attach => attach.ideaID == idea.ID).ToList();

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    attachment.DeleteFile();
                    attachment.DeleteDirectory();
                }

                db.Attachments.RemoveRange(db.Attachments.Where(attach => attach.ideaID == id));
            }

            db.Comments.RemoveRange(db.Comments.Where(com => com.ideaID == id));

            db.Ideas.Remove(idea);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Ideas/Approve/5
        public ActionResult Approval(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Idea idea = db.Ideas.Find(id);

            if (idea == null)
            {
                return HttpNotFound();
            }

            return View(idea);
        }

        // POST: Ideas/Approve/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approval([Bind(Include = "ID,title,body,cre_date,cre_user,statusCode,denialReason")] Idea idea)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(idea).State = EntityState.Modified;
                var currentIdea = db.Ideas.FirstOrDefault(p => p.ID == idea.ID);
                if (currentIdea == null)
                {
                    return HttpNotFound();
                }

                currentIdea.statusCode = idea.statusCode;
                currentIdea.denialReason = idea.denialReason;

                db.SaveChanges();

                var appSettings = ConfigurationManager.AppSettings;

                // store path to server location of the attachment storage
                var connectionInfo = appSettings["serverPath"];

                // combine the server location and the name of the new folder to be created
                var ideaFolder = string.Format(@"{0}{1}_{2}", connectionInfo, idea.ID, idea.title);
                DirectoryInfo dir = new DirectoryInfo(ideaFolder);

                // Store the attachments from the desired attachment folder
                var attachments = dir.GetFiles();

                ViewBag.path = ideaFolder;
                ViewBag.attachments = attachments;

                string body;
                if (idea.statusCode == "Added")
                {
                    body = string.Format(
                        "Your idea was added" +
                        "<br/><br/>{0}"
                        , idea.body);
                }
                else
                {
                    body = string.Format(
                        "Your idea wsa not added" +
                        "<br/><br/>{0}" +
                        "<br/><br/>Reason for Denial:" +
                        "<br/>{1}" +
                        "<br/><br/>If this is not rectified in 10 business days," +
                        "the submission will be removed and no further action will be taken." +
                        "<br/><br/>Please go to <a href=\"http://localhost:52398/Ideas/Edit/{2}\">Great Ideas</a> to resubmit your idea."
                        , idea.body, idea.denialReason, idea.ID);
                }

                string subject = string.Format("New Idea Submission: {0}", idea.title);
                MailAddress from = new MailAddress("teamzed@outlook.com");
                MailAddress to = new MailAddress("rws10@live.com");


                SendEmail(from, to, subject, body);
                return RedirectToAction("Index");
            }

            return View(idea);
        }

        public FileResult Download(string attachmentpath, string attachmentName)
        {
            return File(attachmentpath, System.Net.Mime.MediaTypeNames.Application.Octet, attachmentName);
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