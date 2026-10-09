using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonProjetAirlplane.Models
{
    public class MarketDetails
    {
        [Key]
        public string ID { get; set; } = string.Empty; // ex: 04092023YULTUN
        public string? FN_CARRIER { get; set; } // ex: TU
        public int FN_NUMBER { get; set; } // ex: 203
        public string? ARR_AP_ACTUAL { get; set; } // Escale d'arrivée
        public int PAX_BOOKED_C { get; set; }
        public int PAX_BOOKED_Y { get; set; }
        public string? Escalestat { get; set; }
        public decimal AVG_FARE { get; set; } // Tarif moyen
        public string AC_REGISTRATION { get; set; }
        public string DAY_OF_ORIGIN { get; set; }
        public int PAX_FLOWN_C { get; set; }
        public int PAX_FLOWN_Y { get; set; }
        // Clé étrangère vers Market (basée sur l'escale d'arrivée ou d'origine)
        [ForeignKey("Market")]
        public string? EscaleDKey { get; set; }
        public Market? Market { get; set; }
    }
}