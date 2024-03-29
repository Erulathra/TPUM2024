using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientData
{
	internal class Warehouse : IWarehouse
	{
		private readonly Dictionary<Guid, IItem> items = new Dictionary<Guid, IItem>();
		private readonly object itemsLock = new object();

		public event EventHandler<InflationChangedEventArgs>? InflationChanged;

		
		private readonly ConnectionService connectionService;

		public Warehouse(ConnectionService connectionService)
		{
			this.connectionService = connectionService;
			this.connectionService.OnMessage += OnMessage;
		}

		private void OnMessage(string obj)
		{
			
		}

		public void SellItem(Guid itemId)
		{
			lock (itemsLock)
			{
				if (items.ContainsKey(itemId))
				{
					items[itemId].IsSold = true;
				}
			}
		}
		
		public List<IItem> GetItems()
		{
			List<IItem> result = new List<IItem>();
			lock (itemsLock)
			{
				result.AddRange(items.Values.Select(item => (IItem)item.Clone()));
			}
			
			return result;
		}

		public List<IItem> GetAvailableItems()
		{
			List<IItem> result = new List<IItem>();
			lock (itemsLock)
			{
				result.AddRange(items.Values
					.Where(item => !item.IsSold)
					.Select(item => (IItem)item.Clone()));
			}
			
			return result;
		}

		public void AddItem(IItem itemToAdd)
		{
			lock (itemsLock)
			{
				items.Add(itemToAdd.Id, itemToAdd);
			}
		}

		public void RemoveItem(Guid itemIdToRemove)
		{
			lock (itemsLock)
			{
				items.Remove(itemIdToRemove);
			}
		}

		public IItem GetItemByID(Guid guid)
		{
			IItem result;
			lock (itemsLock)
			{
				if (items.ContainsKey(guid))
				{
					result = items[guid];
				}
				else
				{
					throw new KeyNotFoundException();
				}
			}
			
			return result;
		}

		public List<IItem> GetItemsByType(ItemType type)
		{
			List<IItem> result = new List<IItem>();
			lock (itemsLock)
			{
				result.AddRange(items.Values
					.Where(item => item.Type == type)
					.Select(item => (IItem)item.Clone()));
			}
			
			return result;
		}
	}
}