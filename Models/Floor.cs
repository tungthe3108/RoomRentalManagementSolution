using System;
using System.Collections.Generic;

namespace RoomRentalManagementSolution.Models;

public partial class Floor
{
    public int Id { get; set; }

    public int Floor1 { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
