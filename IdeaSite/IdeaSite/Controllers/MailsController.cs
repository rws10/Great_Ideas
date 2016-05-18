﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdeaSite.Models;
using System.Net.Mail;
using System.Threading;
using System.DirectoryServices;
using System.Collections.Specialized;
using System.DirectoryServices.AccountManagement;

namespace IdeaSite.Controllers
{
    public class MailsController : Controller
    {
        private IdeaSiteContext db = new IdeaSiteContext();

        // GET: Mails/Create
        public ActionResult WriteNew()
        {
            // retrieve the current user's email
            string from = GetEmail();            
            
            // test for an '@freshfromflorida.com' email address. And stop the creation of the email and stop whatever action is occuring.
            if(from == null)
            {
                TempData["Message"] = "Your message has not been sent because you lack a valid email address." 
                    + "Please contact your supervisor about obtaining an \"@FreshFromFlorida.com\" email address.";

                return RedirectToAction("Index", "Ideas");
            }

            Mail mail = new Mail
            {
                Subject = "Great Ideas help request",
                From = from,
                To = "rws10@live.com"
            };

            return View(mail);
        }

        // POST: Mails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WriteNew([Bind(Include = "ID,From,To,Subject,Body")] Mail mail)
        {

            if (ModelState.IsValid)
            {
                MailMessage mailMsg = new MailMessage();
                mailMsg.Subject = mail.Subject;
                mailMsg.From = new MailAddress(mail.From);
                mailMsg.To.Add(new MailAddress(mail.To));
                mailMsg.Body = mail.Body;
                mailMsg.IsBodyHtml = true;

                SendEmail(mailMsg);

                TempData["Message"] = "Your message was submitted successfully.";
                return RedirectToAction("Index", "Ideas");
            }

            return View(mail);
        }

        public ActionResult AutoEmail()
        {
            MailMessage mailMsg = new MailMessage();
            List<string> emailInfo = TempData["EmailInfo"] as List<string>;
            string subject;
            string body;
            switch (Int32.Parse(emailInfo[0]))
            {
                // Compose an email to send to PPMO Group for Idea Creation
                case 1:
                    subject = string.Format("New Idea Submission: {0}", emailInfo[1]);

                    body = string.Format("{0} has submitted an Idea on Great Ideas:" +
                        "<br/><br/>{1}:" +
                        "<br/>{2}" +
                        "<br/><br/>Please go to <a href=\"http://localhost:52398/Ideas/Approval/{3}\">Great Ideas</a> to submit approval.",
                        emailInfo[3], emailInfo[1], emailInfo[2], Int32.Parse(emailInfo[4]));
                    TempData["Message"] = "Your idea has been successfully created.";
                    break;

                // Compose an email to send to PPMO Group for an Edited Idea
                case 2:
                    subject = string.Format("An idea has been edited: {0}", emailInfo[1]);

                    body = string.Format("{0} has Edited an Idea on Great Ideas:" +
                        "<br/><br/>{1}:" +
                        "<br/>{2}" +
                        "<br/><br/>Please go to <a href=\"http://localhost:52398/Ideas/Approval/{3}\">Great Ideas</a> to submit approval.",
                        emailInfo[3], emailInfo[1], emailInfo[2], int.Parse(emailInfo[4]));
                    TempData["Message"] = "Your idea has been successfully submitted.";
                    break;

                // Compose an email to send to user whose idea was accepted
                case 3:
                    subject = string.Format("New Idea Submission: {0}", emailInfo[1]);
                    body = string.Format(
                            "Your idea was accepted" +
                            "<br/><br/>{0}"
                            , emailInfo[2]);
                    TempData["Message"] = "Your acceptance was successful.";
                    break;

                // Compose an email to send to user whose idea was not accepted
                case 4:
                    subject = string.Format("New Idea Submission: {0}", emailInfo[1]);
                    body = string.Format(
                                                "Your idea was not accepted" +
                                                "<br/><br/>{0}" +
                                                "<br/><br/>Reason for Denial:" +
                                                "<br/>{1}" +
                                                "<br/><br/>If this is not rectified in 10 business days," +
                                                "the submission will be removed and no further action will be taken." +
                                                "<br/><br/>Please go to <a href=\"http://localhost:52398/Ideas/Edit/{2}\">Great Ideas</a> to resubmit your idea."
                                                , emailInfo[2], emailInfo[4], int.Parse(emailInfo[5]));
                    TempData["Message"] = "Your denial was successful.";
                    break;

                // Compose an email to send to the owner of an idea that was commented on
                default:
                    subject = string.Format("New comment added to your idea: {0}", emailInfo[1]);

                    body = string.Format("{0} has commented on your idea." +
                        "<br/><br/>To view this comment, go to <a href=\"http://localhost:52398/Comments/Index/{1}\">Great Ideas</a>.",
                        emailInfo[3], int.Parse(emailInfo[4]));
                    TempData["Message"] = "Your idea has been successfully submitted.";
                    break;
            }

            mailMsg.Subject = subject;
            mailMsg.Body = body;

            // retrieve the current user's email
            string from = GetEmail();

            // test for an '@freshfromflorida.com' email address. And stop the creation of the email and stop whatever action is occuring.
            if (from == null)
            {
                TempData["Message"] = "Your message has not been sent because you lack a valid email address."
                    + "Please contact your supervisor about obtaining a proper email address.";

                return RedirectToAction("Index", "Ideas");
            }

            mailMsg.From = new MailAddress(from);
            mailMsg.To.Add(new MailAddress("rws10@live.com"));

            mailMsg.IsBodyHtml = true;

            SendEmail(mailMsg);

            // redirect either to The comments index or the ideas index, based on where the email 
            if(TempData["Message"] as string == "Your email was not sent.")
            {
                return RedirectToAction("Index", "Ideas");
            }

            if (Int32.Parse(emailInfo[0]) == 5)
            {
                int ideaID = (int)TempData["IdeaID"];
                Idea idea = db.Ideas.Find(ideaID);
                return RedirectToAction("Index", "Comments", idea);
            }

            return RedirectToAction("Index", "Ideas");
        }


        public void SendEmail(Object mailMsg)
        {
            MailMessage mailMessage = (MailMessage)mailMsg;
            try
            {
                /* Setting should be kept somewhere so no need to 
                   pass as a parameter (might be in web.config)       */

                SmtpClient smtpClient = new SmtpClient("relay.DOACS.State.FL.US");
                //smtpClient.UseDefaultCredentials = true;
/*                NetworkCredential networkCredential = new NetworkCredential();
//               networkCredential.UserName = mailMessage.From.ToString();
//                networkCredential.Password = "T3@m_Z3d";
//              smtpClient.Credentials = networkCredential;
                if (!String.IsNullOrEmpty("587"))
                    smtpClient.Port = Convert.ToInt32("587");*/

                //If you are using gmail account then
                smtpClient.EnableSsl = false;

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtpClient.Send(mailMessage);
            }
            catch (SmtpException ex)
            {
                TempData["Message"] = "Your email was not sent.";
                // Code to Log error
            }
        }


        string GetEmail()
        {
            string username = Environment.UserName;
            string domain = Environment.UserDomainName;

            List<string> emailAddresses = new List<string>();

            PrincipalContext domainContext = new PrincipalContext(ContextType.Domain, domain);
            UserPrincipal user = UserPrincipal.FindByIdentity(domainContext, username);

            // Add the "mail" entry
            emailAddresses.Add(user.EmailAddress);

            // Add the "proxyaddresses" entries.
            System.DirectoryServices.PropertyCollection properties = ((DirectoryEntry)user.GetUnderlyingObject()).Properties;
            foreach (object property in properties["proxyaddresses"])
            {
                emailAddresses.Add(property.ToString());
            }

            string from = null;

            for (int i = 0; i < emailAddresses.Count; i++)
            {
                if (emailAddresses[i].Contains("@freshfromflorida.com"))
                {
                    from = emailAddresses[i];
                    break;
                }
            }

            return from;
        }
/*        public void SendEmailInBackgroundThread(MailMessage mailMessage)
        {
            Thread bgThread = new Thread(new ParameterizedThreadStart(SendEmail));
            bgThread.IsBackground = true;
            bgThread.Start(mailMessage);
        }*/

        /*internal static void SendEmail(MailAddress fromAddress, MailAddress toAddress, string subject, string body)
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
            // emails disabled because there is no error handling for a bad connection.
            smt.Send(msg);
        }*/

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
