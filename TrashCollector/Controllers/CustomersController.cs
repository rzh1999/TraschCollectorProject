﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
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
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = _context.Customers.Where(c => c.IdentityUserId == userId).SingleOrDefault();
                if (customer == null)
                {
                    return RedirectToAction("Create");
                }

                return View(customer);
            }
            catch
            {
                return RedirectToAction("Create");
            }
           
        }
        public ActionResult SetPickUpDate(string id)
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = _context.Customers.Where(c => c.IdentityUserId == userId).SingleOrDefault();
                if (customer == null)
                {
                    return RedirectToAction("Index");
                }
                return View(customer);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetPickUpDate(CustomersModel customersModel)
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = _context.Customers.Where(c => c.IdentityUserId == userId).SingleOrDefault();
                customer.OneTimeDate = customersModel.OneTimeDate;


                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
           
        }
            public ActionResult SetSuspendDate(string id)
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = _context.Customers.Where(c => c.IdentityUserId == userId).SingleOrDefault();
                if (customer == null)
                {
                    return RedirectToAction("Create");
                }
                return View(customer);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetSuspendDate(CustomersModel customersModel)
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = _context.Customers.Where(c => c.IdentityUserId == userId).SingleOrDefault();
                customer.SuspendService = customersModel.SuspendService;
                customer.SuspendStart = customersModel.SuspendStart;
                customer.SuspendEnd = customersModel.SuspendEnd;


                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult CustomerBalance(string id)
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = _context.Customers.Where(c => c.IdentityUserId == userId).SingleOrDefault();
                if (customer == null)
                {
                    return RedirectToAction("Index");
                }
                return View(customer);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
        //Get Customer Pick Up
        public ActionResult CustomerPickUp(string id)
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = _context.Customers.Where(c => c.IdentityUserId == userId).SingleOrDefault();
                if (customer == null)
                {
                    return RedirectToAction("Create");
                }
                return View(customer);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerPickUp(CustomersModel customersModel)
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = _context.Customers.Where(c => c.IdentityUserId == userId).SingleOrDefault();

                string formattedDay = DayOfWeekFormatter.FormatDay(customersModel.PickUpDay);
                customer.PickUpDay = formattedDay;

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
      
        
        // GET: Customer/Create
        public ActionResult Create()
        {
            CustomersModel customersModel = new CustomersModel();
            customersModel.Days = new SelectList(Enum.GetValues(typeof(DayOfWeek)));
            
            
            return View(customersModel);
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CustomersModel customersModel)
        {
            HttpClient httpClient = new HttpClient();

            var url =
                string.Format(
                    "https://maps.googleapis.com/maps/api/geocode/json?address={0},+{1},+{2}&key={3}",
                    customersModel.Address, customersModel.City, customersModel.State, ApiKeys.googleApi);

            
            HttpResponseMessage response =  await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                JObject parsedJson = JObject.Parse(jsonResponse);

                var results = parsedJson["results"];
                var latitude = (double)results[0]["geometry"]["location"]["lat"];
                var longitude = (double)results[0]["geometry"]["location"]["lng"];
                var validAddress = results[0]["geometry"]["location_type"].ToString();
                
                if (validAddress == "APPROXIMATE")
                {
                    customersModel.Error = "Invalid Address";
                    return View(customersModel);
                }

                customersModel.Lattitude = latitude;
                customersModel.Longitude = longitude;
            }
                
            
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
    }
}