using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TomatisCRM_API.Models.Entities;

public partial class Slowniki
{
    public int SlwId { get; set; }

    [JsonPropertyName("SlwTyp")]
    public int? SlwTyp { get; set; }
    [JsonPropertyName("SlwWartoscS")]
    public string? SlwWartoscS { get; set; }
    [JsonPropertyName("SlwWartoscD")]
    public decimal? SlwWartoscD { get; set; }
    [JsonPropertyName("SlwOpeUtw")]
    public int? SlwOpeUtw { get; set; }
    [JsonPropertyName("SlwDataUtw")]
    public DateTime? SlwDataUtw { get; set; } = DateTime.Now;
    [JsonPropertyName("SlwOpeMod")]
    public int? SlwOpeMod { get; set; }
    [JsonPropertyName("SlwDataMod")]
    public DateTime? SlwDataMod { get; set; } = DateTime.Now;
    [JsonPropertyName("SlwNextSlw")]
    public int? SlwNextSlw { get; set; }
    [JsonPropertyName("SlwOpis")]
    public string? SlwOpis { get; set; }
    [JsonPropertyName("OpeNazwa")]
    public string? OpeNazwa { get; set; }

    [JsonIgnore]
    public virtual ICollection<Klient> Klients { get; set; } = new List<Klient>();
    [JsonIgnore]
    public virtual Operatorzy? SlwOpeUtwNavigation { get; set; }
    [JsonIgnore]
    public virtual ICollection<Wizyty> Wizyties { get; set; } = new List<Wizyty>();
}
