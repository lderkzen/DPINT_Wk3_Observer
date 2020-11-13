using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPINT_Wk3_Observer.Model
{
	public abstract class Observable<T> : IObservable<T>, IDisposable
	{
		private List<IObserver<T>> _observers;

		public Observable()
		{
			_observers = new List<IObserver<T>>();
		}

		private struct Unsubscriber : IDisposable
		{
			private Action _unsubscribe;
			public Unsubscriber(Action unsubscribe) { _unsubscribe = unsubscribe; }
			public void Dispose() => _unsubscribe();
		}

		public IDisposable Subscribe(IObserver<T> observer)
		{
			_observers.Add(observer);
			return new Unsubscriber(() => _observers.Remove(observer));
		}

		protected void Notify(T subject)
		{
			foreach (var obs in _observers)
			{
				obs.OnNext(subject);
			}
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
