﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Logic;

namespace Model
{
    // TODO: LiptonLeon -- In theory you should not use Model types in View so you need to define adapters in ViewModel 
    public class ItemPresentation : INotifyPropertyChanged
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public PresentationItemType Type { get; private set; }
        public float Price { get; private set; }
        public bool IsSold { get; set; }
        public bool IsNotSold { get => !IsSold; } // Needed in XAML

        public ItemPresentation(IShopItem item)
        {
            Id = item.Id;
            Name = item.Name;
            Description = item.Description;
            Type = (PresentationItemType)item.Type;
            Price = item.Price;
            IsSold = item.IsSold;
        }

        public ItemPresentation(Guid id, string name, string description, PresentationItemType type, float price, bool isSold)
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
