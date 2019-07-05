using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizAPI.Models
{
    public class AnswerDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Correct { get; set; }
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
    }
}