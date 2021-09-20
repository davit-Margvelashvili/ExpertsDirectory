using System.Collections.Generic;

namespace ExpertsDirectory.Models.Members
{
    /// <summary>
    /// Member of this system who is expert and can be found by others
    /// </summary>
    public sealed class Member
    {
        /// <summary>
        /// Member's unique identificator
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Fullname
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Personal website
        /// </summary>
        public string WebSite { get; set; }

        /// <summary>
        /// Specialization tags, which can be used for finding this member
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Friends of this member
        /// </summary>
        public List<Member>? Friends { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public Member(int id, string name, string webSite, string tags, List<Member>? friends)
        {
            Id = id;
            Name = name;
            WebSite = webSite;
            Friends = friends;
            Tags = tags;
        }
    }
}