using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QASite407_2_.Data
{
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime TimeAnswered { get; set; }
        public int QuestionId { get; set; }
        public User User { get; set; }
    }
}
