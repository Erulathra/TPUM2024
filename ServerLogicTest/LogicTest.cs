using System.Linq;
using Logic;

namespace LogicTest
{
    [TestClass]
    public class LogicTest
    {
        private LogicAbstractApi logicApi = LogicAbstractApi.Create(new DataApiMock());
        
        [TestMethod]
        public void SellItem()
        {
            IShopItem[] availableItems = logicApi.GetShop().GetItems().Where(item => !item.IsSold).ToArray();
            Assert.IsTrue(availableItems.Length == 2);
            IShopItem itemToSell = availableItems.First();
            logicApi.GetShop().SellItem(itemToSell.Id);
            
            availableItems = logicApi.GetShop().GetItems().Where(item => !item.IsSold).ToArray();
            
            Assert.IsTrue(availableItems.Length == 1);
        }
        
        [TestMethod]
        public void GetItems()
        {
            Assert.IsTrue(logicApi.GetShop().GetItems().Count == 3);
        }
    }
}