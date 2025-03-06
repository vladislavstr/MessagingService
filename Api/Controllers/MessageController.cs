using Api.Controllers.Base;
using Domain.Models.Handlers.Commands.Message;
using Domain.Models.Handlers.Queries.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController(ISender sender) : BaseController<MessageController>(sender)
    {
        /// <summary>
        /// Create of message - add to data base
        /// </summary>
        /// <param name="createMessageCommand"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-message")]
        public async Task<IActionResult> CreateMessage
            (
                CreateMessageCommand createMessageCommand,
                CancellationToken cancellationToken = default
            ) => await Action(createMessageCommand, cancellationToken);

        /// <summary>
        /// Get messages for last 10 minutes
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetMessages
            (
                CancellationToken cancellationToken = default
            ) => await Action(new GetMessageQuery(), cancellationToken);
    }
}
