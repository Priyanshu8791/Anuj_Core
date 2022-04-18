using Anuj_Core.DB_Context;
using Anuj_Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Anuj_Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Index(userlogin mod)
        {
            AnujContext ent = new AnujContext();
            var user = ent.Logins.Where(m => m.Email == mod.Email).FirstOrDefault();
            if (user.Email == null)
            {
                TempData["invalid"] = "Email.is not found invalid user ";
            }
            else
            {
                if (user.Email == mod.Email && user.Passward == mod.Passward)
                {


                    HttpContext.Session.SetString("name", user.Name);
                    HttpContext.Session.GetString("name");


                    return RedirectToAction("emp_list");

                }
                else
                {
                    TempData["not valid"] = "wrong password";
                }
            }



            return View();
        }
        public IActionResult emp_list()
        {
            List<Employee> set = new List<Employee>();
            AnujContext obj = new AnujContext();
            var res = obj.PersonDetails.ToList();

            foreach (var item in res)
            {
                set.Add(new Employee
                {
                    Id = item.Id,
                    Name = item.Name,
                    Email = item.Email,              
                    City = item.City,
                    
                });
            }
            return View(set);

        }
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registration(Employee rock)
        {
            AnujContext obj = new AnujContext();
            PersonDetail set = new PersonDetail();
            set.Id = rock.Id;
            set.Name = rock.Name;
            set.Email = rock.Email;                      
            set.City = rock.City;
           
            if (rock.Id == 0)
            {
                obj.PersonDetails.Add(set);
                obj.SaveChanges();
                return RedirectToAction("emp_list");
            }
            else
            {
                obj.Entry(set).State = EntityState.Modified;
                obj.SaveChanges();
                return RedirectToAction("emp_list");

            }



        }
        public IActionResult Edit(int id)
        {
            AnujContext obj = new AnujContext();
            var edit = obj.PersonDetails.Where(m => m.Id == id).First();
            Employee set = new Employee();
            set.Id = edit.Id;
            set.Name = edit.Name;
            set.Email = edit.Email;      
            set.City = edit.City;          
            return View("Registration", set);
        }
        public IActionResult delete(int id)
        {
            AnujContext obj = new AnujContext();
            var dlt = obj.PersonDetails.Where(m => m.Id == id).First();
            obj.PersonDetails.Remove(dlt);
            obj.SaveChanges();
            return RedirectToAction("emp_list");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View("index");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
