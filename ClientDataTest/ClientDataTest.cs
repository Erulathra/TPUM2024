using ClientApi;
using ClientData;
using NuGet.Frameworks;

namespace ClientDataTest;

[TestClass]
public class ClientDataTest
{
	static ConnectionServiceMock connectionService = new ConnectionServiceMock();
	DataAbstractApi data = DataAbstractApi.Create(connectionService);

	public void PrepareData()
	{
        connectionService.MockUpdateAll([
			new ItemDTO(Guid.NewGuid(), "n1", "d", "Potion", 10.0f, false),
            new ItemDTO(Guid.NewGuid(), "n2", "d", "Armor", 30.0f, false)
		]);
    }

	[TestMethod]
	public void UpdateAllTest()
	{
		ItemDTO[] itemDTOs = [
			new ItemDTO(Guid.NewGuid(), "n1", "d", "Potion", 10.0f, false),
			new ItemDTO(Guid.NewGuid(), "n2", "d", "Armor", 30.0f, false)
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
	public async void SellItemTest()
	{
		Guid sellGuid = Guid.NewGuid();
        connectionService.MockUpdateAll([
			new ItemDTO(sellGuid, "n1", "d", "Potion", 10.0f, false),
			new ItemDTO(Guid.NewGuid(), "n2", "d", "Armor", 30.0f, false)
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