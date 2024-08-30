using System;
using System.Collections.Generic;

namespace ManageLibrary.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool DeleteFlag { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
