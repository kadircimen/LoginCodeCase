using System.Threading.Tasks;
using LoginCodeCase.Model;
using LoginCodeCase.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace LoginCodeCase.Pages.Partials
{
    public class _loginViewComponent : ViewComponent
    {
        private IUser _userService;
        public _loginViewComponent(IUser userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync(UserLogin login)
        {
            var log = new Model.UserLogin();
            return View(log);
        }
    }
}
