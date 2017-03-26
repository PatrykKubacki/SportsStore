using System;
using System.Globalization;
using System.Linq;
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
                SignInUser(model.Email);
                var user = _userRepository.Users.FirstOrDefault(u => u.Email == model.Email);
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

                _emailSender.SendMessage(user.Email);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }

            return RedirectToAction("Login");
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
    }
}