﻿namespace TomatisCRM_API.Models
{
    public class KlientModel
    {
        public int KntId { get; set; }

        public string KntAkronim { get; set; } = null!;

        public string? KntNazwa { get; set; }

        public string? KntTel { get; set; }

        public string? KntEmail { get; set; }

        public string? KntOpeUNazwa { get; set; }

        public DateTime? KntDataUtw { get; set; }

        public DateTime? KntDataUrodzenia { get; set; }

        public bool? KntStacjonarny { get; set; }

        public string? KntOpis { get; set; }

        public bool? KntDoKontaktu { get; set; }

        public string? KntGrupaWiekowa { get; set; }

        public string? KntNextEtap { get; set; }

        public DateTime? KntDataKontaktu { get; set; }

        public string? KntStatus { get; set; }
    }
}
