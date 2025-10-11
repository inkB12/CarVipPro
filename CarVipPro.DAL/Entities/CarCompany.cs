﻿using System;
using System.Collections.Generic;

namespace CarVipPro.DAL.Entities;

public partial class CarCompany
{
    public int Id { get; set; }

    public string CatalogName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<ElectricVehicle> ElectricVehicles { get; set; } = new List<ElectricVehicle>();
}
