using System;
using System.Collections.Generic;

namespace CarVipPro.DAL.Entities;

public partial class ElectricVehicle
{
    public int Id { get; set; }

    public int CarCompanyId { get; set; }

    public int CategoryId { get; set; }

    public string Model { get; set; } = null!;

    public string? Version { get; set; }

    public decimal Price { get; set; }

    public string? Specification { get; set; }

    public string? ImageUrl { get; set; }

    public string? Color { get; set; }

    public bool IsActive { get; set; }

    public virtual CarCompany CarCompany { get; set; } = null!;

    public virtual VehicleCategory Category { get; set; } = null!;

    public virtual ICollection<DriveSchedule> DriveSchedules { get; set; } = new List<DriveSchedule>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
