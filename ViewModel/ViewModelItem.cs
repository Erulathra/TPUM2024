using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model;

namespace ViewModel
{
	public class ViewModelItem
	{
		public enum ViewModelItemType
		{
			Potion = 0,
			Sword = 1,
			Armor = 2,
			Helmet = 3
		}
		
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ViewModelItemType Type { get; private set; }
        public float Price { get; private set; }
        public bool IsSold { get; set; }
        public bool IsNotSold { get => !IsSold; } // Needed in XAML

        public ViewModelItem(ModelItem item)
        {
            Id = item.Id;
            Name = item.Name;
            Description = item.Description;
            Type = (ViewModelItemType)item.Type;
            Price = item.Price;
            IsSold = item.IsSold;
        }

        public ViewModelItem(Guid id, string name, string description, ViewModelItemType type, float price, bool isSold)
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