using System;
using System.Collections.Generic;

namespace RoomRentalManagementSolution.Models;

public partial class Area
{
    public int Id { get; set; }

    public decimal Area1 { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
