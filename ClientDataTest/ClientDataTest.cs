using ConnectionApi;
using ClientData;

namespace ClientDataTest;

[TestClass]
public class ClientDataTest
{
	static ConnectionServiceMock connectionService = new ConnectionServiceMock();
	DataAbstractApi data = DataAbstractApi.Create(connectionService);

	public void PrepareData()
	{
        connectionService.MockUpdateAll([
            new ItemDTO{Id = Guid.NewGuid(), Name = "n1", Description = "d", Type = "Potion", Price = 10.0f, IsSold = false},
            new ItemDTO{Id = Guid.NewGuid(), Name = "n2", Description = "d", Type = "Armor", Price = 30.0f, IsSold = false}
		]);
    }

	[TestMethod]
	public void UpdateAllTest()
	{
        ItemDTO[] itemDTOs = [
			new ItemDTO{Id = Guid.NewGuid(), Name = "n1", Description = "d", Type = "Potion", Price = 10.0f, IsSold = false },
			new ItemDTO{Id = Guid.NewGuid(), Name = "n2", Description = "d", Type = "Armor", Price = 30.0f, IsSold = false }
		];

		connectionService.MockUpdateAll(itemDTOs);
		List<IItem> items = data.GetWarehouse().GetItems();

        for (int i = 0; i < itemDTOs.Length; i++)
        {
			Assert.IsTrue(
				itemDTOs[i].Id == items[i].Id
				&& itemDTOs[i].Name == items[i].Name
				&& itemDTOs[i].Description == items[i].Description
				&& itemDTOs[i].Price == items[i].Price
				&& itemDTOs[i].IsSold == items[i].IsSold);
        }
    }

	[TestMethod]
	public void InflationChangeTest()
	{
		PrepareData();

		List<IItem> itemsBefore = data.GetWarehouse().GetItems();
		float newInflation = 2.0f;
		connectionService.MockInflationChanged(itemsBefore, 2.0f);
		List<IItem> itemsAfter = data.GetWarehouse().GetItems();

		for (int i = 0; i < itemsBefore.Count; i++)
		{
			Assert.AreEqual(itemsBefore[i].Price * newInflation, itemsAfter[i].Price);
		}
	}

	[TestMethod]
	public async Task SellItemTest()
	{
		Guid sellGuid = Guid.NewGuid();
        connectionService.MockUpdateAll([
            new ItemDTO{Id = sellGuid, Name = "n1", Description = "d", Type = "Potion", Price = 10.0f, IsSold = false },
            new ItemDTO{Id = Guid.NewGuid(), Name = "n2", Description = "d", Type = "Armor", Price = 30.0f, IsSold = false }
        ]);

		await data.GetWarehouse().SellItem(sellGuid);

		Assert.AreEqual(sellGuid, connectionService.lastSoldGuid);
    }

    [TestMethod]
    public void GetItemsTest()
    {
		PrepareData();

        Assert.AreNotEqual(data.GetWarehouse().GetItems().Count, 0);
    }

    [TestMethod]
    public void GetItemByIdTest()
    {
		PrepareData();

        List<IItem> items = data.GetWarehouse().GetItems();
        IItem testItem = items[0];

        Assert.AreEqual(testItem, data.GetWarehouse().GetItemByID(testItem.Id));
    }

    [TestMethod]
    public void GetItemsByTypeTest()
    {
		PrepareData();

        List<IItem> foundItems = data.GetWarehouse().GetItemsByType(ItemType.Potion);

        foreach (IItem item in foundItems)
        {
            Assert.AreEqual(item.Type, ItemType.Potion);
        }
    }
}