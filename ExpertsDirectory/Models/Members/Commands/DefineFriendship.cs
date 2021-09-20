using ExpertsDirectory.Models.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace ExpertsDirectory.Models.Members.Commands
{
    /// <summary>
    /// Friendships are bi-directional i.e. If David is a friend of Oliver, Oliver is always a friend of David as well
    /// </summary>
    public sealed class DefineFriendship
    {
        /// <summary>
        /// Id of member
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int ThisMemberId { get; set; }

        /// <summary>
        /// Id of other member
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int OtherMemberId { get; set; }

        public void Validate()
        {
            if (ThisMemberId == OtherMemberId)
                throw new DomainException(
                    $"Cannot define friendship with himself. {nameof(ThisMemberId)}:{ThisMemberId}, {nameof(OtherMemberId)}:{OtherMemberId}");
        }
    }
}