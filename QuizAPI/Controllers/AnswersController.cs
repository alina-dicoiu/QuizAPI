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
    public class AnswersController : ApiController
    {
        private QuizDBEntities db = new QuizDBEntities();

        // GET: api/Answers/All

        [ActionName("All")]

        public IEnumerable<AnswerDTO> GetAnswers()
        {
            return (from a in db.Answers
                    select new AnswerDTO()
                    {
                        Id = a.Id,
                        Text = a.Text,
                        Correct = a.Correct,
                        QuestionId = a.QuestionId,
                        QuestionText = a.Question.Text

                    }).ToList();
        }

        // GET: api/Answers/ById/5
        [ResponseType(typeof(AnswerDTO))]

        [ActionName("ById")]

        public IHttpActionResult GetAnswer(int id)
        {
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return NotFound();
            }

            return Ok(new AnswerDTO()
            {
                Id = answer.Id,
                Text = answer.Text,
                Correct = answer.Correct,
                QuestionId = answer.QuestionId,
                QuestionText = answer.Question.Text
            });
        }

        // PUT: api/Answers/Update/5
        [ResponseType(typeof(void))]

        [ActionName("Update")]

        public IHttpActionResult PutAnswer(int id, Answer answer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != answer.Id)
            {
                return BadRequest();
            }

            db.Entry(answer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(id))
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

        // POST: api/Answers/Add
        [ResponseType(typeof(int))]

        [ActionName("Add")]

        public IHttpActionResult PostAnswer(Answer answer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Answers.Add(answer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = answer.Id }, new AnswerDTO()
            {
                Id = answer.Id,
                Text = answer.Text,
                Correct = answer.Correct,
                QuestionId = answer.QuestionId,
                QuestionText = answer.Question.Text
            });
              
        }

        // DELETE: api/Answers/Delete/5
        [ResponseType(typeof(Answer))]

        [ActionName("Delete")]

        public IHttpActionResult DeleteAnswer(int id)
        {
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return NotFound();
            }

            db.Answers.Remove(answer);
            db.SaveChanges();

            return Ok(answer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AnswerExists(int id)
        {
            return db.Answers.Count(e => e.Id == id) > 0;
        }
    }
}