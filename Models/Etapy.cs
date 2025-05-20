using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TomatisCRM_API.Controllers;

namespace TomatisCRM_API.Models;

public partial class Etapy
{
    public int EtapId { get; set; }

    public string? EtapNazwa { get; set; }
    public string? EtapOpis { get; set; }

    public string? EtapNextNazwa { get; set; }
}
