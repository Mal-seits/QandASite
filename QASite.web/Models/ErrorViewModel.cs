using System;
using System.Collections.Generic;
using System.IO;
using QASite.data;
namespace QASite.web.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
    public class ShowAllQuestionsViewModel
    {
        public List<Question> Questions { get; set; }
        public List<Tag> Tags { get; set; }
    }
    public class ShowSingleQuestionViewModel
    {
        public Question Question { get; set; }
        public User User { get; set; }
        public bool CanLike { get; set; }
        public string Alert { get; set; }
        public bool CanAnswer { get; set; }
    }

}
