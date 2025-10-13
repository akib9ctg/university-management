using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniversityManagement.Application.Classes.Command.CreateClass;
using UniversityManagement.Application.Classes.Command.UpdateClass;
using UniversityManagement.Application.Classes.Queries.GetClassById;

namespace UniversityManagement.API.Controllers
{
    public class ClassController : BaseApiController
    {
        private readonly ISender _sender;

        public ClassController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateClass(CreateClassRequest createClassRequest, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new CreateClassCommand(createClassRequest), cancellationToken);
            return Success(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetClassByIdQuery(id), cancellationToken);
            return Success(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdateClassRequest updateClassRequest, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new UpdateClassCommand(updateClassRequest), cancellationToken);
            return Success(result);
        }
    }
}
