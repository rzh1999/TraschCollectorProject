﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TrashCollector.Data;
using TrashCollector.Models;

namespace TrashCollector.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeesController : Controller
    {
       

        private ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Employees
        public ActionResult Index()
        {
            CustomerEmployeeViewModel viewModel = new CustomerEmployeeViewModel();

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employee = _context.Employees.Where(c => c.IdentityUserId == userId).SingleOrDefault();
            viewModel.Employee = employee;

            DateTime dateTime = DateTime.Now;
            var date =  dateTime.ToString("dddd");
          
            
            if (employee == null)
            {
                return RedirectToAction("Create");
            }
            try
            {

                var myCustomers = _context.Customers.Where(c => c.PickUpDay == date && c.ZipCode == employee.ZipCode && c.SuspendService != true  || c.OneTimeDate == dateTime).ToList();
                myCustomers.RemoveAll(d => d.SuspendStart <= dateTime || d.SuspendEnd >= dateTime);
                               
                return View(myCustomers);
            }
            catch
            {
                return View();
            }
        }


        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeesModel employeesModel)
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
               employeesModel.IdentityUserId = userId;
                _context.Employees.Add(employeesModel);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
                
            }
            catch
            {
                return View();
            }
        }
       
       

       

        public  async Task<IActionResult> GetCustomerDay(string EmpSearch, EmployeesModel employeesModel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            employeesModel = _context.Employees.Where(e => e.IdentityUserId == userId).SingleOrDefault();
            DateTime dateTime = DateTime.Now;
            var date = dateTime.ToString("dddd");
            ViewData["GetCustomerDay"] = EmpSearch;

            try
            {
                var empquery = _context.Customers.Where(c => c.ZipCode == employeesModel.ZipCode && c.SuspendService != true || c.OneTimeDate == dateTime).ToList();
                empquery.RemoveAll(d => d.SuspendStart <= dateTime || d.SuspendEnd >= dateTime);
                if (!String.IsNullOrEmpty(EmpSearch))
                {
                    EmpSearch = DayOfWeekFormatter.FormatDay(EmpSearch);
                    empquery = empquery.Where(x => x.PickUpDay.Contains(EmpSearch)).ToList();

                }
                return View(empquery);
            }
            catch
            {
                return View("Index");
            }
        }

      
        public ActionResult CompletePickUp(int id)
        {
            if (id != 0)
            {
                
                try
                {
                    var customer = _context.Customers.Where(x => x.CustomerId == id).SingleOrDefault();
                    customer.AccountBalance += customer.CollectionFee;
                    customer.ConfirmPickUp = true;
                    _context.SaveChanges();
                    return RedirectToAction(nameof(GetCustomerDay));
                }
                catch
                {
                    return View();
                }
              
            }
            return RedirectToAction(nameof(GetCustomerDay));
        }

        public ActionResult DisplayMap(int id)
        {
            CustomerEmployeeViewModel viewModel = new CustomerEmployeeViewModel();

            var customer = _context.Customers.Where(x => x.CustomerId == id).SingleOrDefault();
           
            return View(customer);
        }
    }
}