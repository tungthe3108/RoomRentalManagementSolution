using System;
using System.Collections.Generic;

namespace RoomRentalManagementSolution.Models;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public int RoomId { get; set; }

    public int CustomerId { get; set; }

    public string InvoiceType { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public decimal Price { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}
