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
using BookService.Models;
using BookService.Models.DTO;
using Newtonsoft.Json;

namespace BookService.Controllers
{
    public class BooksController : ApiController
    {
        private BookServiceContext db = new BookServiceContext();
        //
        // GET: api/Books
        //public async Task<HttpResponseMessage> GetBooksAsync()
        //{
        //    HttpResponseMessage resp = null;
        //    try
        //    {
        //        IList<BookDTO> bookList = await db.Books.Select(b => new BookDTO { Id = b.Id, AuthorName = b.Author.Name, Title = b.Title }).ToListAsync();
        //        //string serializedBooks = JsonConvert.SerializeObject(bookList);
        //        //Request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        //        resp = Request.CreateResponse(HttpStatusCode.OK, bookList);
        //    }
        //    catch (InvalidOperationException exOp)
        //    {

        //        resp = Request.CreateResponse(HttpStatusCode.InternalServerError, exOp.Message);
        //    }
        //    catch(Exception ex)
        //    {
        //        resp = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }

        //    return resp;
        //}

        // GET: api/Books
        public IQueryable<BookDTO> GetBooks()
        {
            var books = from b in db.Books
                        select new BookDTO()
                        {
                            Id = b.Id,
                            Title = b.Title,
                            AuthorName = b.Author.Name
                        };

            return books;
        }

        // GET: api/Books/5
        [ResponseType(typeof(BookDetailDTO))]
        public async Task<IHttpActionResult> GetBook(int id)
        {
            BookDetailDTO book = await db.Books
                .Include(b => b.Author)
                   .Select(c => new BookDetailDTO {
                                                    AuthorName = c.Author.Name
                                                    , Genre = c.Genre
                                                    , Id = c.Id
                                                    , Price = c.Price
                                                    , Title = c.Title
                                                    , Year = c.Year
                                                }
                   )
                   .FirstOrDefaultAsync(d => d.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // PUT: api/Books/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBook(int id, Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != book.Id)
            {
                return BadRequest();
            }

            db.Entry(book).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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

        // POST: api/Books
        [ResponseType(typeof(BookDTO))]
        public async Task<IHttpActionResult> PostBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Books.Add(book);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = book.Id }, new BookDTO { Id = book.Id, AuthorName = book.Author.Name,  Title = book.Title });
        }

        // DELETE: api/Books/5
        [ResponseType(typeof(Book))]
        public async Task<IHttpActionResult> DeleteBook(int id)
        {
            Book book = await db.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            db.Books.Remove(book);
            await db.SaveChangesAsync();

            return Ok(book);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookExists(int id)
        {
            return db.Books.Count(e => e.Id == id) > 0;
        }
    }
}