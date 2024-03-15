using System;
using System.Collections.Generic;

namespace RoomRentalManagementSolution.Models;

public partial class NumOfPerson
{
    public int Id { get; set; }

    public int NumOfPerson1 { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
