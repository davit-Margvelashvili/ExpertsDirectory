using ExpertsDirectory.Database;
using ExpertsDirectory.Models.Exceptions;
using ExpertsDirectory.Models.Members.Commands;
using ExpertsDirectory.Service.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Member = ExpertsDirectory.Models.Members.Member;

namespace ExpertsDirectory.Service.Implementations
{
    internal sealed class MemberService : IMemberService
    {
        private readonly ExpertsDirectoryContext _dbContext;
        private readonly IWebSiteParser _webSiteParser;

        public MemberService(ExpertsDirectoryContext dbContext, IWebSiteParser webSiteParser)
        {
            _dbContext = dbContext;
            _webSiteParser = webSiteParser;
        }

        public async Task<Member> CreateMemberAsync(CreateMember command, CancellationToken cancellationToken)
        {
            var tags = await _webSiteParser.GetTagsAsync(command.WebSite, cancellationToken);

            var newMember = new Database.Models.Member
            {
                Name = command.Name,
                WebSite = command.WebSite,
                Tags = string.Join(',', tags)
            };

            await _dbContext.Members.AddAsync(newMember, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new Member(newMember.Id, newMember.Name, newMember.WebSite, newMember.Tags, null);
        }

        public async Task DefineFriendshipAsync(DefineFriendship command, CancellationToken cancellationToken)
        {
            command.Validate();

            var members = _dbContext
                .Members
                .Where(m => m.Id == command.ThisMemberId || m.Id == command.OtherMemberId)
                .ToList();

            if (members.Count != 2)
                throw new DomainException($"One of the member could not be found. Member Ids: '{command.ThisMemberId}, {command.OtherMemberId}'");

            var first = members[0];
            var second = members[1];

            var firstFriends = first.Friends.FromJson<List<int>>() ?? new List<int>(1);
            var secondFriends = second.Friends.FromJson<List<int>>() ?? new List<int>(1);

            if (firstFriends.Exists(id => id == command.ThisMemberId || id == command.OtherMemberId)
                || secondFriends.Exists(id => id == command.ThisMemberId || id == command.OtherMemberId))
            {
                return;
            }

            if (first.Id == command.OtherMemberId)
                (firstFriends, secondFriends) = (secondFriends, firstFriends);

            firstFriends.Add(command.OtherMemberId);
            secondFriends.Add(command.ThisMemberId);

            first.Friends = firstFriends.ToJson();
            second.Friends = secondFriends.ToJson();

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task<List<Member>> ListMembersAsync(CancellationToken cancellationToken)
        {
            return _dbContext
                .Members
                .AsNoTracking()
                .Select(m => new Member(m.Id, m.Name, m.WebSite, m.Tags, null))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Member>> ListMembersAsync(int memberId, string tag, CancellationToken cancellationToken)
        {
            tag = $"%{tag}%";

            const string sql =
@"SELECT Id,[Name],WebSite,Tags
FROM dbo.Members
WHERE Id <> {0}
	AND  {0} NOT IN (SELECT VALUE FROM OPENJSON(Friends,'$'))
	AND Tags LIKE {1}
";
            return await _dbContext
                .Members
                .FromSqlRaw(sql, memberId, tag)
                .Select(m => new Member(m.Id, m.Name, m.WebSite, m.Tags, null))
                .ToListAsync(cancellationToken);
        }

        public async Task<Member?> GetMemberDetailsAsync(int id, CancellationToken cancellationToken)
        {
            var member = await _dbContext.Members.FindAsync(id);

            if (member is null)
                return null;

            if (member.Friends is null)
                return Map(member);

            var friendIds = $"({string.Join(',', member.Friends.FromJson<List<int>>().Select(m => m.ToString()))})";

            var friends = _dbContext
                .Members
                .FromSqlRaw($"SELECT Id,[Name],WebSite,Tags FROM Members WHERE Id IN {friendIds}")
                .Select(m => new Member(m.Id, m.Name, m.WebSite, m.Tags, null))
                .ToList();

            return Map(member, friends);
        }

        private static Member Map(Database.Models.Member member) => new(member.Id, member.Name, member.WebSite, member.Tags, null);

        private static Member Map(Database.Models.Member member, List<Member> friends) => new(member.Id, member.Name, member.WebSite, member.Tags, friends);
    }
}