using ManageLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.Net;
using static System.Reflection.Metadata.BlobBuilder;

namespace ManageLibrary.Controllers
{
    public class HomeController : Controller
    {
        private readonly ManagerLibraryContext context = new ManagerLibraryContext();
        public ActionResult Index(string? title, int? authorId, int? genreId, int? indexPage)
        {
            var author = context.Authors.Where(au => au.DeleteFlag == false).ToList();
            ViewBag.AuthorList = author;
            var genre = context.Genres.Where(g => g.DeleteFlag == false).ToList();
            ViewBag.GenreList = genre;

            var book = context.Books.Where(b => b.DeleteFlag == false && b.QuantityInStock > 0).ToList();
            if(!string.IsNullOrEmpty(title)) book = book.Where(b => b.Title.Contains(title)).ToList();
            if (authorId != null) book = book.Where(b => b.AuthorId == authorId).ToList();
            if (genreId != null)  book = book.Where(b => b.GenreId == genreId).ToList();
            foreach (var b in book)
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
        public ActionResult DetailBook(int id)
        {
            var book = context.Books.FirstOrDefault(x => x.Id == id);

            if (book == null)
            {
                return RedirectToAction("Index");
            }

            var rating = context.Ratings.Where(r => r.DeleteFlag==false && r.BookId==id).ToList();
            
            int totalRating = rating.Count;
            double avgRating = 0;
            if(totalRating > 0)
            {
                avgRating = (double)rating.Sum(r => r.Star) / (double)totalRating;
            }

            ViewBag.TotalRating = totalRating;
            ViewBag.AvgRating = avgRating;

            ViewBag.BookDetail = book;
            return View();
        }
        public ActionResult Borrow(List<int> selectedBooks) 
        {
            if (selectedBooks == null || !selectedBooks.Any())
            {
                TempData["message"] = "No books selected.";
                return RedirectToAction("Index");
            }

            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["message"] = "Can't Borrow. Please Login.";
                return RedirectToAction("Index");
            }

            Borrowing borrow = new Borrowing();
            borrow.UserId = (int)HttpContext.Session.GetInt32("UserId");
            borrow.StartAt = DateTime.Now;
            borrow.EndAt = DateTime.Now.AddDays(7);
            //borrow.ActualEndAt = DateTime.Now.AddDays(7);
            borrow.CreatedAt = DateTime.Now;
            borrow.UpdatedAt = DateTime.Now;
            borrow.DeleteFlag = false;
            context.Borrowings.Add(borrow);
            context.SaveChanges();

            foreach (var bookId in selectedBooks)
            {
                var book = context.Books.FirstOrDefault(x => x.Id == bookId);
                if (book == null) 
                {
                    TempData["message"] = "Fail no item.";
                    return RedirectToAction("Index");
                }
                BorrowingItem borrowingItem = new BorrowingItem();
                borrowingItem.BookId = bookId;
                borrowingItem.BorrowingId = borrow.Id;
                borrowingItem.Quantity = 1;
                borrowingItem.CreatedAt = DateTime.Now;
                borrowingItem.UpdatedAt = DateTime.Now;
                borrowingItem.DeleteFlag = false;
                context.BorrowingItems.Add(borrowingItem);
                context.SaveChanges();
            }


            TempData["message"] = "Borrowing successful.";
            return RedirectToAction("Index");
        }
        
        public ActionResult Borrowing(string title, int? indexPage)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                ViewBag.message = "Can't view history. Please Login.";
                return View();
            }
            var userId = (int)HttpContext.Session.GetInt32("UserId");
            var borrow = context.Borrowings.Where(b => b.UserId == userId).ToList();
            if (borrow == null || borrow.Count == 0) 
            {
                ViewBag.message = "chưa từng mượn sách";
                return View();
            }
            foreach (var b in borrow) 
            { 
                b.BorrowingItems = context.BorrowingItems.Where(bi =>  bi.BorrowingId == b.Id).ToList();
                foreach (var bi in b.BorrowingItems) 
                {
                    bi.Book = context.Books.FirstOrDefault(b => b.Id == bi.BookId);
                }
            }

            if (string.IsNullOrEmpty(title)) title = "";
            borrow = borrow.Where(b => b.BorrowingItems.Any(bi => bi.Book.Title.Contains(title.Trim()))).ToList();

            int pageSize = 3;
            if (indexPage == null) { indexPage = 0; }
            var borrowPage = borrow.Skip((int)indexPage * pageSize)
                           .Take(pageSize)
                           .ToList();

            int totalBorrows = borrow.Count();
            int totalPages = (int)Math.Ceiling((double)totalBorrows / pageSize);

            ViewBag.CurrentPage = indexPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            ViewBag.ListItems = borrowPage;

            ViewBag.SearchUserName = title;
            return View();
        }
        
    }
}
