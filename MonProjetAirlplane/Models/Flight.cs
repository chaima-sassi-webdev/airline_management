using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonProjetAirlplane.Models
{
    [Table("Flights")]
    public class Flight
    {
        [Key]
        public int Id { get; set; }

        // Propriétés réelles (liées à la base de données)
        public string? FN_CARRIER { get; set; }
        public string? FN_NUMBER { get; set; }
        public string? AC_REGISTRATION { get; set; }
        public string? DAY_OF_ORIGIN { get; set; }
        public string? ARR_AP_ACTUAL { get; set; }
        public int PAX_FLOWN_C { get; set; }
        public int PAX_FLOWN_Y { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal AVG_FARE { get; set; }

        // --- ALIAS POUR LES VUES ---
        [NotMapped]
        public string Vol 
        { 
            get => $"{FN_CARRIER} {FN_NUMBER}"; 
            set 
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var parts = value.Split(' ');
                    if (parts.Length > 0) FN_CARRIER = parts[0];
                    if (parts.Length > 1) FN_NUMBER = parts[1];
                }
            } 
        }

        [NotMapped]
        public string Immatriculation 
        { 
            get => AC_REGISTRATION ?? string.Empty; 
            set => AC_REGISTRATION = value; 
        }

        [NotMapped]
        public string Date 
        { 
            get => DAY_OF_ORIGIN ?? string.Empty; 
            set => DAY_OF_ORIGIN = value; 
        }

        [NotMapped]
        public string ArrivalEscale 
        { 
            get => ARR_AP_ACTUAL ?? string.Empty; 
            set => ARR_AP_ACTUAL = value; 
        }

        [NotMapped]
        public int PaxClub 
        { 
            get => PAX_FLOWN_C; 
            set => PAX_FLOWN_C = value; 
        }

        [NotMapped]
        public int PaxEco 
        { 
            get => PAX_FLOWN_Y; 
            set => PAX_FLOWN_Y = value; 
        }

        [NotMapped]
        public decimal TarifMoyen 
        { 
            get => AVG_FARE; 
            set => AVG_FARE = value; 
        }

    }
}