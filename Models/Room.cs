using System;
using System.Collections.Generic;

namespace RoomRentalManagementSolution.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string RoomName { get; set; } = null!;

    public int FloorId { get; set; }

    public int NumOfPersonId { get; set; }

    public int NumOfLiving { get; set; }

    public int AreaId { get; set; }

    public int PriceId { get; set; }

    public virtual Area Area { get; set; } = null!;

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual Floor Floor { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual NumOfPerson NumOfPerson { get; set; } = null!;

    public virtual RoomPrice Price { get; set; } = null!;

    public virtual ICollection<Customer> Customers1 { get; set; } = new List<Customer>();

    public virtual ICollection<Customer> CustomersNavigation { get; set; } = new List<Customer>();
}
