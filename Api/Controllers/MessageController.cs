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
        /// Save message to database and send in WebSocket
        /// </summary>
        /// <param name="saveAndSendMessageCommand"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> SaveAndSendMessage
            (
                SaveAndSendMessageCommand saveAndSendMessageCommand,
                CancellationToken cancellationToken = default
            ) => await Action(saveAndSendMessageCommand, cancellationToken);

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
