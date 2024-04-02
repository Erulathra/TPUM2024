using System;
using System.Collections.Generic;
using System.Linq;
using Logic;

namespace Model
{
    public class WarehousePresentation
    {
        private IShop Shop { get; set; }

        public event EventHandler<ModelInflationChangedEventArgs>? InflationChanged;
        public Action? OnItemsUpdated;
		public event Action<bool>? TransactionFinish;

        public WarehousePresentation(IShop shop)
        {
            this.Shop = shop;

            shop.ItemsUpdated += () => OnItemsUpdated?.Invoke();
            shop.InflationChanged += (obj, args) => InflationChanged?.Invoke(this, new ModelInflationChangedEventArgs(args));
            shop.TransactionFinish += succeeded => TransactionFinish?.Invoke(succeeded);
        }

        public void RequestUpdate()
        {
            Shop.RequestUpdate();
        }

        public List<ModelItem> GetItems()
        {
            return Shop.GetItems()
                .Select(item => new ModelItem(item))
                .ToList();
        }

        public List<ModelItem> GetAvailableItems()
        {
            return Shop.GetAvailableItems()
                .Select(item => new ModelItem(item))
                .ToList();
        }

        public List<ModelItem> GetItemsByType(ModelItemType itemType)
        {
            return Shop.GetItemsByType((LogicItemType)itemType)
                .Select(item => new ModelItem(item))
                .ToList();
        }
    }
}
