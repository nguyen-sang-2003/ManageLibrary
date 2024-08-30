using System;
using System.Collections.Generic;

namespace ManageLibrary.Models;

public partial class Borrowing
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    public DateTime ActualEndAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool DeleteFlag { get; set; }

    public virtual ICollection<BorrowingItem> BorrowingItems { get; set; } = new List<BorrowingItem>();

    public virtual User User { get; set; } = null!;
}
