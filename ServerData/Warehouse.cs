using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
	internal class Warehouse : IWarehouse
	{
		private readonly Dictionary<Guid, IItem> items = new Dictionary<Guid, IItem>();
		private object itemsLock = new object();

		private bool enableInflation;
		private object inflationLock = new object();

		public event EventHandler<InflationChangedEventArgs>? InflationChanged;
		
		public Warehouse()
		{
			AddItem(CreateItem("Nóż do kopert", "+5 obrażen", ItemType.Sword, 50));
            AddItem(CreateItem("Drewniane Wiadro", "+5 pancerza", ItemType.Helmet, 25));
            AddItem(CreateItem("Stalowe Wiadro", "+6 pancerza", ItemType.Helmet, 25));
            AddItem(CreateItem("Barszcz czerwony", "Leczy 100% HP", ItemType.Potion, 15));
            AddItem(CreateItem("Rosół", "Leczy 99.(9)% HP", ItemType.Potion, 15));

            SimulateInflation();
            enableInflation = true;
		}

		~Warehouse()
		{
			enableInflation = false;
			lock (inflationLock) { }
		}

		private async void SimulateInflation()
		{
			while (true)
			{
				Random random = new Random();
				// from 5 to 10 seconds
				float waitSeconds = (float)random.NextDouble() * 5f + 5f;
				await Task.Delay((int)Math.Truncate(waitSeconds * 1000f));
				
				// from 0.5 to 1.5
				float inflation = (float)random.NextDouble() + 0.5f;

				lock (itemsLock)
				{
					foreach (IItem item in items.Values)
					{
						item.Price *= inflation;
					}
				}
				
				InflationChanged?.Invoke(this, new InflationChangedEventArgs(inflation));

				lock (inflationLock)
				{
					if (!enableInflation)
					{
						break;
					}
				}
			}
		}


		public IItem CreateItem(string name, string description, ItemType type, float price)
		{
			return new Item(name, description, type, price);
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