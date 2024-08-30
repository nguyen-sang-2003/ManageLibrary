using ManageLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Net;
using static System.Reflection.Metadata.BlobBuilder;

namespace ManageLibrary.Controllers
{
    public class HomeController : Controller
    {
        private readonly ManagerLibraryContext context = new ManagerLibraryContext();
        public ActionResult Index(string? title)
        {
            var book = context.Books.ToList();
            if(!string.IsNullOrEmpty(title)) book = book.Where(b => b.Title.Contains(title)).ToList();
            ViewBag.Book = book;
            return View();
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
            borrow.ActualEndAt = DateTime.Now.AddDays(7);
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
        
        public ActionResult Borrowing()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                ViewBag.message = "Can't view history. Please Login.";
                return View();
            }
            var userId = (int)HttpContext.Session.GetInt32("UserId");
            var borrow = context.Borrowings.Where(b => b.UserId == userId).ToList();
            if (borrow == null) 
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
            ViewBag.ListItems = borrow;
            return View();
        }
        
    }
}
