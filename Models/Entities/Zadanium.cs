using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TomatisCRM_API.Models.Entities;

public partial class Zadanium
{
    public int ZadId { get; set; }
    [JsonPropertyName("ZadNazwa")]
    public string? ZadNazwa { get; set; }
    [JsonPropertyName("ZadOpis")]
    public string? ZadOpis { get; set; }
    [JsonPropertyName("ZadDataUkonczenia")]
    public DateTime? ZadDataUkonczenia { get; set; }
    [JsonPropertyName("ZadOpeid")]
    public int? ZadOpeid { get; set; }
}
