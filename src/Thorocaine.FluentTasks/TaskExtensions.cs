#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Thorocaine.FluentTasks
{
	public static class TaskExtensions
	{
		public static Task<TResult> Select<TSource, TResult>(this Task<TSource> source, Func<TSource, TResult> selector) 
			=> source.ContinueWith(task => selector(task.Result));

		public static Task<IEnumerable<T>> AsEnumerable<T>(this Task<T[]> source)
			=> source.Select(results => results.AsEnumerable());
		
		public static Task<IEnumerable<TResult>> SelectMany<TSource, TResult>(
			this Task<IEnumerable<TSource>> source,
			Func<TSource, TResult> selector) 
			=> source.Select(result => result.Select(selector));

		public static Task<IEnumerable<T>> Where<T>(this Task<IEnumerable<T>> source, Func<T, bool> predicate)
			=> source.Select(result => result.Where(predicate));

		public static Task<T> Do<T>(this Task<T> source, Action<T> tap)
			=> source.Select(result => { tap(result); return result; });

		public static Task<IEnumerable<T>> ForEach<T>(this Task<IEnumerable<T>> source, Action<T> action)
			=> source.Select(result => ForEach(result, action));
		
		public static Task<IEnumerable<T>> ForEach<T>(this Task<IEnumerable<T>> source, Func<T, Task> action)
			=> source.Select(result => ForEach(result, action)).Unwrap();

		static Task<IEnumerable<T>> ForEach<T>(IEnumerable<T> result, Func<T, Task> action)
		{
			var enumerable = result as T[] ?? result.ToArray();
			return Task.WhenAll(enumerable.Select(action)).ContinueWith(_ => enumerable.AsEnumerable());	
		}
		
		static IEnumerable<T> ForEach<T>(IEnumerable<T> source, Action<T> action)
		{
			var forEach = source as T[] ?? source.ToArray();
			foreach (var value in forEach) action(value);
			return forEach;
		}
	}
}