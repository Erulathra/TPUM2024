using System;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
	internal class Warehouse : IWarehouse
	{
		private readonly Dictionary<Guid, IItem> items = new Dictionary<Guid, IItem>();

		public Warehouse()
		{
			AddItem(CreateItem("Nóż do kopert", "+5 obrarzen", ItemType.Sword, 50));
            AddItem(CreateItem("Drewniane Wiadro", "+5 pancerza", ItemType.Helmet, 25));
            AddItem(CreateItem("Stalowe Wiadro", "+6 pancerza", ItemType.Helmet, 25));
            AddItem(CreateItem("Barszcz czerwony", "Leczy 100% HP", ItemType.Potion, 15));
            AddItem(CreateItem("Rosół", "Leczy 99.(9)% HP", ItemType.Potion, 15));
		}

		public IItem CreateItem(string name, string description, ItemType type, float price)
		{
			return new Item(name, description, type, price);
		}

		public void SellItem(Guid itemId)
		{
			if (items.ContainsKey(itemId))
			{
				items[itemId].IsSold = true;
			}
		}
		
		public List<IItem> GetItems()
		{
			return items.Values.ToList();
		}

		public List<IItem> GetAvailableItems()
		{
			return GetItems().Where(item => !item.IsSold).ToList();
		}

		public void AddItem(IItem itemToAdd)
		{
			items.Add(itemToAdd.Id, itemToAdd);
		}

		public void RemoveItem(Guid itemIdToRemove)
		{
			items.Remove(itemIdToRemove);
		}

		public IItem GetItemByID(Guid guid)
		{
			if (!items.ContainsKey(guid))
			{
				throw new KeyNotFoundException();
			}
			
			return items[guid];
		}

		public List<IItem> GetItemsByType(ItemType type)
		{
			return GetItems().Where(item => item.Type == type).ToList();
		}
	}
}