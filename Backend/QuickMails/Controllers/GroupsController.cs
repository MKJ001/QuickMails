namespace QuickMails.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using Services.Models;

    [Route("api/groups")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IEmailGroupService emailGroupService;

        public GroupsController(IEmailGroupService emailGroupService)
        {
            this.emailGroupService = emailGroupService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NewEmailGroupDto dto)
        {
            var createdGroup = await this.emailGroupService.Create(dto);

            return this.Ok(createdGroup);
        }

        [HttpPut("{groupId}")]
        public async Task<IActionResult> Update(int groupId, [FromBody] NewEmailGroupDto dto)
        {
            var updatedGroup = await this.emailGroupService.Update(groupId, dto);

            return this.Ok(updatedGroup);
        }

        [HttpDelete("{groupId}")]
        public async Task<IActionResult> Delete(int groupId)
        {
            await this.emailGroupService.Delete(groupId);

            return this.NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var groups = await this.emailGroupService.GetAll();

            return this.Ok(groups);
        }
    }
}