using System;
using System.Collections.Generic;

namespace ManageLibrary.Models;

public partial class Rating
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public int UserId { get; set; }

    public int Star { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool DeleteFlag { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
