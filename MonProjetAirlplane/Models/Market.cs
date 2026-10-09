using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonProjetAirlplane.Models
{
    [Table("Market")]
    public class Market
    {
        [Key]
        public string Escale_D { get; set; } = string.Empty; // Code escale (ex: ABJ)
        public string? ZoneGeo { get; set; }
        public string? ZoneGeoArabe { get; set; }
        public string? Pays { get; set; }
        public string? PaysArabe { get; set; }
        public string? PaysArRapport { get; set; }

        // Relation Navigation
        public ICollection<MarketDetails> MarketDetails { get; set; } = new List<MarketDetails>();
    }
}