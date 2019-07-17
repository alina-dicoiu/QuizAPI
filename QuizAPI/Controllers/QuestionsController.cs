using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using QuizAPI.Models;

namespace QuizAPI.Controllers
{
    public class QuestionsController : ApiController
    {

        private QuizDBEntities db = new QuizDBEntities();

        // GET: api/Questions/All

        [ActionName("All")]

        public IEnumerable<QuestionDTO> GetQuestions()
        {
            return (from s in db.Questions
                    select new QuestionDTO()
                    {
                        Id = s.Id,
                        Text = s.Text,
                        CategoryId = s.CategoryId,
                        CategoryName = s.Category.Name,
                        PossibleAnswers = (from a in s.Answers
                                           select new AnswerDTO()
                                           {
                                               Id = a.Id,
                                               Text = a.Text,
                                               QuestionId = a.QuestionId,
                                               QuestionText = a.Question.Text,
                                               Correct = a.Correct
                                           }).ToList()
                    }).ToList(); 
        }

        // GET: api/Questions/ByCategory/5


        [ActionName("ByCategory")]

        public IEnumerable<QuestionDTO> GetQuestionsByCategory( int id )
        {
            return (from s in db.Questions 
                    where s.Category.Id == id
                    select new QuestionDTO()
                    {
                        Id = s.Id,
                        Text = s.Text,
                        CategoryId = s.CategoryId,
                        CategoryName = s.Category.Name,
                        PossibleAnswers = (from a in s.Answers
                                           select new AnswerDTO()
                                           {
                                               Id = a.Id,
                                               Text = a.Text,
                                               QuestionId = a.QuestionId,
                                               QuestionText = a.Question.Text,
                                               Correct = a.Correct
                                           }).ToList()
                    }).ToList();
        }

        // GET: api/Questions/ById/5
        [ResponseType(typeof(Question))]

        [ActionName("ById")]

        public IHttpActionResult GetQuestion(int id)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return NotFound();
            }

            return Ok(new QuestionDTO()
            {
                Id = question.Id,
                Text = question.Text,
                CategoryId = question.CategoryId,
                CategoryName = question.Category.Name,
                PossibleAnswers = (from a in question.Answers
                                   select new AnswerDTO()
                                   {
                                       Id = a.Id,
                                       Text = a.Text,
                                       QuestionId = a.QuestionId,
                                       QuestionText = a.Question.Text,
                                       Correct = a.Correct
                                   }).ToList()
            });
        }

        // PUT: api/Questions/Update/5
        [ResponseType(typeof(void))]

        [ActionName("Update")]

        public IHttpActionResult PutQuestion(int id, Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != question.Id)
            {
                return BadRequest();
            }

            db.Entry(question).State = EntityState.Modified;
            foreach(var answer in question.Answers)
            {
                db.Entry(answer).State = EntityState.Modified;
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Questions/Add
        [ResponseType(typeof(Question))]

        [ActionName("Add")]

        public IHttpActionResult PostQuestion(Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Questions.Add(question);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = question.Id }, new QuestionDTO()
            {
                Id = question.Id,
                Text = question.Text,
                CategoryId = question.CategoryId,
                PossibleAnswers = (from a in question.Answers
                                   select new AnswerDTO()
                                   {
                                       Id = a.Id,
                                       Text = a.Text,
                                       QuestionId = a.QuestionId,
                                       QuestionText = a.Question.Text,
                                       Correct = a.Correct
                                   }).ToList()
            });
        }

        // DELETE: api/Questions/Delete/5
        [ResponseType(typeof(Question))]

        [ActionName("Delete")]

        public IHttpActionResult DeleteQuestion(int id)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return NotFound();
            }

            db.Questions.Remove(question);
            db.SaveChanges();

            return Ok(question);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuestionExists(int id)
        {
            return db.Questions.Count(e => e.Id == id) > 0;
        }
    }
}