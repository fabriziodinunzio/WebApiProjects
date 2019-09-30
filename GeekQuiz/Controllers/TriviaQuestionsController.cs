using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using GeekQuiz.Models;

namespace GeekQuiz.Controllers
{
    public class TriviaQuestionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/TriviaQuestions
        public IQueryable<TriviaQuestion> GetTriviaQuestions()
        {
            return db.TriviaQuestions;
        }

        // GET: api/TriviaQuestions/5
        [ResponseType(typeof(TriviaQuestion))]
        public async Task<IHttpActionResult> GetTriviaQuestion(int id)
        {
            TriviaQuestion triviaQuestion = await db.TriviaQuestions.FindAsync(id);
            if (triviaQuestion == null)
            {
                return NotFound();
            }

            return Ok(triviaQuestion);
        }

        // PUT: api/TriviaQuestions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTriviaQuestion(int id, TriviaQuestion triviaQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != triviaQuestion.Id)
            {
                return BadRequest();
            }

            db.Entry(triviaQuestion).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TriviaQuestionExists(id))
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

        // POST: api/TriviaQuestions
        [ResponseType(typeof(TriviaQuestion))]
        public async Task<IHttpActionResult> PostTriviaQuestion(TriviaQuestion triviaQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TriviaQuestions.Add(triviaQuestion);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = triviaQuestion.Id }, triviaQuestion);
        }

        // DELETE: api/TriviaQuestions/5
        [ResponseType(typeof(TriviaQuestion))]
        public async Task<IHttpActionResult> DeleteTriviaQuestion(int id)
        {
            TriviaQuestion triviaQuestion = await db.TriviaQuestions.FindAsync(id);
            if (triviaQuestion == null)
            {
                return NotFound();
            }

            db.TriviaQuestions.Remove(triviaQuestion);
            await db.SaveChangesAsync();

            return Ok(triviaQuestion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TriviaQuestionExists(int id)
        {
            return db.TriviaQuestions.Count(e => e.Id == id) > 0;
        }
    }
}