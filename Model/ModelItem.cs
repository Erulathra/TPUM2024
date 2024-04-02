using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Logic;

namespace Model
{
    public class ModelItem : INotifyPropertyChanged
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ModelItemType Type { get; private set; }
        public float Price { get; private set; }
        public bool IsSold { get; set; }
        public bool IsNotSold { get => !IsSold; } // Needed in XAML

        public ModelItem(IShopItem item)
        {
            Id = item.Id;
            Name = item.Name;
            Description = item.Description;
            Type = (ModelItemType)item.Type;
            Price = item.Price;
            IsSold = item.IsSold;
        }

        public ModelItem(Guid id, string name, string description, ModelItemType type, float price, bool isSold)
        {
            Id = id;
            Name = name;
            Description = description;
            Type = type;
            Price = price;
            IsSold = isSold;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
