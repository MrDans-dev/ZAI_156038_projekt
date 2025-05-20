using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TomatisCRM_API.Models.Entities;

public partial class AppConf
{
    public int SconfId { get; set; }
    [JsonPropertyName("SconfNazwa")]
    public string SconfNazwa { get; set; } = null!;
    [JsonPropertyName("SconfWartoscS")]
    public string? SconfWartoscS { get; set; }
    [JsonPropertyName("SconfWartoscD")]
    public decimal? SconfWartoscD { get; set; }
    [JsonPropertyName("SconfWartoscDt")]
    public DateTime? SconfWartoscDt { get; set; }
}
