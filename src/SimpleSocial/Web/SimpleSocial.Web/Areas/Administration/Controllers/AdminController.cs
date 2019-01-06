﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleSocial.Services.DataServices.SearchDataServices;
using SimpleSocial.Web.Areas.Administration.Services;
using SimpleSocial.Web.Areas.Administration.ViewModels;

namespace SimpleSocial.Web.Areas.Administration.Controllers
{
    [Authorize("Admin")]
    [Area("Administration")]
    public class AdminController : Controller
    {
        private readonly IAdministrationServices administrationServices;
        private readonly ISearchServices searchServices;

        public AdminController(
            IAdministrationServices administrationServices,
            ISearchServices searchServices)
        {
            this.administrationServices = administrationServices;
            this.searchServices = searchServices;
        }

        public IActionResult Index()
        {
            ViewData["Quote"] = administrationServices.GetRandomQuote();
            return View();
        }

        public IActionResult AllReports()
        {
            return View();
        }

        public IActionResult ManageUsers(string searchText)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                var result = this.searchServices.GetResultOfSearch(searchText, User);
                var users = result.UsersFound.Users.ToList();
                var usersFoundToPromoteDemote = new PromoteDemoteViewModel()
                {
                    AdminUsers = administrationServices.GetAdminUsers(User,users),
                    NonAdminUsers = administrationServices.GetNonAdminUsers(User,users)
                };

                return View(usersFoundToPromoteDemote);
            }

            var usersToPromoteDemote = new PromoteDemoteViewModel
            {
                AdminUsers = administrationServices.GetAdminUsers(User),
                NonAdminUsers = administrationServices.GetNonAdminUsers(User)
            };

            return View(usersToPromoteDemote);
        }

        public IActionResult Promote(string id)
        {
            administrationServices.PromoteUser(id);
            return RedirectToAction("ManageUsers");
        }

        public IActionResult Demote(string id)
        {
            administrationServices.DemoteUser(id);
            return RedirectToAction("ManageUsers");
        }
    }
}