using jQueryAjax.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jQueryAjax.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAllEmployee());
        }

        IEnumerable<Employee> GetAllEmployee()
        {
            using (jQueryAjaxDBEntities db = new jQueryAjaxDBEntities())
            {
                return db.Employees.ToList();
            }
        }

        public ActionResult AddOrEdit(int id = 0)
        {
            Employee emp = new Employee();
            if (id != 0)
            {
                using (jQueryAjaxDBEntities ent = new jQueryAjaxDBEntities())
                {
                    emp = ent.Employees.Where(x => x.Id == id).FirstOrDefault<Employee>();
                }
            }
            return View(emp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEdit(Employee emp)
        {
            try
            {
                if (emp.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(emp.ImageUpload.FileName);
                    string extension = Path.GetExtension(emp.ImageUpload.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    emp.ImagePath = "~/AppFiles/Images/" + fileName;
                    emp.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                }

                using (jQueryAjaxDBEntities ent = new jQueryAjaxDBEntities())
                {
                    if (emp.Id == 0)
                    {
                        ent.Employees.Add(emp);
                        ent.SaveChanges();
                    }
                    else
                    {
                        ent.Entry(emp).State = EntityState.Modified;
                        ent.SaveChanges();
                    }
                }
                return Json(new { success = true, html = ViewToStringConverter.RenderRazorViewToString(this, "ViewAll", GetAllEmployee()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                using (jQueryAjaxDBEntities ent = new jQueryAjaxDBEntities())
                {
                    Employee emp = ent.Employees.Where(x => x.Id == id).FirstOrDefault<Employee>();
                    ent.Employees.Remove(emp);
                    ent.SaveChanges();
                }
                return Json(new { success = true, html = ViewToStringConverter.RenderRazorViewToString(this, "ViewAll", GetAllEmployee()), message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}