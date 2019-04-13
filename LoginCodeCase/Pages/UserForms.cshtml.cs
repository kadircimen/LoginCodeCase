

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginCodeCase.Model;
using LoginCodeCase.Interfaces;
using LoginCodeCase.Enums;
using Microsoft.AspNetCore.Http;
using LoginCodeCase.Info;
using AutoMapper;
using LoginCodeCase.Helpers;
using Microsoft.Extensions.Options;
namespace LoginCodeCase.Pages
{

    /// <summary>
    /// Tüm form componentlarının post ve getleri handler ile ayrıştırılarak view a ilgili component render ediliyor.
    /// </summary>
    public class UserFormsModel : PageModel
    {
        private IUser _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        public int FormType = 0;
        public int UserID = 0;
        public const string MessageKey = nameof(MessageKey);
        public const string PageTitle = nameof(PageTitle);
        public UserFormsModel(IUser userService, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        //Sayfaya default düşen Get Methodu. Burada kullanıcının daha önce login yapıp yapmadığını session verilerine bakarak kontrol edilir.
        //Duruma göre yönlendirmeler yapılır
        public async Task<IActionResult> OnGetAsync()
        {
            int sessionCheck = 0;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("loginState")) || HttpContext.Session.GetString("loginState") == Enum.GetName(typeof(UserState), 1))
            {
                sessionCheck = (int)UserState._logged;
                HttpContext.Session.SetInt32("loginState", (int)UserState._logged);
                return Redirect("/user-details/" + HttpContext.Session.GetInt32("loggedUserId"));
            }
            else
            {
                return Redirect("/forms/loginpage");
            }
        }
        //
        //OnLogin Handlers
        public UserLogin Login { get; set; } //-> ComponentView daki form bilgilerini içeren class.
        public async Task OnGetLoginPageAsync()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("loginState")) || HttpContext.Session.GetString("loginState") == Enum.GetName(typeof(UserState), 1))
            {
                //ComponentView a parametre gönderilecek olan Login classının modele eklenmesi
                Login = new UserLogin();
                FormType = (int)FormTypes._login;
                TempData[PageTitle] = "Kullanıcı Girişi";
            }
            else
            {
                Redirect("/user-details/" + HttpContext.Session.GetInt32("loggedUserId"));
            }
        }
        public async Task<IActionResult> OnPostLoginFormAsync(UserLogin Login)
        {
            if (!ModelState.IsValid)
                return Page();
            //login işlemleri...
            var user = _userService.Authenticate(Login.Email, Login.Password);
            if (user == null)
            {
                TempData[MessageKey] = "Kullanıcı Bulunamadı";
                return Page();
            }
            else
            {
                HttpContext.Session.SetInt32("loggedUserId", user.ID);
                HttpContext.Session.SetString("loginState", Enum.GetName(typeof(UserState), 1).ToString());
                TempData[MessageKey] = "Giriş Yapıldı";
                return Redirect("/user-details/" + user.ID);
            }
        }
        //OnRegister Handlers
        public UserDto RegisterUser { get; set; }
        public async Task OnGetRegisterPageAsync()
        {
            TempData[PageTitle] = "Üye Ol";
            RegisterUser = new UserDto();
            FormType = (int)FormTypes._register;
        }
        public async Task<IActionResult> OnPostRegisterFormAsync(UserDto RegisterUser)
        {
            if (!ModelState.IsValid)
                return Page();
            var user = _mapper.Map<User>(RegisterUser);
            try
            {
                //save user
                _userService.Create(user, RegisterUser.Password);
                TempData[MessageKey] = "Üye Kaydı Gerçekleştirildi";
                return RedirectToAction(Request.Path);
            }
            catch (AppException ex)
            {
                TempData[MessageKey] = "Kullanıcı Eklerken Hata Oluştu";
                return RedirectToAction(Request.Path);
            }
        }
        //OnForget Handlers -PasswordForm
        public ChangePass Pass { get; set; }
        public async Task OnGetPasswordFormAsync()
        {
            Pass = new ChangePass();
            FormType = (int)FormTypes._forget;
            TempData[PageTitle] = "Şifre Güncelle";
        }
        public async Task<IActionResult> OnPostPasswordFormAsync(ChangePass NewPassword)
        {
            if (!ModelState.IsValid)
                return Page();
            var user = _userService.GetByEmail(NewPassword.Email);
            try
            {
                //save user
                if (user != null)
                {
                    _userService.Update(user.ID, NewPassword.Password);
                    TempData[MessageKey] = "Şifre Değiştirildi";
                    HttpContext.Session.Clear();
                }
                else
                {
                    TempData[MessageKey] = "Kullanıcı Bulunamadı";
                }
                return Redirect("/forms/loginform");
            }
            catch (AppException ex)
            {
                TempData[MessageKey] = "Şifre Değiştirirken Hata Oluştu";
                return RedirectToAction(Request.Path);
            }
        }


        //Oturumu Kapat
        public async Task<IActionResult> OnGetSignOut()
        {
            HttpContext.Session.Clear();
            return Redirect("/forms/loginpage");
        }

        //Address Handlers
        //şehir ve caddeleri generic oluşturduğum listeler içerisinden jquery ajax ile çekmekteyim.
        public IList<CityValues> Cities { get; set; }
        public IList<DistrictValues> Districts { get; set; }
        public IActionResult OnGetDistrict(int city)
        {
            Districts = new List<DistrictValues>
            {
                new DistrictValues{ID=1, CityID=1,Name="Adalar"},
                new DistrictValues{ID=2, CityID=1,Name="Büyükçekmece"},
                new DistrictValues{ID=3, CityID=1,Name="Gaziosmanpaşa"},
                new DistrictValues{ID=4, CityID=1,Name="Sarıyer"},
                new DistrictValues{ID=5, CityID=1,Name="Topkapı"},
                new DistrictValues{ID=6, CityID=2,Name="Akyurt"},
                new DistrictValues{ID=7, CityID=2,Name="Altındağ"},
                new DistrictValues{ID=8, CityID=2,Name="Ayaş"},
                new DistrictValues{ID=9, CityID=2,Name="Çamlıdere"},
                new DistrictValues{ID=10, CityID=2,Name="Çankaya"},
                new DistrictValues{ID=11, CityID=3,Name="Aliağa"},
                new DistrictValues{ID=12, CityID=3,Name="Balçova"},
                new DistrictValues{ID=13, CityID=3,Name="Bayındır"},
                new DistrictValues{ID=14, CityID=3,Name="Bayraklı"},
                new DistrictValues{ID=15, CityID=3,Name="Konak"},
                new DistrictValues{ID=16, CityID=4,Name="Akseki"},
                new DistrictValues{ID=17, CityID=4,Name="Aksu"},
                new DistrictValues{ID=18, CityID=4,Name="Alanya"},
                new DistrictValues{ID=19, CityID=4,Name="Demre"},
                new DistrictValues{ID=20, CityID=4,Name="Döşemealtı"}
            };

            string districts = "<option value=\"-1\">İlçe Seçiniz</option>";
            foreach (var item in Districts.Where(x => x.CityID == city))
            {
                districts += "<option value=\"" + item.ID + "\">" + item.Name + "</option>";
            }

            return Content(districts, "text/html");
        }
        public IActionResult OnGetCities()
        {
            Cities = new List<CityValues>{
                new CityValues{ ID = 1, Name = "İstanbul"},
                new CityValues{ ID = 2, Name = "Ankara"},
                new CityValues{ ID = 3, Name = "İzmir" },
                new CityValues{ ID = 4, Name = "Antalya"}
            };
            string city = "<option value=\"-1\">İl Seçiniz</option>";
            foreach (var item in Cities)
            {
                city += "<option value=\"" + item.ID + "\">" + item.Name + "</option>";
            }

            return Content(city, "text/html");
        }
    }
}