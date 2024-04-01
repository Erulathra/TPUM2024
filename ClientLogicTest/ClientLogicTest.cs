using Logic;
using LogicTest;

namespace ClientLogicTest;

[TestClass]
public class ClientLogicTest
{
    private LogicAbstractApi logicApi = LogicAbstractApi.Create(new DataApiMock());

    [TestMethod]
    public void SellItem()
    {
        IShopItem itemToSell = logicApi.GetShop().GetAvailableItems()[0];
        logicApi.GetShop().SellItem(itemToSell.Id);

        Assert.IsTrue(logicApi.GetShop().GetAvailableItems().Count == 1);
    }

    [TestMethod]
    public void GetItems()
    {
        Assert.IsTrue(logicApi.GetShop().GetAvailableItems().Count == 2);
        Assert.IsTrue(logicApi.GetShop().GetItems().Count == 3);
        Assert.IsTrue(logicApi.GetShop().GetItemsByType(LogicItemType.Armor).Count == 2);
    }
}