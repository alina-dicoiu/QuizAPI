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
    public class CategoriesController : ApiController
    {
        private QuizDBEntities db = new QuizDBEntities();

        // GET: api/Categories/All

        [ActionName("All")]

        public IEnumerable<CategoryDTO> GetCategories()
        {
            return (from s in db.Categories
                    select new CategoryDTO()
                    {
                        Id = s.Id,
                        Name=s.Name
                    }).ToList();
        }

        // GET: api/Categories/ById/5
        [ResponseType(typeof(Category))]

        [ActionName("ById")]

        public IHttpActionResult GetCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(new CategoryDTO()
            {
                Id = category.Id,
                Name = category.Name
            });
        }

        // PUT: api/Categories/Update/5
        [ResponseType(typeof(void))]

        [ActionName("Update")]

        public IHttpActionResult PutCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.Id)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // POST: api/Categories/Add
        [ResponseType(typeof(Category))]

        [ActionName("Add")]

        public IHttpActionResult PostCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = category.Id }, new CategoryDTO()
            {
                Id = category.Id,
                Name = category.Name
            });
        }

        // DELETE: api/Categories/Delete/5
        [ResponseType(typeof(Category))]

        [ActionName("Delete")]

        public IHttpActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return Ok(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.Id == id) > 0;
        }
    }
}