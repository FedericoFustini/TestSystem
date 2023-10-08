namespace TestSystem.Models
{
	public class CreatedUserResponse
	{
		public CreatedUserResponse(Guid userId)
		{
			Id = userId;
		}

		public Guid Id { get; init; }
	}
}
