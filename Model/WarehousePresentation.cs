using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;

namespace Model
{
    public class WarehousePresentation
    {
        private IShop shop { get; set; }

        public WarehousePresentation(IShop shop)
        {
            this.shop = shop;
        }

        public List<ItemPresentation> GetItems()
        {
            return shop.GetItems()
                            .Select(item => new ItemPresentation(item))
                            .Cast<ItemPresentation>()
                            .ToList();
        }

        public List<ItemPresentation> GetAvailableItems()
        {
            return shop.GetAvailableItems()
                            .Select(item => new ItemPresentation(item))
                            .Cast<ItemPresentation>()
                            .ToList();
        }

        public List<ItemPresentation> GetItemsByType(PresentationItemType itemType)
        {
            return shop.GetItemsByType((LogicItemType)itemType)
                            .Select(item => new ItemPresentation(item))
                            .Cast<ItemPresentation>()
                            .ToList();
        }
    }
}
