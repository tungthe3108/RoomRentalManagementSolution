using System;
using System.Collections.Generic;

namespace RoomRentalManagementSolution.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public int? RoomId { get; set; }

    public string Name { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public DateTime Dob { get; set; }

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual Room? Room { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    public virtual ICollection<Room> RoomsNavigation { get; set; } = new List<Room>();
}
