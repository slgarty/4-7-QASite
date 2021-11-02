using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace QASite407_2_.Data
{
    public class QARepository
    {
        private readonly string _connectionString;

        public QARepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user, string password)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

            using (var context = new QADbContext(_connectionString))
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public User GetUserByEmail(string email)
        {
            using (var context = new QADbContext(_connectionString))
            {
                return context.Users.FirstOrDefault(u => u.Email == email);
            }
        }

        public User Login(string email, string password)
        {
            var user = GetUserByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return isValid ? user : null;

        }

        public void AddQuestion(Question question, List<string> tags)
        {
            question.DatePosted = DateTime.Now;
            question.UserId = question.User.Id;
            question.User = null;
            using var context = new QADbContext(_connectionString);
            context.Questions.Add(question);
            context.SaveChanges();
            foreach (string tag in tags)
            {
                Tag t = GetTag(tag);
                int tagId;
                if (t == null)
                {
                    tagId = AddTag(tag);
                }
                else
                {
                    tagId = t.Id;
                }
                context.QuestionsTags.Add(new QuestionsTags
                {
                    QuestionId = question.Id,
                    TagId = tagId
                });
            }

            context.SaveChanges();
        }

        public void AddAnswer(Answer answer, int questionId )
        {

            using (var context = new QADbContext(_connectionString))
            {
                answer.QuestionId = questionId;
                answer.UserId = answer.User.Id;
                answer.TimeAnswered = DateTime.Now;
                answer.User = null;
                context.Answers.Add(answer);
                context.SaveChanges();
            }
        }

        public void AddLike(Likes like)
        {
            using (var context = new QADbContext(_connectionString))
            {
                context.QuestionLikes.Add(like);
                context.SaveChanges();
            }
        }

        public List<Likes>GetLikes(int questionId)
        {
            using var context = new QADbContext(_connectionString);
            return context.QuestionLikes.Where(q => q.QuestionId == questionId).ToList();
        }

        public List<Question> Get()
        {
            using var context = new QADbContext(_connectionString);
            return context.Questions
                .Include(q => q.Answers)
                .Include(q => q.Likes)
                .Include(q => q.QuestionsTags)
                .ThenInclude(q => q.Tag)
                .OrderByDescending(q => q.DatePosted)
                .ToList();
        }

        public Question GetQuestionById(int id)
        {
            using var context = new QADbContext(_connectionString);
            return context.Questions
                .Include(q => q.User)
                .ThenInclude(q => q.LikedQuestions)
                .Include(q => q.Answers)
                .ThenInclude(a => a.User)
                .Include(q => q.Likes)
                .Include(u => u.QuestionsTags)
                .ThenInclude(qt => qt.Tag)
                .FirstOrDefault(q => q.Id == id);
        }

        public void AddLike(int questionId, int userId)
        {
            using var context = new QADbContext(_connectionString);
            var like = new Likes
            {
                QuestionId = questionId,
                UserId = userId
            };
            context.QuestionLikes.Add(like);
            context.SaveChanges();
        }

        public int GetQuestionLikes(int questionId)
        {
            using var context = new QADbContext(_connectionString);
            return context.QuestionLikes.Count(q => q.QuestionId == questionId);
        }

        private Tag GetTag(string name)
        {
            using var ctx = new QADbContext(_connectionString);
            return ctx.Tags.FirstOrDefault(t => t.Name == name);
        }

        private int AddTag(string name)
        {
            using var ctx = new QADbContext(_connectionString);
            var tag = new Tag { Name = name };
            ctx.Tags.Add(tag);
            ctx.SaveChanges();
            return tag.Id;
        }


    }
}
