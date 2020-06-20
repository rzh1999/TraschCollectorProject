using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrashCollector.Data;
using TrashCollector.Models;

namespace TrashCollector.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomersController : Controller
    {
        private ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customer
        public ActionResult Index(CustomersModel customersModel)
        {
            
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customers.Where(c => c.IdentityUserId == userId).SingleOrDefault();
            if (customer == null)
            {
                return RedirectToAction("Create");
            }

            return View(customer);
        }
        
        public ActionResult SetSuspendDate(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customers.Where(c => c.IdentityUserId == userId).SingleOrDefault();
            if (customer == null)
            {
                return RedirectToAction("Create");
            }
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetSuspendDate(CustomersModel customersModel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customers.Where(c => c.IdentityUserId == userId).SingleOrDefault();
            customer.SuspendService = customersModel.SuspendService;
            customer.SuspendStart = customersModel.SuspendStart;
            customer.SuspendEnd = customersModel.SuspendEnd;

            if (customersModel.SuspendService == false)
            {
                return View();
            }
            else
            {
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult CustomerBalance(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customers.Where(c => c.IdentityUserId == userId).SingleOrDefault();
            if (customer == null)
            {
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        //Get Customer Pick Up
        public ActionResult CustomerPickUp(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customers.Where(c => c.IdentityUserId == userId).SingleOrDefault();
            if (customer == null)
            {
                return RedirectToAction("Create");
            }
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerPickUp(CustomersModel customersModel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customers.Where(c => c.IdentityUserId == userId).SingleOrDefault();
            customer.PickUpDay = customersModel.PickUpDay;
            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        // GET: Customer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomersModel customersModel)
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                customersModel.IdentityUserId = userId;
                _context.Customers.Add(customersModel);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Customer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}