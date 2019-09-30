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
using GeekQuiz.Models;

namespace GeekQuiz.Controllers
{
    public class TriviaAnswersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/TriviaAnswers
        public IQueryable<TriviaAnswer> GetTriviaAnswers()
        {
            return db.TriviaAnswers;
        }

        // GET: api/TriviaAnswers/5
        [ResponseType(typeof(TriviaAnswer))]
        public IHttpActionResult GetTriviaAnswer(int id)
        {
            TriviaAnswer triviaAnswer = db.TriviaAnswers.Find(id);
            if (triviaAnswer == null)
            {
                return NotFound();
            }

            return Ok(triviaAnswer);
        }

        // PUT: api/TriviaAnswers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTriviaAnswer(int id, TriviaAnswer triviaAnswer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != triviaAnswer.Id)
            {
                return BadRequest();
            }

            db.Entry(triviaAnswer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TriviaAnswerExists(id))
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

        // POST: api/TriviaAnswers
        [ResponseType(typeof(TriviaAnswer))]
        public IHttpActionResult PostTriviaAnswer(TriviaAnswer triviaAnswer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TriviaAnswers.Add(triviaAnswer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = triviaAnswer.Id }, triviaAnswer);
        }

        // DELETE: api/TriviaAnswers/5
        [ResponseType(typeof(TriviaAnswer))]
        public IHttpActionResult DeleteTriviaAnswer(int id)
        {
            TriviaAnswer triviaAnswer = db.TriviaAnswers.Find(id);
            if (triviaAnswer == null)
            {
                return NotFound();
            }

            db.TriviaAnswers.Remove(triviaAnswer);
            db.SaveChanges();

            return Ok(triviaAnswer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TriviaAnswerExists(int id)
        {
            return db.TriviaAnswers.Count(e => e.Id == id) > 0;
        }
    }
}