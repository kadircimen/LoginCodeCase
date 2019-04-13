using LoginCodeCase.Info;
using LoginCodeCase.Interfaces;
using LoginCodeCase.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LoginCodeCase.Pages.Partials
{
    public class _registerViewComponent : ViewComponent
    {
        private IUser _userService;
        public _registerViewComponent(IUser userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync(UserDto user)
        {
            var userreg = new UserDto();
            userreg = user;
            return View(userreg);
        }
    }
}
