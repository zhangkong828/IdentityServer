using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Resource.Api1.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private static readonly List<Book> Books = new List<Book>
        {
            new Book{Id=1,Name="SICP"},
            new Book{Id=2,Name="lnh"}
        };

        [HttpGet]
        public List<Book> GetAll()
        {
            return Books;
        }

        [HttpGet]
        public Book GetById(int bookId)
        {
            return Books.FirstOrDefault(_ => _.Id == bookId);
        }

        [HttpPost]
        public Book Add(Book book)
        {
            Books.Add(book);
            return book;
        }

        [HttpPost]
        public Book Put(int bookId, Book book)
        {
            var oldBook = Books.FirstOrDefault(_ => _.Id == book.Id);
            if (oldBook != null)
            {
                oldBook.Name = book.Name;
            }
            return oldBook;
        }

        [HttpPost]
        public Book Delete(int bookId)
        {
            var book = Books.FirstOrDefault(_ => _.Id == bookId);
            if (book != null)
            {
                Books.Remove(book);
            }
            return book;
        }
    }
}
