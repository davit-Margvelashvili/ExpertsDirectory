using ExpertsDirectory.Models.Members;
using ExpertsDirectory.Models.Members.Commands;
using ExpertsDirectory.Service.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace ExpertsDirectory.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MembersController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IMemberService _memberService;
        private readonly ILogger<MembersController> _logger;

        public MembersController(IMemberService memberService, ILogger<MembersController> logger)
        {
            _memberService = memberService;
            _logger = logger;
        }

        [HttpGet("List")]
        public async Task<ActionResult<List<Member>>> ListAsync(CancellationToken cancellationToken) =>
            Ok(await _memberService.ListMembersAsync(cancellationToken));

        [HttpGet("List/{tag}")]
        public async Task<ActionResult<List<Member>>> ListAsync([Required, FromQuery] int memberId, [FromRoute] string tag, CancellationToken cancellationToken) =>
            Ok(await _memberService.ListMembersAsync(memberId, tag, cancellationToken));

        [HttpGet("{memberId}")]
        public async Task<ActionResult<Member>> GetDetailsAsync([FromRoute] int memberId, CancellationToken cancellationToken)
        {
            var member = await _memberService.GetMemberDetailsAsync(memberId, cancellationToken);
            return member is null ? NotFound($"Member with id: '{memberId}' not found") : Ok(member);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateMember command, CancellationToken cancellationToken)
        {
            var newMember = await _memberService.CreateMemberAsync(command, cancellationToken);
            return Created($"Members/{newMember.Id}", newMember);
        }

        [HttpPost("DefineFriendship")]
        public async Task<IActionResult> DefineFriendshipAsync([FromBody] DefineFriendship command, CancellationToken cancellationToken)
        {
            await _memberService.DefineFriendshipAsync(command, cancellationToken);
            return Ok();
        }
    }
}