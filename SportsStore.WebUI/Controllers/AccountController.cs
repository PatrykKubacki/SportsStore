using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;
using SportsStore.Domain.Data;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        readonly IAuthentication _userAuthenctication;
        readonly IEmailSender _emailSender;
        readonly IAddressRepository _addressRepository;
        readonly ICityRepository _cityRepository;
        readonly IUserRepository _userRepository;

        public AccountController(IAddressRepository addressRepository, ICityRepository cityRepository, IUserRepository userRepository, IEmailSender sender)
        {
            _addressRepository = addressRepository;
            _cityRepository = cityRepository;
            _userRepository = userRepository;
            _userAuthenctication = new Authentication(_userRepository);
            _emailSender = sender;
        }

        [AllowAnonymous]
        public ViewResult Login()
        {
            if (!HttpContext.User.Identity.IsAuthenticated) return View();

            var userEmail = HttpContext.User.Identity.Name;
            var user = _userRepository.Users.FirstOrDefault(u => u.Email == userEmail);
            return View("LoginResult",user);
        }
        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View();

            if (_userAuthenctication.CheckLogin(model.Email, model.Password))
            {
                var user = _userRepository.Users.FirstOrDefault(u => u.Email == model.Email);
                if (!user.Confirmed) return RedirectToAction("Confirm", new { email = user.Email });

                SignInUser(model.Email);
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Niepoprawny e-mail lub hasło");
            return View();
        }

        void SignInUser(string email)
        {
            SetupFormsAuthTicket(email, false);
            FormsAuthentication.SetAuthCookie(email, false);
        }
        
        void SetupFormsAuthTicket(string email, bool persistanceFlag)
        {
            var user = _userRepository.Users.FirstOrDefault(u=>u.Email == email);
            var userId = user?.Id;
            var userData = userId?.ToString(CultureInfo.InvariantCulture);
            var authTicket = new FormsAuthenticationTicket(1, user?.Email, DateTime.Now, DateTime.Now.AddMinutes(1),
                                                            persistanceFlag, userData);

            var encTicket = FormsAuthentication.Encrypt(authTicket);
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            var user = CreateNewClient();
            return View(user);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(User user)
        {
            if (!ModelState.IsValid) return View();

            try
            {
                if(_cityRepository.Cities.All(c => c.Name != user.Address.City.Name))
                    _cityRepository.SaveCity(CreateNewCity(user.Address.City.Name));

                if(_addressRepository.Addresses.All(a=>a.Street != user.Address.Street && a.Code != user.Address.Code
                                                    && a.Number != user.Address.Number && a.City.Name != user.Address.City.Name))
                    _addressRepository.SaveAddress(CreateNewAddress(user.Address));

                if(_userRepository.Users.All(u=>u.Email != user.Email && u.Phone != user.Phone))
                    _userRepository.SaveUser(CreateNewClient(user));

                return RedirectToAction("Confirm",new{email = user.Email});
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }

        [AllowAnonymous]
        public ActionResult Confirmation(string email)
        {
            _userRepository.ConfirmEmail(email);
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public ActionResult Confirm(string email)
        {
            var user = _userRepository.Users.FirstOrDefault(u => u.Email == email);
            if (user.Confirmed)
                RedirectToAction("Login");

            ViewBag.Email = email;
            var body = $"Twój login to: {email}\nPotwiedz swój adres e-mail http://localhost:3416/Account/Confirmation?email={email} \nW przeciwnym razie zignoruj te wiadomość.";
            _emailSender.SendMessage(email,"Pomyślnie Zarejestrowano", body);
            return View();
        }

        User CreateNewClient(User user = null)
        {
            var addres = _addressRepository.Addresses.FirstOrDefault(a => a.Street == user?.Address.Street && a.Code == user?.Address.Code
                                                                          && a.Number == user?.Address.Number);
            return  new User
            {
                Id = 0,
                Email = user?.Email ?? "",
                Password = user?.Password ?? "",
                FirstName =  user?.FirstName ?? "",
                LastName =  user?.LastName ?? "",
                Phone = user?.Phone ?? "",
                Confirmed = user?.Confirmed ?? false,

                Address = addres ?? new Address
                {
                    CityId =  0,
                    City = new City { Id = 0},
                    Id = 0
                },
                RoleId = 2,
                AddressId = addres?.Id ?? 0
            };
        }

        Address CreateNewAddress(Address address)
        {
            return new Address
            {
                Id = 0,
                Code = address.Code,
                Number = address.Number,
                Street = address.Street,
                CityId = _cityRepository.Cities.FirstOrDefault(c => c.Name == address.City.Name)?.Id,
            };
        }

        City CreateNewCity(string name)
        {
            return new City{Id = 0, Name = name};
        }

        public ActionResult ChangePassword(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(int Id, ChangePasswordViewModel model)
        {
            ViewBag.Id = Id;
            if (!ModelState.IsValid) return View(model);

            _userRepository.ChangePassword(Id, model.NewPassword);
            TempData["message"] = "Zmieniono pomyślnie hasło";

            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public ActionResult ForgottenPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ForgottenPassword(ForgottenPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var user = _userRepository.Users.FirstOrDefault(u => u.Email == model.Email);
            if (user == null) return RedirectToAction("Login");

            var newPasssword = _userRepository.ResetPassword(user.Id);
            var body = $"Twoje hasło zostało zmienione na {newPasssword}\nMożesz teraz sie zalogować używając tego hasła";
            _emailSender.SendMessage(user.Email,"Resetowanie hasła",body);

            return RedirectToAction("Login");
        }

    }
}