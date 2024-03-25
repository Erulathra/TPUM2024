using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Model;

namespace ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        private Model.Model model;

        private ObservableCollection<ItemPresentation> items;
        public ObservableCollection<ItemPresentation> Items
        {
            get => items;
            private set
            {
                if (items != value)
                {
                    items = value;
                    OnPropertyChanged("Items");
                }
            }
        }

        private string inflatcionString;
        public string InflationString
        {
            get => inflatcionString;
            private set
            {
                if (inflatcionString != value)
                {
                    inflatcionString = value;
                    OnPropertyChanged();
                }
            }
        }

        public ViewModel()
        {
            this.model = new Model.Model(null);
            model.InflationChanged += HandleInflationChanged;
            Items = new ObservableCollection<ItemPresentation>(model.warehousePresentation.GetAvailableItems());

            
            OnAllButtonCommand = new RelayCommand(() => HandleOnAllButton());
            OnPotionsButtonCommand = new RelayCommand(() => HandleOnPotionsButton());
            OnSwordsButtonCommand = new RelayCommand(() => HandleOnSwordsButton());
            OnArmorsButtonCommand = new RelayCommand(() => HandleOnArmorsButton());
            OnHelmetsButtonCommand = new RelayCommand(() => HandleOnHelmetsButton());

            OnItemButtonCommand = new RelayCommand<Guid>((id) => HandleOnItemButton(id));
        }

        private void HandleInflationChanged(object sender, ModelInflationChangedEventArgs args)
        {
            InflationString = $"NewInflation: {args.NewInflation}";
        }

        public ICommand OnAllButtonCommand { get; private set; }
        private void HandleOnAllButton()
        {
            items.Clear();
            model.warehousePresentation.GetAvailableItems().ToList().ForEach(items.Add);
            PrintItems();
        }

        public ICommand OnPotionsButtonCommand { get; private set; }
        private void HandleOnPotionsButton()
        {
            items.Clear();
            model.warehousePresentation.GetItemsByType(PresentationItemType.Potion).ToList().ForEach(items.Add);
            PrintItems();
        }

        public ICommand OnSwordsButtonCommand { get; private set; }
        private void HandleOnSwordsButton()
        {
            items.Clear();
            model.warehousePresentation.GetItemsByType(PresentationItemType.Sword).ToList().ForEach(items.Add);
            PrintItems();
        }

        public ICommand OnArmorsButtonCommand { get; private set; }
        private void HandleOnArmorsButton()
        {
            items.Clear();
            model.warehousePresentation.GetItemsByType(PresentationItemType.Armor).ToList().ForEach(items.Add);
            PrintItems();
        }

        public ICommand OnHelmetsButtonCommand { get; private set; }
        private void HandleOnHelmetsButton()
        {
            items.Clear();
            model.warehousePresentation.GetItemsByType(PresentationItemType.Helmet).ToList().ForEach(items.Add);
            PrintItems();
        }

        public ICommand OnItemButtonCommand { get; private set; }
        private void HandleOnItemButton(Guid id)
        {
            model.SellItem(id);
            ItemPresentation item = items.ToList().Find(item => item.Id == id);
            Console.Out.WriteLine($"Sold: {id}, {item.Name}, {item.Description}");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PrintItems()
        {
            items.ToList().ForEach(item => Console.Out.WriteLine($"{item.Id}, {item.Name}, {item.Description}"));
            Console.Out.WriteLine("-------");
        }
    }
}
