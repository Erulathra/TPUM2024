using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
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

        private AsyncObservableCollection<ViewModelItem> items;
        public AsyncObservableCollection<ViewModelItem> Items
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

        private string transactionString;
        public string TransactionString
        {
            get => transactionString;
            private set
            {
                if (transactionString != value)
                {
                    transactionString = value;
                    OnPropertyChanged();
                }
            }
        }

        public ViewModel()
        {
            model = new Model.Model(null);
            model.ModelConnectionService.OnConnectionStateChanged += OnConnectionStateChanged;
            model.ModelConnectionService.OnError += OnConnectionStateChanged;
            
            model.ModelConnectionService.Logger += Log;
            model.ModelConnectionService.OnMessage += OnMessage;
            
            model.WarehousePresentation.InflationChanged += HandleInflationChanged;
            model.WarehousePresentation.OnItemsUpdated += HandleOnItemsUpdated;
            model.WarehousePresentation.TransactionFinish += HandleTransactionFinish;

            OnConnectionStateChanged();

            CurrentTab = CurrentTabEnum.All;
            Items = new AsyncObservableCollection<ViewModelItem>(model.WarehousePresentation.GetItems().Select(item => new ViewModelItem(item)));
            
            inflationString = "Inflation: 1.0";
            transactionString = "Transaction: Ready";

            OnAllButtonCommand = new RelayCommand(() => HandleOnAllButton());
            OnAvailableButtonCommand = new RelayCommand(() => HandleOnAvailableButton());
            OnPotionsButtonCommand = new RelayCommand(() => HandleOnPotionsButton());
            OnSwordsButtonCommand = new RelayCommand(() => HandleOnSwordsButton());
            OnArmorsButtonCommand = new RelayCommand(() => HandleOnArmorsButton());
            OnHelmetsButtonCommand = new RelayCommand(() => HandleOnHelmetsButton());

            OnItemButtonCommand = new RelayCommand<Guid>((id) => HandleOnItemButton(id));
        }

        public async Task CloseConnection()
        {
            if (model.ModelConnectionService.IsConnected())
            {
                await model.ModelConnectionService.Disconnect();
            }
        }

        private void HandleTransactionFinish(bool succeeded)
        {
            string time = DateTime.Now.ToLongTimeString();
            TransactionString = succeeded ?
                $"Transaction finished succesfully! ({time})"
                : $"Transaction failed! ({time})";
        }

        private void Log(string message)
        {
            Console.WriteLine(message);
        }

        private void OnMessage(string message)
        {
            Log($"New Message: {message}");
        }

        private void OnConnectionStateChanged()
        {
            bool actualState = model.ModelConnectionService.IsConnected();
            ConnectionString = actualState ? "Connected" : "Disconnected";
            
            if (!actualState)
            {
                Task.Run(() => model.ModelConnectionService.Connect(new Uri(@"ws://localhost:21370")));
            }
            else
            {
                model.WarehousePresentation.RequestUpdate();
            }
        }

        private void HandleInflationChanged(object sender, ModelInflationChangedEventArgs args)
        {
            InflationString = $"Inflation: {args.NewInflation}";
            RefreshItems();
        }

        private void HandleOnItemsUpdated()
        {
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
            Task.Run(async () => await model.SellItem(id));
        }

        private void RefreshItems()
        {
            items.Clear();

            if (CurrentTab == CurrentTabEnum.All)
            {
                items.AddRange(model.WarehousePresentation.GetItems().Select(item => new ViewModelItem(item)));
            }
            else if (CurrentTab == CurrentTabEnum.Available)
            {
                items.AddRange(model.WarehousePresentation.GetAvailableItems().Select(item => new ViewModelItem(item)));
            }
            else
            {
                items.AddRange(model.WarehousePresentation.GetItemsByType((ModelItemType)CurrentTab).Select(item => new ViewModelItem(item)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
