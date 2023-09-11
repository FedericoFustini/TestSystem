using Microsoft.AspNetCore.Mvc;
using TestSystem.BusinessLogic.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DatabaseInitializationController : ControllerBase
	{
		private readonly IDatabaseInitializationManager _databaseInitializationManager;
		private readonly ILogger<DatabaseInitializationController> _logger;

		public DatabaseInitializationController(IDatabaseInitializationManager databaseInitializationManager, ILogger<DatabaseInitializationController> logger)
		{
			_databaseInitializationManager = databaseInitializationManager;
			_logger = logger;
		}


		[HttpPut("create")]
		public async Task<IActionResult> CreateDatabase()
		{
			try
			{
				await _databaseInitializationManager.CreateDatabase();
				return Ok();
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				throw;
			}
		}

		[HttpDelete()]
		public async Task<IActionResult> DeleteDatabase()
		{
			try
			{
				await _databaseInitializationManager.DeleteDatabase();
				return Ok();
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				throw;
			}
		}

		[HttpPost("populate")]
		public async Task<IActionResult> PopulateDatabase()
		{
			try
			{
				await _databaseInitializationManager.PopulateDatabase();
				return Ok();
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
				throw;
			}
		}

	}
}
