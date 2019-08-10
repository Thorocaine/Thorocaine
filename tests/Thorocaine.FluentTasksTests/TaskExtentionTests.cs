#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Thorocaine.FluentTasks;
using Xunit;

namespace Thorocaine.FluentTasksTests
{
	public class TaskExtensionTests
	{
		const int Cent = 100;
		readonly Task<int> tCent = Cent.AsTask();

		[Fact]
		public async Task AsTasks()
		{
			var result = await tCent;
			result.Should().Be(Cent);
		}

		[Theory]
		[MemberData(nameof(IntManipulationTestData))]
		public async Task Select_to_manipulate_a_result(Func<int, int> selector, int expected)
		{
			var result = await tCent.Select(selector);
			result.Should().Be(expected);
		}

		[Fact]
		public async Task Select_to_manipulate_a_string_result()
		{
			var result = await "My String".AsTask().Select(x => x.ToUpper());
			result.Should().Be("MY STRING");
		}

		[Fact]
		public async Task SelectMany_manipulates_result_enumerable_from_array()
		{
			var intTask = Enumerable.Range(1, 3).ToArray().AsTask();
			var doubleTask = intTask.AsEnumerable().SelectMany(x => x * 2);
			var results = await doubleTask; 
			results.Should().Equal(2, 4, 6);
		}
		
		[Fact]
		public async Task SelectMany_manipulates_result_enumerable()
		{
			var intTask = Enumerable.Range(1, 3).AsTask();
			var doubleTask = intTask.SelectMany(x => x * 2);
			var results = await doubleTask; 
			results.Should().Equal(2, 4, 6);
		}

		[Fact]
		public async Task Where_filters_results()
		{
			var enumTask = Enumerable.Range(1, 10).AsTask();
			var result = await enumTask.Where(x => x % 2 == 0);
			result.Should().Equal(2, 4, 6, 8, 10);
		}

		[Fact]
		public async Task Do_creates_a_side_effect()
		{
			var testVal = 0;
			await tCent.Do(x => testVal = x);
			testVal.Should().Be(Cent);
		}

		[Fact]
		public async Task Do_does_not_affect_pipe()
		{
			var result = await tCent.Do(x => { }).Select(x => x / 2);
			result.Should().Be(50);
		}

		[Fact]
		public async Task ForEach_executes_for_each_item_in_enumerable()
		{
			var results = new List<int>();
			var task = Enumerable.Range(1, 5).AsTask();
			await task.ForEach(results.Add);
			results.Should().Equal(1, 2, 3, 4, 5);
		}
		
		[Fact]
		public async Task ForEach_should_not_affect_pipe()
		{
			var task = Enumerable.Range(1, 5).AsTask();
			var results = await task.ForEach(x => { });
			results.Should().Equal(1, 2, 3, 4, 5);
		}

		[Fact]
		public async Task ForEach_can_run_async_tasks()
		{
			var results = new List<int>();
			var task = Enumerable.Range(1, 5).AsTask();
			await task.ForEach(async (x) => await Task.Run(() => results.Add(x)));
			results.Should().Equal(1, 2, 3, 4, 5);
		}
		
		[Fact]
		public async Task ForEach_can_run_async_and_not_affect_pipe()
		{
			var task = Enumerable.Range(1, 5).AsTask();
			var results = await task.ForEach(async _ => await Task.Run(() => { }));
			results.Should().Equal(1, 2, 3, 4, 5);
		}

		public static readonly TheoryData<Func<int, int>, int> IntManipulationTestData =
			new TheoryData<Func<int, int>, int>()
			   .AddNext(x => x / 4, 25)
			   .AddNext(x => x / 2, 50)
			   .AddNext(x => x * 2, 200)
			   .AddNext(x => x * 4, 400);
	}
}