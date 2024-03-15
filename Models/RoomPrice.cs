using System;
using System.Collections.Generic;

namespace RoomRentalManagementSolution.Models;

public partial class RoomPrice
{
    public int Id { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
