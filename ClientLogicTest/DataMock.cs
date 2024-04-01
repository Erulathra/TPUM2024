using System;
using System.Collections.Generic;
using ClientData;

namespace LogicTest
{
	public class DataApiMock : DataAbstractApi
	{
		private readonly WarehouseMock warehouseMock = new WarehouseMock();
		private readonly ConnectionServiceMock connectionServiceMock = new ConnectionServiceMock();

        public override IConnectionService GetConnectionService()
        {
			return connectionServiceMock;
        }

        public override IWarehouse GetWarehouse()
		{
			return warehouseMock;
		}
	}

	public class ItemMock : IItem
	{
		public ItemMock(string name, string description, ItemType type, float price, bool isSold)
		{
			Id = Guid.NewGuid();
			Name = name;
			Description = description;
			Type = type;
			Price = price;
			IsSold = isSold;
		}

		public Guid Id { get; }
		public string Name { get; }
		public string Description { get; }
		public ItemType Type { get; }
		public float Price { get; set; }
		public bool IsSold { get; set; }
		public object Clone()
		{
			throw new NotImplementedException();
		}
	}

	public class WarehouseMock : IWarehouse
	{
		private readonly List<IItem> allItems;
		private readonly List<IItem> availableItems;

		public WarehouseMock()
		{
			availableItems = new List<IItem>
			{
				new ItemMock("Test", "TestDesc", ItemType.Armor, 10f, false),
				new ItemMock("Test2", "TestDesc2", ItemType.Armor, 11f, false)
			};
			allItems = new List<IItem>
			{
				new ItemMock("Sold", "SoldDesc2", ItemType.Helmet, 11f, true)
			};
			allItems.AddRange(availableItems);
		}

        public List<IItem> GetItems()
		{
			return allItems;
		}

		public List<IItem> GetAvailableItems()
		{
			return availableItems;
		}

		public float GetCurrentInflation()
		{
			return 1f;
		}

		public event EventHandler<InflationChangedEventArgs>? InflationChanged;
		public event Action? ItemsUpdated;
		public event Action<bool>? TransactionFinish;

		public IItem CreateItem(string name, string description, ItemType type, float price)
		{
			throw new NotImplementedException();
		}

		public void AddItem(IItem itemToAdd)
		{
			throw new NotImplementedException();
		}

		public void RemoveItem(Guid itemIdToRemove)
		{
			throw new NotImplementedException();
		}

		public IItem GetItemByID(Guid guid)
		{
			throw new NotImplementedException();
		}

		public List<IItem> GetItemsByType(ItemType type)
		{
			if (type == ItemType.Armor)
			{
				return availableItems;
			}

			return new List<IItem>();
		}

        public void RequestUpdate()
        {
            throw new NotImplementedException();
        }

        public async Task SellItem(Guid itemId)
        {
            IItem? item = availableItems.Find((item) => item.Id == itemId);
            if (item != null)
            {
                availableItems.Remove(item);
            }

			// Return dummy task
            await new Task(() => { });
        }
    }

    public class ConnectionServiceMock : IConnectionService
    {
        public event Action<string>? Logger;
        public event Action? OnConnectionStateChanged;
        public event Action<string>? OnMessage;
        public event Action? OnError;
        public event Action? OnDisconnect;

        public Task Connect(Uri peerUri)
        {
            throw new NotImplementedException();
        }

        public Task Disconnect()
        {
            throw new NotImplementedException();
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(string message)
        {
            throw new NotImplementedException();
        }
    }
}
