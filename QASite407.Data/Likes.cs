using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QASite407.Data{
    public class Likes
    {
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public User User;
        public Question Question;
    }
}
