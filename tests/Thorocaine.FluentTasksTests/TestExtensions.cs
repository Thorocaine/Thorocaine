using Xunit;

namespace Thorocaine.FluentTasksTests {
	public static class TestExtensions
	{
		public static TheoryData<T1, T2> AddNext<T1, T2>(this TheoryData<T1, T2> data, T1 p1, T2 p2)
		{
			data.Add(p1, p2);
			return data;
		}  
	}
}