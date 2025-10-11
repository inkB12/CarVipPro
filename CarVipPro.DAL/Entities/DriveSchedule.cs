using System;
using System.Collections.Generic;

namespace CarVipPro.DAL.Entities;

public partial class DriveSchedule
{
    public int Id { get; set; }

    public int ElectricVehicleId { get; set; }

    public int CustomerId { get; set; }

    public int AccountId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string Status { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ElectricVehicle ElectricVehicle { get; set; } = null!;
}
