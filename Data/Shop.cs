using System;
using System.Collections.Generic;

namespace Data
{
	class Shop : IShop
	{
		private Dictionary<Guid, IItem> items;
		
		public List<IItem> GetItems()
		{
			throw new NotImplementedException();
		}

		public List<IItem> GetAvailableItems()
		{
			throw new NotImplementedException();
		}

		public void AddItem(IItem itemToAdd)
		{
			throw new NotImplementedException();
		}

		public void RemoveItem(IItem itemToRemove)
		{
			throw new NotImplementedException();
		}

		public IItem GetItemByID(Guid guid)
		{
			throw new NotImplementedException();
		}

		public List<IItem> GetItemsByType(ItemType type)
		{
			throw new NotImplementedException();
		}
	}
}