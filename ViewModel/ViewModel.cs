using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model;

namespace ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        private Model.Model model;

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
        }

        private void HandleInflationChanged(object sender, ModelInflationChangedEventArgs args)
        {
            InflationString = $"NewInflation: {args.NewInflation}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
