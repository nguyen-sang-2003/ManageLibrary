using Humanizer.Localisation;
using ManageLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ManageLibrary.Controllers
{
    public class AdminController : Controller
    {
        private readonly ManagerLibraryContext context = new ManagerLibraryContext();
        public IActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Product(string title, int? authorId, int? genreId, int? indexPage) 
        {
            var author = context.Authors.Where(au => au.DeleteFlag == false).ToList();
            ViewBag.AuthorList = author;
            var genre = context.Genres.Where(g => g.DeleteFlag == false).ToList();
            ViewBag.GenreList = genre;

            if (string.IsNullOrEmpty(title)) title = "";
            var book = context.Books.Where(b => b.DeleteFlag==false && b.Title.Contains(title)).ToList();

            if (authorId != null)
            {
                book = book.Where(b => b.AuthorId == authorId).ToList();
            }
            
            if (genreId != null)
            {
                book = book.Where(b => b.GenreId == genreId).ToList();
            }

            foreach(var b in book)
            {
                b.Author = context.Authors.FirstOrDefault(a => a.DeleteFlag == false && a.Id == b.AuthorId);
                b.Genre = context.Genres.FirstOrDefault(g => g.DeleteFlag == false && g.Id == b.GenreId);
            }

            int pageSize = 3;
            if (indexPage == null) { indexPage = 0; }
            var bookPage = book.Skip((int)indexPage * pageSize)
                           .Take(pageSize)
                           .ToList();

            int totalBooks = book.Count();
            int totalPages = (int)Math.Ceiling((double)totalBooks / pageSize);

            ViewBag.CurrentPage = indexPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            ViewBag.Book = bookPage;

            ViewBag.SearchName = title;
            ViewBag.SelectedAuthorId = authorId;
            ViewBag.SelectedGenreId = genreId;
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

            var rating = context.Ratings.Where(r => r.DeleteFlag == false && r.BookId == id).ToList();

            int totalRating = rating.Count;
            double avgRating = 0;
            if (totalRating > 0)
            {
                avgRating = (double)rating.Sum(r => r.Star) / (double)totalRating;
            }

            ViewBag.TotalRating = totalRating;
            ViewBag.AvgRating = avgRating;

            ViewBag.BookDetail = book;
            return View();
        }
        public ActionResult UpdateBook(int id)
        {
            var authorList = context.Authors.Where(a => a.DeleteFlag == false).ToList();
            ViewBag.authorList = authorList;
            var genreList = context.Genres.Where(g => g.DeleteFlag == false).ToList();
            ViewBag.genreList = genreList;

            var book = context.Books.FirstOrDefault(b => b.Id==id && b.DeleteFlag == false);
            if(book==null)
            {
                ViewBag.message = "not found book.";
                return View();
            }
            ViewBag.book = book;
            return View();
        }
        [HttpPost]
        public ActionResult UpdateBook(Book book)
        {
            var bookUpdate = context.Books.FirstOrDefault(b => b.Id==book.Id && b.DeleteFlag==false);
            var bookExit = context.Books.FirstOrDefault(b => b.Title.Equals(book.Title) && b.Id!=book.Id);
            if(bookExit==null)
            {
                if (bookUpdate != null)
                {
                    bookUpdate.Title = book.Title;
                    bookUpdate.Image = book.Image;
                    bookUpdate.Subtitle = book.Subtitle;
                    bookUpdate.AuthorId = book.AuthorId;
                    bookUpdate.GenreId = book.GenreId;
                    bookUpdate.PublishingYear = book.PublishingYear;
                    bookUpdate.QuantityInStock = book.QuantityInStock;
                    bookUpdate.Description = book.Description;
                    bookUpdate.UpdatedAt = DateTime.Now;

                    context.Books.Update(bookUpdate);
                    context.SaveChanges();
                    ViewBag.message = "update successfull";

                }
                else
                {
                    ViewBag.message = "book not found.";
                }

            }
            else
            {
                ViewBag.message = "book exit.";
            }
            return View();
        }
        public ActionResult Author(string name,int? indexPage)
        {
            
            if (string.IsNullOrEmpty(name)) name = "";
            var author = context.Authors.Where(au => au.DeleteFlag == false && au.Name.Contains(name)).ToList();
            foreach (var a in author)
            {
                a.Books = context.Books.Where(b => b.AuthorId == a.Id && b.DeleteFlag==false).ToList();
            }

            
            int pageSize = 2;
            if (indexPage == null) { indexPage = 0; }
            var authorPage = author.Skip((int)indexPage * pageSize)
                           .Take(pageSize)
                           .ToList();

            int totalAuthors = author.Count();
            int totalPages = (int)Math.Ceiling((double)totalAuthors / pageSize);

            ViewBag.CurrentPage = indexPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            ViewBag.author = authorPage;

            ViewBag.SearchName = name;
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

        public ActionResult Genre(string name, int? indexPage)
        {
            if (string.IsNullOrEmpty(name)) name = "";
            var genre = context.Genres.Where(g => g.DeleteFlag==false && g.Name.Contains(name)).ToList();
            foreach (var a in genre)
            {
                a.Books = context.Books.Where(b => b.GenreId == a.Id && b.DeleteFlag == false).ToList();
            }

            int pageSize = 2;
            if (indexPage == null) { indexPage = 0; }
            var genrePage = genre.Skip((int)indexPage * pageSize)
                           .Take(pageSize)
                           .ToList();

            int totalGenres = genre.Count();
            int totalPages = (int)Math.Ceiling((double)totalGenres / pageSize);

            ViewBag.CurrentPage = indexPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            ViewBag.genre = genrePage;
            ViewBag.SearchName = name;
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
        
        public ActionResult Borrowing(string userName, int? indexPage)
        {
            var userList =context.Users.Where(u => u.DeleteFlag==false).ToList();
            ViewBag.UserList = userList;

            var borrow = context.Borrowings.Where(b=>b.DeleteFlag==false).ToList();
            foreach(var b in borrow)
            {
                b.User = context.Users.FirstOrDefault(u => u.Id == b.UserId && u.DeleteFlag==false);
                b.BorrowingItems = context.BorrowingItems
                                .Where(bi => bi.BorrowingId == b.Id && bi.DeleteFlag==false)
                                .ToList();
                foreach(var borItem in b.BorrowingItems)
                {
                    borItem.Book = context.Books.FirstOrDefault(bo => bo.Id == borItem.BookId);
                }
            }
            if (string.IsNullOrEmpty(userName)) userName = "";
            borrow = borrow.Where(b => b.User.Name.Contains(userName)).ToList();

            int pageSize = 4;
            if (indexPage == null) { indexPage = 0; }
            var borrowPage = borrow.Skip((int)indexPage * pageSize)
                           .Take(pageSize)
                           .ToList();

            int totalBorrows = borrow.Count();
            int totalPages = (int)Math.Ceiling((double)totalBorrows / pageSize);

            ViewBag.CurrentPage = indexPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            ViewBag.BorrowList = borrowPage;

            ViewBag.SearchUserName = userName;
            return View();
        }
        public ActionResult ButtonReturnBook(int borrowingId)
        {
            var borrowing = context.Borrowings.FirstOrDefault(b => b.DeleteFlag == false && b.Id == borrowingId);
            if (borrowing == null)
            {
                TempData["message"] = "Borrowing record not found.";
                return RedirectToAction("Borrowing");
            }
            borrowing.ActualEndAt = DateTime.Now;
            context.Borrowings.Update(borrowing);
            context.SaveChanges();
            TempData["message"] = "return book successful.";
            var borrowingItem = context.BorrowingItems.Where(bi => bi.DeleteFlag==false && bi.BorrowingId == borrowing.Id).ToList();
            foreach(var item in borrowingItem)
            {
                var book = context.Books.FirstOrDefault(b => b.DeleteFlag == false && b.Id == item.BookId);
                book.QuantityInStock = book.QuantityInStock + 1;
                context.Books.Update(book);
                context.SaveChanges();
            }
            return RedirectToAction("Borrowing");
        }
        
        public ActionResult Rating(string title, int? userId, int? indexPage)
        {
            var userList = context.Users.Where(u => u.DeleteFlag == false).ToList();
            ViewBag.UserList = userList;
            
            var rating = context.Ratings.Where(r => r.DeleteFlag == false).ToList();
            foreach(var r in rating)
            {
                r.Book = context.Books.FirstOrDefault(b => b.DeleteFlag == false && b.Id == r.BookId);
                r.User = context.Users.FirstOrDefault(u => u.DeleteFlag == false && u.Id == r.UserId);
            }
            if (string.IsNullOrEmpty(title)) title = "";
            rating = rating.Where(r => r.Book.Title.Contains(title.Trim())).ToList();
            if (userId != null) rating = rating.Where(r => r.UserId == userId).ToList();

            int pageSize = 3;
            if (indexPage == null) { indexPage = 0; }
            var ratingPage = rating.Skip((int)indexPage * pageSize)
                           .Take(pageSize)
                           .ToList();

            int totalRatings = rating.Count();
            int totalPages = (int)Math.Ceiling((double)totalRatings / pageSize);

            ViewBag.CurrentPage = indexPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;


            ViewBag.Rating = ratingPage;

            ViewBag.SearchName = title;
            ViewBag.SelectedUserId = userId;
            return View();
        }
        public ActionResult DeleteRating(int ratingId)
        {
            var rating = context.Ratings.FirstOrDefault(r => r.Id == ratingId && r.DeleteFlag == false);
            if(rating != null) 
            {
                rating.DeleteFlag = true;
                context.Ratings.Update(rating);
                context.SaveChanges();
                TempData["message"] = "return book successful.";
            }
            else
            {
                TempData["message"] = "Rating record not found.";
            }
            return RedirectToAction("Rating");
        }
    }
}
