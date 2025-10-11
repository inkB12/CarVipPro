using System;
using System.Collections.Generic;

namespace CarVipPro.DAL.Entities;

public partial class Customer
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string FullName { get; set; } = null!;

    public string? IdentityCard { get; set; }

    public string? Address { get; set; }

    public string? ZipCode { get; set; }

    public virtual ICollection<DriveSchedule> DriveSchedules { get; set; } = new List<DriveSchedule>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
