using Mortgage_Loan_Processing_System.Models;
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
            if (customer != null)
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
                        if (employee.Role == "Loan Officer")
                        {
                            return RedirectToAction("LoanOfficer", new { id = employee.username });
                        }
                        else if (employee.Role == "Loan Investigator")
                        {
                            return RedirectToAction("LoanInvestigator", new { id = employee.username });
                        }
                        else
                        {
                            return RedirectToAction("LoanAuthorizer", new { id = employee.username });
                        }
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
        public ActionResult userRegistration(string username, string password, string name, string email, string contact, DateTime dob, string gender, string address)
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

            Customer nCust = mlps.Customers.Add(newCustomer);
            mlps.SaveChanges();
            if (nCust == null)
            {
                return RedirectToAction("userReg");
            }
            else
            {
                return RedirectToAction("userDashboard", new { id = nCust.username });
            }
        }


        public ActionResult applicationForm(string id)
        {
            if (id == "guest")
            {
                return RedirectToAction("userReg");
            }
            else
            {
                ViewBag.Message = id;
                return View();
            }

        }

        public ActionResult LoanApplication(string type, string value, string amount, int tenure, string aadhar, string pan, string account, string link, string id)
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
            return RedirectToAction("userDashboard", new { id = id });
        }

        public ActionResult userDashboard(string id)
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

        public ActionResult viewApplication(string id)
        {
            Application application = mlps.Applications.Find(int.Parse(id));
            ViewBag.username = application.username;
            return View(application);
        }
        public ActionResult viewApplicant(string id)
        {
            Customer customer = mlps.Customers.Find(int.Parse(id));
            return View(customer);
        }

        public ActionResult LoanOfficer(string id)
        {
            Employee employee = mlps.Employees.Find(id);
            ViewBag.name = employee.name;
            ViewBag.id = employee.username;
            MyViewModel viewModel = new MyViewModel();

            var list = mlps.Applications.ToList();
            foreach (var item in list)
            {
                if (item.Status == "Applied")
                {
                    viewModel.applied.Add(item);
                }
                else if (item.Status == "Investigated")
                {
                    viewModel.investigated.Add(item);
                }
            }

            return View(viewModel);
        }
        public ActionResult verify(string id)
        {
            string[] arr = id.Split(separator: ',');
            Application application = mlps.Applications.Find(int.Parse(arr[0]));

            application.Status = "Under Investigation";
            mlps.SaveChanges();
            return RedirectToAction("LoanOfficer", new { id = arr[1] });
        }
        public ActionResult Reject(string id)
        {
            string[] arr = id.Split(separator: ',');
            Application application = mlps.Applications.Find(int.Parse(arr[0]));

            application.Status = "Rejected";
            mlps.SaveChanges();
            return RedirectToAction("LoanOfficer", new { id = arr[1] });
        }
        public ActionResult Approve(string id)
        {
            string[] arr = id.Split(separator: ',');
            Application application = mlps.Applications.Find(int.Parse(arr[0]));

            application.Status = "Approved";
            mlps.SaveChanges();
            return RedirectToAction("LoanOfficer", new { id = arr[1] });
        }

        public ActionResult LoanInvestigator(string id)
        {
            Employee employee = mlps.Employees.Find(id);
            ViewBag.name = employee.name;
            ViewBag.id = employee.username;

            List<Application> applications = new List<Application>();

            var list = mlps.Applications.ToList();

            foreach (var item in list)
            {
                if (item.Status == "Under Investigation")
                {
                    applications.Add(item);
                }
            }
            return View(applications);
        }
        public ActionResult LoanAuthorizer(string id)
        {
            Employee employee = mlps.Employees.Find(id);
            ViewBag.name = employee.name;
            ViewBag.id = employee.username;

            List<Application> applications = new List<Application>();

            var list = mlps.Applications.ToList();

            foreach (var item in list)
            {
                if (item.Status == "Approved")
                {
                    applications.Add(item);
                }
            }
            return View(applications);
        }

        public ActionResult Report(string id,string report)
        {
            string[] arr = id.Split(separator: ',');
            Application application = mlps.Applications.Find(int.Parse(arr[0]));
            application.Report = report;
            application.Status = "Investigated";
            mlps.SaveChanges();
            return RedirectToAction("LoanInvestigator", new { id = arr[1] });
        }

        public ActionResult sanctioned(string id)
        {
            string[] arr = id.Split(separator: ',');
            Application application = mlps.Applications.Find(int.Parse(arr[0]));
            
            application.Status = "Sanctioned";
            mlps.SaveChanges();
            return RedirectToAction("LoanAuthorizer", new { id = arr[1] });
        }
    }
}