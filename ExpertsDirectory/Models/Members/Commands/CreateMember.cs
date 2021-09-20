using System.ComponentModel.DataAnnotations;

namespace ExpertsDirectory.Models.Members.Commands
{
    public sealed class CreateMember
    {
        [Required(AllowEmptyStrings = false), StringLength(64)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false), Url]
        public string WebSite { get; set; }

        public CreateMember(string name, string webSite)
        {
            Name = name;
            WebSite = webSite;
        }
    }
}