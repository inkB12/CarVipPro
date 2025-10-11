using System;
using System.Collections.Generic;

namespace CarVipPro.DAL.Entities;

public partial class Feedback
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public string FeedbackType { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime DateTime { get; set; }

    public bool IsActive { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
