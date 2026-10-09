using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonProjetAirlplane.Models
{
    [Table("Contracts")]
    public class ContractViewModel
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le titre du contrat est requis.")]
        [MaxLength(150)]
        [Display(Name = "Titre du Contrat")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom du fournisseur est requis.")]
        [MaxLength(150)]
        [Display(Name = "Fournisseur")]
        public string SupplierName { get; set; } = string.Empty; // Ou un SupplierId si vous souhaitez faire une relation

        [Required(ErrorMessage = "La date de début est requise.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date de début")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "La date de fin est requise.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date de fin")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Le montant est requis.")]
        [Column(TypeName = "decimal(18,2)]")]
        [Display(Name = "Montant (TND / USD)")]
        public decimal Amount { get; set; }

        [MaxLength(50)]
        [Display(Name = "Statut")]
        public string Status { get; set; } = "Actif"; // Actif, Expiré, Résilié
    }
}