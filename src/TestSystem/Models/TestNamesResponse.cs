using TestSystem.BusinessLogic.Models;

namespace TestSystem.Models
{
	public class TestsNamesResponse
	{
		public TestsNamesResponse(IEnumerable<TestName> names)
		{
			Names = names.Select(x => new TestNameResponse(x));
		}

		public IEnumerable<TestNameResponse> Names { get; init; }

	}

	public class TestNameResponse
	{
		public int Id { get; init; }
		public string Name { get; init; }

		public TestNameResponse(TestName name)
		{
			Id = name.Id;
			Name = name.Name;
		}
	}
}
