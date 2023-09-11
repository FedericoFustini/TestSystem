using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TestSystem.BusinessLogic;
using TestSystem.BusinessLogic.Exceptions;
using TestSystem.BusinessLogic.Interfaces;
using TestSystem.Models;

namespace TestSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUsersManager _usersManager;
		private readonly ILogger<UsersController> _logger;

		public UsersController(IUsersManager usersManager, ILogger<UsersController> logger)
		{
			_usersManager = usersManager;
			_logger = logger;
		}

		[HttpGet("{userId}/test/{testId}/result")]
		[ProducesResponseType(typeof(CorrectAnswerResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
		public async Task<IActionResult> ReadTestResult([FromRoute, Required] Guid? userId, [FromRoute, Required] int? testId)
		{
			try
			{
				var count = await _usersManager.ReadTestResult(userId.Value, testId.Value);
				return Ok(new CorrectAnswerResponse(count));
			}
			catch (UserNotFoundException e)
			{
				_logger.LogWarning(e, e.Message);
				return NotFound(e.Message);
			}
			catch (TestNotCompletedException e)
			{
				_logger.LogWarning(e, e.Message);
				return Conflict(e.Message);
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				throw;
			}
		}
	}
}
