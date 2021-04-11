using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using QASite.data;
using QASite.web.Models;
namespace QASite.web.Controllers
{
 
    public class QuestionsController : Controller
    {
        private readonly IConfiguration _configuration;

        public QuestionsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult AskAQuestion()
        {
            return View();
        }
        public IActionResult AddQuestion(string title, string text, List<string> tags)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repository = new QARepository(connectionString);
            var userRepository = new UserRepository(connectionString);
            User user;
            if (User.Identity.IsAuthenticated)
            {
                user = userRepository.GetUser(User.Identity.Name);
            }
            else
            {
                return Redirect("/home/login");
            }
            repository.AddQuestion(title, text, tags, user.Id);
            return Redirect("/home/index");
        }
        public IActionResult ViewQuestion(int questionId)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repository = new QARepository(connectionString);
            var userRepository = new UserRepository(connectionString);
            var question = repository.GetQuestionById(questionId);
            User user;
            var canLike = true;
            string alert = null;
            var canAnswer = true;
            if (User.Identity.IsAuthenticated)
            {
               user = userRepository.GetUser(User.Identity.Name);
               canLike = repository.CanLike(user.Id);
                if (!canLike)
                {
                    TempData["Login"] = "You have already liked this question";
                }
            }
            else
            {
                user = null;
                canLike = false;
                TempData["Login"] = "You must be logged in to like a question";
                canAnswer = false;
            }
            alert = (string)TempData["Login"];

            var vm = new ShowSingleQuestionViewModel
            {
                Question = question,
                User = user,
                CanLike = canLike,
                Alert = alert,
                CanAnswer = canAnswer
            };
            
            return View(vm);
        }
        public IActionResult GetLikesForQuestion(int questionId)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repository = new QARepository(connectionString);
            var likes = repository.GetLikesForQuestion(questionId);
            return Json(likes);
        }
        [HttpPost]
        public IActionResult LikeQuestion(int questionId)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repository = new QARepository(connectionString); 
            var userRepository = new UserRepository(connectionString);
            User user;
            if (User.Identity.IsAuthenticated)
            {
                user = userRepository.GetUser(User.Identity.Name);
            }
            else
            {
                return Redirect("/home/login");
            }
            repository.AddLikeForQuestion(questionId, user);
            return Json(null);
        }
        public IActionResult AddAnswer(int questionId, string text)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repository = new QARepository(connectionString); var userRepository = new UserRepository(connectionString);
            User user;
            if (User.Identity.IsAuthenticated)
            {
                user = userRepository.GetUser(User.Identity.Name);
            }
            else
            {
                return Redirect("/home/login");
            }
            repository.AddAnswer(questionId, text, user.Id);
            return Redirect("/home/index");
        }
    }


}
