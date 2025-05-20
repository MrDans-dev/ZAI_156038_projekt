using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using TomatisCRM_API.Entities;

namespace TomatisCRM_API.Models.Entities;

public partial class Klient
{
    public int KntId { get; set; }

    [JsonPropertyName("KntAkronim")]
    public string KntAkronim { get; set; } = null!;
    [JsonPropertyName("KntNazwa")]
    public string? KntNazwa { get; set; }
    [JsonPropertyName("KntTel")]
    public string? KntTel { get; set; }
    [JsonPropertyName("KntEmail")]
    public string? KntEmail { get; set; }
    [JsonPropertyName("KntDataUtw")]
    public DateTime? KntDataUtw { get; set; } = DateTime.Now;
    [JsonPropertyName("KntOpeUtw")]
    public int? KntOpeUtw { get; set; }
    [JsonPropertyName("KntOpeMod")]
    public int? KntOpeMod { get; set; }
    [JsonPropertyName("KntDataMod")]
    public DateTime? KntDataMod { get; set; } = DateTime.Now;
    [JsonPropertyName("KntDataUrodzenia")]
    public DateTime? KntDataUrodzenia { get; set; }
    [JsonPropertyName("KntStatusSlw")]
    public int? KntStatusSlw { get; set; }
    [JsonPropertyName("KntStacjonarny")]
    public bool? KntStacjonarny { get; set; }
    [JsonPropertyName("KntOpis")]
    public string? KntOpis { get; set; }
    [JsonPropertyName("KntDoKontaktu")]
    public bool? KntDoKontaktu { get; set; } = false;
    [JsonPropertyName("KntDataKontaktu")]
    public DateTime? KntDataKontaktu { get; set; }
    [JsonPropertyName("KntOpeUNazwa")]
    public string? KntOpeUNazwa { get; set; }
    [JsonPropertyName("KntStatus")]
    public string? KntStatus { get; set; }
    [JsonIgnore]
    public virtual Operatorzy? KntOpeModNavigation { get; set; }
    [JsonIgnore]
    public virtual Operatorzy? KntOpeUtwNavigation { get; set; }
    [JsonIgnore]
    public virtual Slowniki? KntStatusSlwNavigation { get; set; }

    [JsonIgnore]
    public virtual ICollection<Wizyty> Wizyties { get; set; } = new List<Wizyty>();
    [JsonIgnore]
    public virtual ICollection<Pliki> Plikis { get; set; } = new List<Pliki>();
}
