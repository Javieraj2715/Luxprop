using System;
using System.Collections.Generic;

namespace Luxprop.Data.Data.Models;

public partial class PropertyTour360
{
    public int Id { get; set; }

    public int PropertyId { get; set; }

    public string TourUrl { get; set; } = null!;

    public string? Title { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual Propiedad Property { get; set; } = null!;
}
