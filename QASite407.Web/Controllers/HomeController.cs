using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QASite407.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using QASite407_2_.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;

namespace QASite407.Web.Controllers
{
    public class HomeController : Controller
    {
        private IWebHostEnvironment _environment;
        private readonly string _connectionString;

        public HomeController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _environment = environment;
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            var repo = new QARepository(_connectionString);
            return View(repo.Get());
        }
        public IActionResult AskQuestion()
        {
            if(!User.Identity.IsAuthenticated)
            {
                return Redirect("/home/login");
            }
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult AskQuestion(Question question, List<string>tags)
        {
            var repo = new QARepository(_connectionString);
            var user = repo.GetUserByEmail(User.Identity.Name);
            question.User = user;
            repo.AddQuestion(question, tags);
            return Redirect("/");
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddAnswer(Answer answer, int questionId)
        {
            var repo = new QARepository(_connectionString);
            var user = repo.GetUserByEmail(User.Identity.Name);
            answer.User = user;
            repo.AddAnswer(answer, questionId);
            return Redirect($"/home/ViewQuestion?id={questionId}");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var db = new QARepository(_connectionString);
            var user = db.Login(email, password);
            if (user == null)
            {
                TempData["message"] = "Invalid email/password combination";
                return Redirect("/home/login");
            }

            var claims = new List<Claim>
            {
                new Claim("user", email)
            };


            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

            return Redirect("/home/Index");
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(User user, string password)
        {
            var db = new QADbContext(_connectionString);
            var repository = new QARepository(_connectionString);
            repository.AddUser(user, password);
            return Redirect("/home/login");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }
        public IActionResult ViewQuestion(int id)
        {
            var repo = new QARepository(_connectionString);
            var vm = new HomePageViewModel();
            vm.Question = repo.GetQuestionById(id);
            vm.GetUser = repo.GetUserByEmail(User.Identity.Name);
            return View(vm);
        }
        [HttpPost]
        [Authorize]
        public void AddLike(int questionId)
        {
            var repo = new QARepository(_connectionString);
            var user = repo.GetUserByEmail(User.Identity.Name);
            repo.AddLike(questionId, user.Id);
        }

        public IActionResult GetLikes(int questionId)
        {
            var repo = new QARepository(_connectionString);
            return Json(new { likes = repo.GetQuestionLikes(questionId) });
        }

    }
}
