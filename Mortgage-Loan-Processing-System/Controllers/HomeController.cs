using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mortgage_Loan_Processing_System.Controllers
{
    public class HomeController : Controller
    {
        mlpsEntities mlps = new mlpsEntities();
        

        public ActionResult Index()
        {            
            return View();
        }
        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            Customer customer = mlps.Customers.Find(username);
            if (customer!=null)
            {
                if (customer.password == password)
                {
                    Response.Write("<script>alert('Welcome Customer')</script>");
                    return RedirectToAction("userDashboard", new { id = username });
                }
                else
                {
                    Response.Write("<script>alert('Invalid Username or Password.')</script>");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                Employee employee = mlps.Employees.Find(username);
                if (employee != null)
                {
                    if (employee.password == password)
                    {
                        Response.Write("<script>alert('Welcome Employee')</script>");
                        return RedirectToAction("Enquiry");
                    }
                    else
                    {
                        Response.Write("<script>alert('Invalid Username or Password.')</script>");
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    Response.Write("<script>alert('Invalid Username or Password.')</script>");
                    return RedirectToAction("Index");
                }
            }
           
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Enquiry(string id)
        {
            ViewBag.username = id;            
            return View();
        }

        public ActionResult emiCal()
        {
            return View();
        }

        public ActionResult userReg()
        {
            return View();
        }

        [HttpPost]
        public ActionResult userRegistration(string username,string password, string name, string email, string contact, DateTime dob, string gender, string address)
        {
            Customer newCustomer = new Customer();
            newCustomer.username = username;
            newCustomer.password = password;
            newCustomer.name = name;
            newCustomer.email = email;
            newCustomer.contactNumber = contact;
            newCustomer.dob = dob.Date.ToString();
            newCustomer.gender = gender;
            newCustomer.address = address;
            
            Customer nCust= mlps.Customers.Add(newCustomer);
            mlps.SaveChanges();
            if (nCust == null)
            {
                return RedirectToAction("userReg");            
            }
            else
            {
                return RedirectToAction("userDashboard", new { id = nCust.username});
            }            
        }


        public ActionResult applicationForm(string id)
        {
            if (id=="guest")
            {
                return RedirectToAction("userReg");
            }
            else
            {
                ViewBag.Message = id;
                return View();
            }
            
        }

        public ActionResult LoanApplication(string type,string value, string amount, int tenure, string aadhar, string pan, string account,string link, string id)
        {
            DateTime date = DateTime.Now;
            Application newApplication = new Application();
            newApplication.PropertyType = type;
            newApplication.PropertyValue = value;
            newApplication.Expected_Loan_Amount = amount;
            newApplication.LoanTenure = tenure;
            newApplication.DateOfApplication = date.Date.ToString();
            newApplication.Aadhar = aadhar;
            newApplication.Pan = pan;
            newApplication.AccountNo = account;
            newApplication.Status = "Applied";
            newApplication.OtherDocuments = link;
            newApplication.username = id;

            mlps.Applications.Add(newApplication);
            mlps.SaveChanges();
            ViewBag.Message = id;
            return RedirectToAction("userDashboard",new { id=id});
        }

        public ActionResult userDashboard(string id )
        {
            Customer customer = mlps.Customers.Find(id);
            ViewBag.username = id;
            ViewBag.Message = customer.name;

            List<Application> userApplications = new List<Application>();
            var list = mlps.Applications.ToList();
            foreach (var item in list)
            {
                if (item.username == customer.username)
                {
                    userApplications.Add(item);
                }
            }

            return View(userApplications);
        }

        
    }
}