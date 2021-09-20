using System.ComponentModel.DataAnnotations;

namespace ExpertsDirectory.Database.Models
{
    public sealed class Member
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(64)]
        public string Name { get; set; } = default!;

        [Required(AllowEmptyStrings = false)]
        [StringLength(128)]
        public string WebSite { get; set; } = default!;

        /// <summary>
        /// Json representation of tags
        /// </summary>
        public string Tags { get; set; } = default!;

        /// <summary>
        /// Json representation of friends list
        /// </summary>
        public string? Friends { get; set; }
    }
}