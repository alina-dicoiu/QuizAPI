using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizAPI.Models
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<AnswerDTO> PossibleAnswers { get; set; }
    }
}