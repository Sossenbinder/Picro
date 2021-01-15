using Picro.Common.Extensions.Async;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Picro.Common.Disposable
{
	public class Disposable : IDisposable, IAsyncDisposable
	{
		private readonly List<IDisposable> _disposables;

		private readonly List<IAsyncDisposable> _asyncDisposables;

		public Disposable()
		{
			_disposables = new List<IDisposable>();
			_asyncDisposables = new List<IAsyncDisposable>();
		}

		protected void RegisterDisposable(IDisposable disposable) => _disposables.Add(disposable);

		protected void RegisterAsyncDisposable(IAsyncDisposable asyncDisposable) => _asyncDisposables.Add(asyncDisposable);

		public void Dispose()
		{
			foreach (var disposable in _disposables)
			{
				disposable.Dispose();
			}
		}

		public ValueTask DisposeAsync()
		{
			return _asyncDisposables.ParallelAsyncValueTask(x => x.DisposeAsync());
		}
	}
}