using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QASite.web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using QASite.data;

namespace QASite.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repository = new QARepository(connectionString);
            var questions = repository.GetAllQuestions();
            
            ShowAllQuestionsViewModel vm = new ShowAllQuestionsViewModel
            {
              
                Questions = questions
            };

            return View(vm);
        }
     
    
      
    }
}
