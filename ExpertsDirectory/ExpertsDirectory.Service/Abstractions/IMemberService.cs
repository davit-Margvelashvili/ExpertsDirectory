using ExpertsDirectory.Models.Members;
using ExpertsDirectory.Models.Members.Commands;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExpertsDirectory.Service.Abstractions
{
    public interface IMemberService
    {
        Task<Member> CreateMemberAsync(CreateMember command, CancellationToken cancellationToken);

        Task DefineFriendshipAsync(DefineFriendship command, CancellationToken cancellationToken);

        Task<List<Member>> ListMembersAsync(CancellationToken cancellationToken);

        Task<List<Member>> ListMembersAsync(int memberId, string tag, CancellationToken cancellationToken);

        Task<Member?> GetMemberDetailsAsync(int id, CancellationToken cancellationToken);
    }
}