﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWorld.ViewModels;
using TheWorld.Services;
using Microsoft.Extensions.Configuration;
using TheWorld.Models;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller

    {
        private IMailService _mailService;
        private IConfigurationRoot _config;
        private WorldContext _context;

        public AppController(IMailService mailService, IConfigurationRoot config, WorldContext context)
        {
            _mailService = mailService;
            _config = config;
            _context = context;
        }
        public IActionResult Index()
        {
            var data = _context.Trips.ToList();
            return View(data);
        }

       

        public IActionResult Contact()
        {
           return View();
        }
        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (model.Email.Contains("aol.com")) ModelState.AddModelError("Email", "We Dont support AOL Addresses");

            if (ModelState.IsValid)
            {

                _mailService.SendMail(_config["MailSettings:ToAddress"], model.Email, "From TheWorld", model.Message);
            }
            ModelState.Clear();

            ViewBag.UserMessage = "Message Sent";

            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
