using Api.Controllers.Base;
using Domain.Models.Handlers.Commands.Message;
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
        public async Task<IActionResult> RemovePatient
            (
                CreateMessageCommand createMessageCommand,
                CancellationToken cancellationToken = default
            ) => await Action(createMessageCommand, cancellationToken);
    }
}
