using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestSystem.BusinessLogic.Exceptions;
using TestSystem.BusinessLogic.Interfaces;
using TestSystem.Models;

namespace TestSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TestsController : ControllerBase
	{
		private readonly ITestsManager _testsManager;
		private readonly ILogger<TestsController> _logger;

		public TestsController(ITestsManager testsManager, ILogger<TestsController> logger)
		{
			_testsManager = testsManager;
			_logger = logger;
		}

		/// <summary>
		/// Get all test name for fill a dropdown for choose test
		/// </summary>
		/// <returns></returns>
		[HttpGet("names")]
		//never use a c# type as response. wrap always in a custom object because if you want add property in future, you can do it without change response type class
		//for this reasone i wrap IEnumerable<TestNameResponse> in a TestsNamesResponse obj
		[ProducesResponseType(typeof(TestsNamesResponse), StatusCodes.Status200OK)]
		public async Task<IActionResult> ReadTestsNames()
		{
			try
			{
				var res = await _testsManager.ReadTestsNames();
				return Ok(new TestsNamesResponse(res));
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				throw;
			}
		}

		/// <summary>
		/// Create user and returns random question for selected test
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		//this response should be paginated if a test can have an high number of questions
		[HttpPut("create")]
		[ProducesResponseType(typeof(CreatedTestResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> CreateTest([FromBody, Required] CreateTestRequest request)
		{
			try
			{
				var createdTest = await _testsManager.CreateTest(request.Username, request.TestId.Value);
				return Ok(new CreatedTestResponse(createdTest));
			}
			catch (TestNotFoundException e)
			{
				_logger.LogWarning(e, e.Message);
				return NotFound(e.Message);
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				throw;
			}
		}

		/// <summary>
		/// Update user test inserting new answer
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPatch()]
		[ProducesResponseType(typeof(CreatedTestResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> UpdateTest([FromBody, Required] UpdateTestRequest request)
		{
			try
			{
				await _testsManager.UpdateTest(request.ToBLUpdateTest());
				return Ok();
			}
			catch (AnswerConflictException e)
			{
				_logger.LogWarning(e, e.Message);
				return Conflict(e.Message);
			}
			catch (UserNotFoundException e)
			{
				_logger.LogWarning(e, e.Message);
				return NotFound(e.Message);
			}
			catch (TestNotFoundException e)
			{
				_logger.LogWarning(e, e.Message);
				return NotFound(e.Message);
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				throw;
			}
		}
	}
}
