using Microsoft.Ajax.Utilities;
using MovieApplication_Midterm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace MovieApplication_Midterm.Controllers
{
    public class HomeController : Controller
    {

        private MovieDbEntities _db = new MovieDbEntities();
        private MoviesDataEntities2 _cdb = new MoviesDataEntities2();
        CartsDatabaseEntities contextobjs = new CartsDatabaseEntities();
        LoginEntities2 contextO = new LoginEntities2();
        // GET: Home
        List<MovieCategory> cartlistss = new List<MovieCategory>();

        public ActionResult login()
        {
            login objs = new login();
            //var logindatabase = Ldb.logins.ToList();

            return View();
        }
        [HttpPost]
        public ActionResult login(string User_Name, string Password, MovieApplication_Midterm.Models.login usermodel)
        {
           
            var LoginRecord = (from item in contextO.logins
                               where item.User_Name == User_Name && item.Password == Password
                               select item).FirstOrDefault();
            if (LoginRecord == null)
            {
                usermodel.loginErrorMessage = "You Have enter Incorrect Username and Password!!";
                return View("login", usermodel);
            }
            else
            {
                return RedirectToAction("Index");
            }
           
            
        }
        public ActionResult Registration()
        {
            return View();
        }

            [HttpPost]
        public ActionResult Registration(login lobj)
        {
            contextO.logins.Add(new login() { User_Name = lobj.User_Name, Password = lobj.Password });
            contextO.SaveChanges();
            return RedirectToAction("login");
        }
        public ActionResult Index()
        {
            return View(_db.Movies);
        }

        // GET: Home/Details/5
     

    
      

        public ActionResult MoviesCategory(string Category)
        {
            List<MovieCategory> data = _cdb.MovieCategories.ToList();
            List<MovieCategory> arlist = new List<MovieCategory>();
            //ArrayList arlist = new ArrayList();
            // foreach(var categorydata in data)
            for (int i=0; i < data.Count; i++)
            {
                if (data[i].Category == Category)
                {

                    arlist.Add(data[i]);
                    
                }
            }
            //Console.WriteLine(arlist);
            return View(arlist.AsEnumerable());

            //return View(_cdb.MovieCategories); 
            //return Content("Id" + Category);
        }
       
        [HttpPost]
        public ActionResult AddtoCart(CartsDatabase obj)
        {
            //updateCart obj = new updateCart();
            List<MovieCategory> data = _cdb.MovieCategories.ToList();
            List<MovieCategory> cartlist = new List<MovieCategory>();
            //ArrayList arlist = new ArrayList();
            // foreach(var categorydata in data)
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Name == obj.Name)
                {

                    cartlist.Add(data[i]);

                }
            }

            // Console.WriteLine(cartlist);
            
            contextobjs.CartsDatabases.Add(new CartsDatabase() { MovieId = cartlist[0].Id, Name = cartlist[0].Name, Gross = cartlist[0].Gross });
            contextobjs.SaveChanges();
            
            int totalGross = 0;

            for (int j = 0; j < contextobjs.CartsDatabases.ToList().Count; j++)
            {
                totalGross = totalGross +  Int32.Parse(contextobjs.CartsDatabases.ToList()[j].Gross);
            }
            contextobjs.CartsDatabases.Add(new CartsDatabase() { MovieId = cartlist[0].Id, Name = cartlist[0].Name, Gross = cartlist[0].Gross });
            var datasets = contextobjs.CartsDatabases.ToList();
            ViewBag.Message = totalGross;

            return View("AddtoCart", datasets);


        }
       

        // POST: Home/Edit/5
       
       
       
        // GET: Home/Delete/5
        public ActionResult DeleteEntry(string Name)
        {
            var MovieRecord = (from item in contextobjs.CartsDatabases
                               where item.Name == Name
                               select item).First();

            contextobjs.CartsDatabases.Remove(MovieRecord);
            contextobjs.SaveChanges();
            var datasets = contextobjs.CartsDatabases.ToList();
            int totalGross = 0;

            for (int j = 0; j < contextobjs.CartsDatabases.ToList().Count; j++)
            {
                totalGross = totalGross + Int32.Parse(contextobjs.CartsDatabases.ToList()[j].Gross);
            }
            ViewBag.Message = totalGross;

            return View("AddtoCart", datasets);
        }
        public ActionResult PlaceOrder()
        {


            
            foreach (var objectscart in contextobjs.CartsDatabases)
            {
                contextobjs.CartsDatabases.Remove(objectscart);
            }
            contextobjs.SaveChanges();
            var datasets = contextobjs.CartsDatabases.ToList();
            int totalGross = 0;

            for (int j = 0; j < contextobjs.CartsDatabases.ToList().Count; j++)
            {
                totalGross = totalGross + Int32.Parse(contextobjs.CartsDatabases.ToList()[j].Gross);
            }
            return View("PlaceOrder");
        }
        // POST: Home/Delete/5
        
    }
}
