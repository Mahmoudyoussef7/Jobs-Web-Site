using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Job_offers.Models;
using Microsoft.AspNet.Identity;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        public ActionResult JobDetails(int jobId)
        {
            var job = db.Jobs.FirstOrDefault(d => d.ID == jobId);

            if (job==null)
            {
                return HttpNotFound();
            }
            TempData["jobId"] = jobId;
            TempData.Keep();
            return View(job);
        }

        [Authorize]
        public ActionResult Apply()
        {
            return View();
        }
        
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Apply(string Message)
        {
            ApplyForJob AppliedJob = new ApplyForJob();
            AppliedJob.UserId = User.Identity.GetUserId();
            AppliedJob.JobId = (int)TempData["jobId"];
            AppliedJob.Message = Message;
            AppliedJob.ApplyDate = DateTime.Now;
            var check = db.ApplyForJobs.Where(d => d.UserId == AppliedJob.UserId && d.JobId == AppliedJob.JobId).ToList();
            if(check.Count<1)
            {
                db.ApplyForJobs.Add(AppliedJob);
                db.SaveChanges();
                ViewBag.result = "لقد تم التقدم للوظيفة بنجاح";
                return View();
            }
            else
            {
                ViewBag.result = "عذراَ لقد تم التقدم من قبل لهذه الوظيفة";
                return View();
            }
            
        }

        [Authorize]
        public ActionResult GetJobsByUser()
        {
            string userId = User.Identity.GetUserId();
            List<ApplyForJob> jobs = db.ApplyForJobs.Where(d => d.UserId == userId).ToList();
            return View(jobs);
        }

        [Authorize]
        public ActionResult DetailsOfJob(int id)
        {
            var job = db.ApplyForJobs.FirstOrDefault(d => d.ID == id);

            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        [Authorize]
        public ActionResult EditAppliedJob(int id)
        {
            var job = db.ApplyForJobs.FirstOrDefault(d => d.ID == id);
            if(job==null)
            {
                return HttpNotFound();
            }
            return View(job);
        }
        
        [Authorize]
        [HttpPost]
        public ActionResult EditAppliedJob(ApplyForJob job)
        {
            if(ModelState.IsValid)
            {
                job.ApplyDate = DateTime.Now;
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GetJobsByUser");
            }
            return View(job);
            
        }

        [Authorize]
        public ActionResult DeleteAppliedJob(int id)
        {
            var job = db.ApplyForJobs.FirstOrDefault(d => d.ID == id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Roles/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult DeleteAppliedJob(ApplyForJob job)
        {
            // TODO: Add delete logic here
            if (ModelState.IsValid)
            {
                var model = db.ApplyForJobs.FirstOrDefault(d => d.ID == job.ID);
                db.ApplyForJobs.Remove(model);
                db.SaveChanges();
                return RedirectToAction("GetJobsByUser");
            }
            return View(job);
        }

        [Authorize]
        public ActionResult GetJobsByPublisher()
        {
            string userId = User.Identity.GetUserId();
            //var jobs = db.ApplyForJobs.Join(db.Jobs, a => a.JobId, b => b.ID, (a, b) =>
            //       new {
            //           JId = b.ID,
            //           UId = a.UserId,
            //           AJId = a.JobId
            //       }).Where(d => d.AJId == d.JId && d.UId == userId).ToList();
            var jobs = from app in db.ApplyForJobs
                       join job in db.Jobs
                       on app.JobId equals job.ID
                       where job.User.Id == userId
                       select app;
            var grouped = from j in jobs
                          group j by j.job.JobName
                          into gr
                          select new AppliedJobViewModel
                          {
                              JobName = gr.Key,
                              Items = gr
                          };
            return View(grouped.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = " هذا الموقع لإرسال واستقبال طلبات التوظيف بين الشركات و المتقدمين للوظائف ";
            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Contact(ContactModel contact)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(contact.Email);
            mail.To.Add(new MailAddress("mohammedazzam2233@gmail.com"));
            mail.Subject = contact.Subject;
            mail.IsBodyHtml = true;
            string body = " اسم المرسل " + contact.Name + " <br/> " + "بريد الالكترونى " + contact.Email + " <br/> " + " عنوان الرسالة " + contact.Subject + " <br/> "
                            + "نص الرسالـــــــة :  " + contact.Body + " <br> ";
            mail.Body = body;

            //Connection configration
            var NC = new NetworkCredential("mohammedazzam2233@gmail.com", "m2a0h9m1o9u9d7");
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = NC;
            smtpClient.Send(mail);
            return RedirectToAction("Index");
        }

        public ActionResult Search()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Search(string searchName)
        {
            var result = db.Jobs.Where(a => a.JobName.Contains(searchName)
            || a.JobDescription.Contains(searchName)
            || a.Category.CategoryName.Contains(searchName)
            || a.Category.CategoryDescription.Contains(searchName)).ToList();
            return View(result);
        }
    }
}