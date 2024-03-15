using System;
using System.Collections.Generic;

namespace RoomRentalManagementSolution.Models;

public partial class Contract
{
    public int ContractId { get; set; }

    public int RoomId { get; set; }

    public int CustomerId { get; set; }

    public DateTime DateOfHire { get; set; }

    public DateTime DateOfExpiration { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}
