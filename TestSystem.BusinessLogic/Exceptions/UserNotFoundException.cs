namespace TestSystem.BusinessLogic.Exceptions
{
	public class UserNotFoundException : Exception
	{
		public UserNotFoundException(string? message) : base(message)
		{
		}
	}
}
