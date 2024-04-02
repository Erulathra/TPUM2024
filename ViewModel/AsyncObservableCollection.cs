using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace ViewModel
{
	public class AsyncObservableCollection<T> : ObservableCollection<T>
	{
		private readonly SynchronizationContext synchronizationContext;

		public AsyncObservableCollection()
		{
			synchronizationContext = SynchronizationContext.Current;
		}

		public AsyncObservableCollection(IEnumerable<T> list)
			: base(list)
		{
			synchronizationContext = SynchronizationContext.Current;
		}
		

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs eventArgs)
		{
			if (SynchronizationContext.Current == synchronizationContext)
			{
				// Execute the CollectionChanged event on the current thread
				RaiseCollectionChanged(eventArgs);
			}
			else
			{
				// Raises the CollectionChanged event on the creator thread
				synchronizationContext.Send(RaiseCollectionChanged, eventArgs);
			}
		}

		private void RaiseCollectionChanged(object param)
		{
			// We are in the creator thread, call the base implementation directly
			base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
		}

		protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			if (SynchronizationContext.Current == synchronizationContext)
			{
				// Execute the PropertyChanged event on the current thread
				RaisePropertyChanged(eventArgs);
			}
			else
			{
				// Raises the PropertyChanged event on the creator thread
				synchronizationContext.Send(RaisePropertyChanged, eventArgs);
			}
		}

		private void RaisePropertyChanged(object param)
		{
			// We are in the creator thread, call the base implementation directly
			base.OnPropertyChanged((PropertyChangedEventArgs)param);
		}

		public void AddRange(IEnumerable<T> elements)
		{
			foreach (var element in elements)
			{
				Add(element);
			}
		}
	}
}