﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AventWithMVC.Controllers
{
    public class DocumentsController : Controller
    {
        // GET: Documents
        public ActionResult Uploaddocumnets()
        {
            return View();
        }
    }
}