using Data;

namespace DataTest
{
    [TestClass]
    public class DataTest
    {
        private DataAbstractApi PrepareData()
        {
            DataAbstractApi data = DataAbstractApi.Create();
            return data;
        }
        
        [TestMethod]
        public void CreateItem()
        {
            DataAbstractApi data = PrepareData();

            string name = "Leczo";
            string desc = "Leczy";
            ItemType type = ItemType.Potion;
            float price = 15.50f;
            
            IItem item = data.GetWarehouse().CreateItem(name, desc, type, price);
            Assert.IsFalse(item.IsSold);
            Assert.AreEqual(item.Name, name);
            Assert.AreEqual(item.Description, desc);
            Assert.AreEqual(item.Type, type);
            Assert.AreEqual(item.Price, price);
        }
        
        [TestMethod]
        public void GetItemsTest()
        {
            DataAbstractApi data = PrepareData();
            Assert.AreNotEqual(data.GetWarehouse().GetItems().Count, 0);
        }
        
        [TestMethod]
        public void GetItemByIdTest()
        {
            DataAbstractApi data = PrepareData();
            List<IItem> items = data.GetWarehouse().GetItems();
            IItem testItem = items[0];
            
            Assert.AreSame(testItem, data.GetWarehouse().GetItemByID(testItem.Id));
        }

        [TestMethod]
        public void ItemsSoldTest()
        {
            DataAbstractApi data = PrepareData();
            List<IItem> items = data.GetWarehouse().GetItems();
            
            data.GetWarehouse().SellItem(items[0].Id);
            Assert.AreEqual(data.GetWarehouse().GetAvailableItems().Count, items.Count - 1);
        }
        
        [TestMethod]
        public void AddRemoveItemsTest()
        {
            DataAbstractApi data = PrepareData();
            IItem item = data.GetWarehouse().CreateItem("Item", "Desc", ItemType.Armor, 10f);
            
            data.GetWarehouse().AddItem(item);
            Assert.AreEqual(data.GetWarehouse().GetItemByID(item.Id), item);
            
            data.GetWarehouse().RemoveItem(item.Id);
            Assert.ThrowsException<KeyNotFoundException>(() => data.GetWarehouse().GetItemByID(item.Id));
        }

        [TestMethod]
        public void GetItemsByTypeTest()
        {
            DataAbstractApi data = PrepareData();
            List<IItem> foundItems = data.GetWarehouse().GetItemsByType(ItemType.Potion);

            foreach (IItem item in foundItems)
            {
                Assert.AreEqual(item.Type, ItemType.Potion);
            }
        }
    }
}