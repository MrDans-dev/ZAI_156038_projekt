using System;
using System.Collections.Generic;

namespace TomatisCRM_API.Models.Entities;

public partial class AppConf
{
    public int SconfId { get; set; }

    public string SconfNazwa { get; set; } = null!;

    public string? SconfWartoscS { get; set; }

    public decimal? SconfWartoscD { get; set; }

    public DateTime? SconfWartoscDt { get; set; }
}
