using ManageLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManageLibrary.Controllers
{
    public class AdminController : Controller
    {
        private readonly ManagerLibraryContext context = new ManagerLibraryContext();
        public IActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Product() 
        {
            var book = context.Books.Where(b => b.DeleteFlag==false).ToList();
            ViewBag.Book = book;
            return View();
        }
        public ActionResult AddProduct(string? message)
        {
            var authorList = context.Authors.Where(a => a.DeleteFlag==false).ToList();
            ViewBag.authorList = authorList;
            var genreList = context.Genres.Where(g => g.DeleteFlag == false).ToList();
            ViewBag.genreList = genreList;
            if(message != null || !string.IsNullOrEmpty(message))
            {
                ViewBag.message = message;
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(Book book)
        {
            var bookExit = context.Books.FirstOrDefault(b => b.Title.Equals(book.Title));
            if (bookExit==null)
            {
                book.CreatedAt = DateTime.Now;
                book.UpdatedAt = DateTime.Now;
                context.Books.Add(book);
                context.SaveChanges();
                ViewBag.message = "add successfull";
                ViewBag.book = book;
            }
            else
            {
                if (bookExit.DeleteFlag)
                {

                    bookExit.Image = book.Image;
                    bookExit.Subtitle= book.Subtitle;
                    bookExit.AuthorId = book.AuthorId;
                    bookExit.GenreId = book.GenreId;
                    bookExit.PublishingYear = book.PublishingYear;
                    bookExit.QuantityInStock = book.QuantityInStock;
                    bookExit.Description = book.Description;
                    bookExit.UpdatedAt = DateTime.Now;
                    bookExit.DeleteFlag = false;

                    context.Books.Update(bookExit);
                    context.SaveChanges();
                    ViewBag.message = "add successfull";
                    ViewBag.book = bookExit;
                }
                else
                {
                    ViewBag.message = "add fail";
                    return RedirectToAction("AddProduct");
                }
            }
            return View();
        }
        public ActionResult DeleteBook(int id)
        {
            var book = context.Books.FirstOrDefault(b => b.Id == id);
            if (book != null) { 
                book.DeleteFlag = true;
                context.Books.Update(book);
                context.SaveChanges();
                TempData["message"] = "delete successfull";
            }
            else { TempData["message"] = "delete successfull"; }
            return RedirectToAction("product");
        }
        public ActionResult DetailBook(int id)
        {
            var book = context.Books.FirstOrDefault(x => x.Id == id);

            if (book == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.BookDetail = book;
            return View();
        }
        public ActionResult Author()
        {
            var author = context.Authors.Where(au => au.DeleteFlag == false).ToList();
            foreach (var a in author)
            {
                a.Books = context.Books.Where(b => b.AuthorId == a.Id && b.DeleteFlag==false).ToList();
            }
            ViewBag.author = author;
            return View();
        }

        public ActionResult AddAuthor(string name)
        {
            var authorExit = context.Authors.FirstOrDefault(x => x.Name==name);
            if (authorExit == null) 
            {
                Author author = new Author();
                author.Name = name;
                author.CreatedAt = DateTime.Now;
                author.UpdatedAt = DateTime.Now;
                author.DeleteFlag = false;
                context.Authors.Add(author);
                context.SaveChanges();
                TempData["message"] = "add successfull";
            }
            else {
                if (authorExit.DeleteFlag)
                {
                    authorExit.DeleteFlag = false;
                    var listBook = context.Books.Where(x => x.AuthorId == authorExit.Id).ToList();
                    foreach(var book in listBook)
                    {
                        book.DeleteFlag=false;
                    }
                    context.Authors.Update(authorExit);
                    context.SaveChanges();
                    TempData["message"] = "add successfull";
                }
                else
                {
                    TempData["message"] = "add fail. Name Exit";
                }
            }
            
            return RedirectToAction("Author");
        }
        public ActionResult DeleteAuthor(int id)
        {
            var author = context.Authors.FirstOrDefault(x => x.Id == id);
            if (author != null)
            {
                author.DeleteFlag = true;
                var listBook = context.Books.Where(x => x.AuthorId == id).ToList();
                foreach(var book in listBook)
                {
                    book.DeleteFlag = true;
                    context.Books.Update(book);
                    context.SaveChanges();
                }
                context.Authors.Update(author);
                context.SaveChanges();
                TempData["message"] = "delete successfull";
            }
            else { TempData["message"] = "delete fail. author not exit"; }
            return RedirectToAction("Author");
        }

        public ActionResult Genre()
        {
            var genre = context.Genres.Where(g => g.DeleteFlag==false).ToList();
            foreach (var a in genre)
            {
                a.Books = context.Books.Where(b => b.GenreId == a.Id && b.DeleteFlag == false).ToList();
            }
            ViewBag.genre = genre;
            return View();
        }
        public ActionResult AddGenre(string name)
        {
            var genreExit = context.Genres.FirstOrDefault(x => x.Name == name);
            if (genreExit == null) 
            { 
                Genre genre = new Genre();
                genre.Name = name;
                genre.CreatedAt = DateTime.Now;
                genre.UpdatedAt = DateTime.Now;
                genre.DeleteFlag = false;
                context.Genres.Add(genre);
                context.SaveChanges();
                TempData["message"] = "add successfull";
            }
            else {
                if (genreExit.DeleteFlag) 
                { 
                    genreExit.DeleteFlag = false ;
                    var bookList = context.Books.Where(b => b.GenreId == genreExit.Id).ToList();
                    foreach (var book in bookList) 
                    { 
                        book.DeleteFlag = false;
                        context.Books.Update(book);
                        context.SaveChanges();
                    }
                    context.Genres.Update(genreExit);
                    context.SaveChanges();
                    TempData["message"] = "add successfull";
                }
                else
                {
                    TempData["message"] = "add fail. Name Exit";
                }
                
            }

            return RedirectToAction("Genre");
        }

        public ActionResult DeleteGenre(int id)
        {
            var genre = context.Genres.FirstOrDefault(b => b.Id == id);
            if (genre != null) 
            {
                genre.DeleteFlag = true;
                var listBook = context.Books.Where(b => b.GenreId ==  id).ToList();
                foreach(var book in listBook)
                {
                    book.DeleteFlag = true;
                    context.Books.Update(book);
                    context.SaveChanges();
                }
                context.Genres.Update(genre);
                context.SaveChanges();
                TempData["message"] = "delete successfull";
            }
            else { TempData["message"] = "delete fail. genre not exit"; }
            return RedirectToAction("Genre");
        }
    }
}
