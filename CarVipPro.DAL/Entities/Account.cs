using System;
using System.Collections.Generic;

namespace CarVipPro.DAL.Entities;

public partial class Account
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string FullName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<DriveSchedule> DriveSchedules { get; set; } = new List<DriveSchedule>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
