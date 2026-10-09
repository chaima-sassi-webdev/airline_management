using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonProjetAirlplane.Models
{
    [Table("Delays")]
    public class DelayViewModel
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le numéro de vol est requis.")]
        [MaxLength(50)]
        [Display(Name = "Numéro de Vol")]
        public string FlightNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le retard en minutes est requis.")]
        [Display(Name = "Retard (minutes)")]
        public int DelayMinutes { get; set; }

        [Required(ErrorMessage = "La raison du retard est requise.")]
        [MaxLength(250)]
        [Display(Name = "Raison / Cause")]
        public string Reason { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date est requise.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date du retard")]
        public DateTime DelayDate { get; set; } = DateTime.Now;

        [MaxLength(50)]
        [Display(Name = "Statut")]
        public string Status { get; set; } = "En cours"; // En cours, Traité, Annulé
    }
}