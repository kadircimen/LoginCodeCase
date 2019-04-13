using LoginCodeCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginCodeCase.Info;

namespace LoginCodeCase.Pages.Partials
{
    public class _forgetViewComponent : ViewComponent
    {
        private IUser _userService;
        public _forgetViewComponent(IUser userService)
        {
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync(ChangePass pwd)
        {
            var pass = new ChangePass();
            return View(pass);
        }
    }
}
