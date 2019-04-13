using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginCodeCase.Interfaces;
using LoginCodeCase.Model;
namespace LoginCodeCase.Pages
{
    public class UserDetailsModel : PageModel
    {
        private IUser _userService;
        public User UserDetails { get; set; }
        public UserDetailsModel(IUser userService)
        {
            _userService = userService;
        }
        public void OnGet(int id)
        {
            UserDetails = _userService.GetByID(id);
        }
    }
}