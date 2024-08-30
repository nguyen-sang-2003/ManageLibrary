using System;
using System.Collections.Generic;

namespace ManageLibrary.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Image { get; set; }

    public string? Subtitle { get; set; }

    public int? AuthorId { get; set; }

    public int? GenreId { get; set; }

    public int? PublishingYear { get; set; }

    public int QuantityInStock { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool DeleteFlag { get; set; }

    public virtual Author? Author { get; set; }

    public virtual ICollection<BorrowingItem> BorrowingItems { get; set; } = new List<BorrowingItem>();

    public virtual Genre? Genre { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
