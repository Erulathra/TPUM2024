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

        private string inflationString;
        public string InflationString
        {
            get => inflationString;
            private set
            {
                if (inflationString != value)
                {
                    inflationString = value;
                    OnPropertyChanged();
                }
            }
        }

        private string connectionString;

        public string ConnectionString
        {
            get => connectionString;
            private set
            {
                if (connectionString != value)
                {
                    connectionString = value;
                    OnPropertyChanged();
                }
            }
        }

        public ViewModel()
        {
            model = new Model.Model(null);
            model.InflationChanged += HandleInflationChanged;
            model.ModelConnectionService.OnConnectionStateChanged += OnConnectionStateChanged;
            model.ModelConnectionService.Logger += Log;

            connectionString = "disconnected";
            model.ModelConnectionService.Connect(new Uri(@"ws://localhost:21370"));

            CurrentTab = CurrentTabEnum.All;
            items = new ObservableCollection<ItemPresentation>();
            Items = new ObservableCollection<ItemPresentation>(model.WarehousePresentation.GetItems());
            
            inflationString = "n/a";

            OnAllButtonCommand = new RelayCommand(() => HandleOnAllButton());
            OnAvailableButtonCommand = new RelayCommand(() => HandleOnAvailableButton());
            OnPotionsButtonCommand = new RelayCommand(() => HandleOnPotionsButton());
            OnSwordsButtonCommand = new RelayCommand(() => HandleOnSwordsButton());
            OnArmorsButtonCommand = new RelayCommand(() => HandleOnArmorsButton());
            OnHelmetsButtonCommand = new RelayCommand(() => HandleOnHelmetsButton());

            OnItemButtonCommand = new RelayCommand<Guid>((id) => HandleOnItemButton(id));
        }

        private void Log(string message)
        {
            Console.WriteLine(message);
        }

        private void OnConnectionStateChanged()
        {
            ConnectionString = model.ModelConnectionService.IsConnected() ? "Connected" : "Disconnected";
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
                model.WarehousePresentation.GetItems().ToList().ForEach(items.Add);
            }
            else if (CurrentTab == CurrentTabEnum.Available)
            {
                model.WarehousePresentation.GetAvailableItems().ToList().ForEach(items.Add);
            }
            else
            {
                model.WarehousePresentation.GetItemsByType((PresentationItemType)CurrentTab).ToList().ForEach(items.Add);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
