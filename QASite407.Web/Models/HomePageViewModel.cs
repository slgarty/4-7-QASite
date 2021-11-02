using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QASite407_2_.Data;

namespace QASite407.Web.Models
{
    public class HomePageViewModel
    {
        public List<Question> Questions { get; set; }
        public Question Question { get; set; }
        public User GetUser { get; set; }
    }
}
