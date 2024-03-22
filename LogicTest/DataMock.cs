using Data;

namespace LogicTest;

public class DataApiMock : DataAbstractApi
{
	private WarehouseMock warehouseMock = new WarehouseMock();

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
}

public class WarehouseMock : IWarehouse
{
	private readonly List<IItem> allItems;
	private readonly List<IItem> avialableItems;

	public WarehouseMock()
	{
		avialableItems = new List<IItem>
		{
			new ItemMock("Test", "TestDesc", ItemType.Armor, 10f, false),
			new ItemMock("Test2", "TestDesc2", ItemType.Armor, 11f, false)
		};
		allItems = new List<IItem>
		{
			new ItemMock("Sold", "SoldDesc2", ItemType.Helmet, 11f, true)
		};
		allItems.AddRange(avialableItems);
	}

	public void SellItem(Guid itemId)
	{
		avialableItems.Remove(avialableItems.Find((item) => item.Id == itemId));
	}

	public List<IItem> GetItems()
	{
		return allItems;
	}

	public List<IItem> GetAvailableItems()
	{
		return avialableItems;
	}

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
			return avialableItems;
		}

		return new List<IItem>();
	}
}