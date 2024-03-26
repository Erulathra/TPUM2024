using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Model;

namespace ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        public enum CurrentTabEnum
        {
            Potions = 0,
            Swords = 1,
            Armors = 2,
            Helmets = 3,
            All = 4,
            Available = 5
        }

        private CurrentTabEnum currentTab;
        public CurrentTabEnum CurrentTab
        {
            get => currentTab;
            private set
            {
                if (currentTab != value)
                {
                    currentTab = value;
                    OnPropertyChanged();
                }
            }

        }

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

            CurrentTab = CurrentTabEnum.All;
            Items = new ObservableCollection<ItemPresentation>(model.warehousePresentation.GetItems());

            OnAllButtonCommand = new RelayCommand(() => HandleOnAllButton());
            OnAvailableButtonCommand = new RelayCommand(() => HandleOnAvailableButton());
            OnPotionsButtonCommand = new RelayCommand(() => HandleOnPotionsButton());
            OnSwordsButtonCommand = new RelayCommand(() => HandleOnSwordsButton());
            OnArmorsButtonCommand = new RelayCommand(() => HandleOnArmorsButton());
            OnHelmetsButtonCommand = new RelayCommand(() => HandleOnHelmetsButton());



            OnItemButtonCommand = new RelayCommand<Guid>((id) => HandleOnItemButton(id));
        }

        private void HandleInflationChanged(object sender, ModelInflationChangedEventArgs args)
        {
            InflationString = $"NewInflation: {args.NewInflation}";
            RefreshItems();
        }

        public ICommand OnAllButtonCommand { get; private set; }
        private void HandleOnAllButton()
        {
            CurrentTab = CurrentTabEnum.All;
            RefreshItems();
        }

        public ICommand OnAvailableButtonCommand { get; private set; }
        private void HandleOnAvailableButton()
        {
            CurrentTab = CurrentTabEnum.Available;
            RefreshItems();
        }

        public ICommand OnPotionsButtonCommand { get; private set; }
        private void HandleOnPotionsButton()
        {
            CurrentTab = CurrentTabEnum.Potions;
            RefreshItems();
        }

        public ICommand OnSwordsButtonCommand { get; private set; }
        private void HandleOnSwordsButton()
        {
            CurrentTab = CurrentTabEnum.Swords;
            RefreshItems();
        }

        public ICommand OnArmorsButtonCommand { get; private set; }
        private void HandleOnArmorsButton()
        {
            CurrentTab = CurrentTabEnum.Armors;
            RefreshItems();
        }

        public ICommand OnHelmetsButtonCommand { get; private set; }
        private void HandleOnHelmetsButton()
        {
            CurrentTab = CurrentTabEnum.Helmets;
            RefreshItems();
        }

        public ICommand OnItemButtonCommand { get; private set; }
        private void HandleOnItemButton(Guid id)
        {
            model.SellItem(id);
            RefreshItems();
        }

        private void RefreshItems()
        {
            items.Clear();

            if (CurrentTab == CurrentTabEnum.All)
            {
                model.warehousePresentation.GetItems().ToList().ForEach(items.Add);
            }
            else if (CurrentTab == CurrentTabEnum.Available)
            {
                model.warehousePresentation.GetAvailableItems().ToList().ForEach(items.Add);
            }
            else
            {
                model.warehousePresentation.GetItemsByType((PresentationItemType)CurrentTab).ToList().ForEach(items.Add);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
