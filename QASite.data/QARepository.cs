using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace QASite.data
{
    public class QARepository
    {

        private readonly string _connectionString;

        public QARepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Question> GetAllQuestions()
        {
            using var context = new QADbContext(_connectionString);
            return context.Questions.OrderByDescending(q => q.Date).Include(q => q.Likes).Include(q => q.Answers).Include(q => q.QuestionsTags).ThenInclude(qt => qt.Tag).ToList();
        }
        private int AddTag(string text)
        {
            using var context = new QADbContext(_connectionString);
            var newTag = new Tag
            {
                Text = text
            };
            context.Tags.Add(newTag);
            context.SaveChanges();
            return newTag.Id;

        }
        public void AddQuestion(string title, string text, List<string> tags, int userId)
        {
            using var context = new QADbContext(_connectionString);
            var question = new Question
            {
                Date = DateTime.Now,
                Text = text,
                Title = title,
                UserId = userId
            };
            context.Questions.Add(question);
            context.SaveChanges();
            foreach(string tagText in tags)
            {
                var currentTag = context.Tags.FirstOrDefault(tag => tag.Text == tagText);
                var tagId = 0;
                if(currentTag == null)
                {
                    tagId = AddTag(tagText);
                    context.SaveChanges();
                    currentTag = context.Tags.FirstOrDefault(t => t.Id == tagId);
                }
                else
                {
                    tagId = currentTag.Id;
                }
                context.QuestionsTags.Add(new QuestionsTags
                {
                    QuestionId = question.Id,
                    Question = question,
                    TagId = tagId,
                    Tag = currentTag
                });
                context.SaveChanges();
            }
        }
        public Question GetQuestionById(int id)
        {
            using var context = new QADbContext(_connectionString);
            return context.Questions.Include(q => q.Answers).ThenInclude(a => a.user).Include(q => q.QuestionsTags).ThenInclude(qt => qt.Tag).Include(q => q.User).FirstOrDefault(q => q.Id == id);
        }
        public int GetLikesForQuestion(int questionId)
        {
            using var context = new QADbContext(_connectionString);
            var question = context.Questions.Include(q => q.Likes).ThenInclude(l => l.User).FirstOrDefault(q => q.Id == questionId);
            return question.Likes.Count;
        }
        public void AddLikeForQuestion(int questionId, User user)
        {
            using var context = new QADbContext(_connectionString);
            var like = new Likes
            {
                QuestionId = questionId,
                UserId = user.Id
            };
            context.Likes.Add(like);
            context.SaveChanges();
        }
        public bool CanLike(int userId)
        {
            using var context = new QADbContext(_connectionString);
            var like = context.Likes.FirstOrDefault(l => l.UserId == userId);
            return like == null ? true : false;
        }
        public void AddAnswer(int questionId, string text, int userId)
        {
            using var context = new QADbContext(_connectionString);
            var answer = new Answer
            {
                Date = DateTime.Now,
                QuestionId = questionId,
                Text = text,
               UserId = userId
   
            };
            context.Answers.Add(answer);
            context.SaveChanges();
        }
    }
    
}
