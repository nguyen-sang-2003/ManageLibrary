using System;
using System.Collections.Generic;

namespace ManageLibrary.Models;

public partial class BorrowingItem
{
    public int Id { get; set; }

    public int BorrowingId { get; set; }

    public int BookId { get; set; }

    public int Quantity { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool DeleteFlag { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Borrowing Borrowing { get; set; } = null!;
}
