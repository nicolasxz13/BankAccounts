using BankAccounts.Data;
using BankAccounts.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankAccounts.Controllers
{
    public class Login : Controller
    {
        private LoginContext _context;

        public Login(LoginContext loginContext)
        {
            _context = loginContext;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("login")]
        public IActionResult Login_in([Bind(Prefix = "Login")] UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                User? userInDb = _context.Users
                    .Select(
                        a =>
                            new User
                            {
                                Name = a.Name,
                                UserId = a.UserId,
                                Email = a.Email,
                                Password = a.Password
                            }
                    )
                    .SingleOrDefault(u => u.Email == userDTO.Email);
                if (userInDb == null)
                {
                    return View("Index");
                }

                PasswordHasher<UserDTO> hasher = new PasswordHasher<UserDTO>();

                var result = hasher.VerifyHashedPassword(
                    userDTO,
                    userInDb.Password,
                    userDTO.Password
                );

                if (result == 0)
                {
                    return View("Index");
                }
                else
                {
                    HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                    HttpContext.Session.SetString("UserName", userInDb.Name);
                    return RedirectToAction("Index","Accounts",new{id = userInDb.UserId});
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost("create")]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();

                user.Password = Hasher.HashPassword(user, user.Password);

                user.CreatedAt = DateTime.Now;
                user.UpdatedAt = DateTime.Now;
                _context.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View("Index");
            }
        }



    }
}
