using System;
using Data;
using Logic;

namespace Model
{
	public class ModelInflationChangedEventArgs : EventArgs
	{
	    public float NewInflation { get; }

	    public ModelInflationChangedEventArgs(float newInflation)
	    {
		    this.NewInflation = newInflation;
	    }

	    internal ModelInflationChangedEventArgs(LogicInflationChangedEventArgs args)
	    {
		    this.NewInflation = args.NewInflation;
	    }
	}
	
    public class Model
    {
        private LogicAbstractApi logicAbstractApi;

        public event EventHandler<ModelInflationChangedEventArgs>? InflationChanged;

        public Model(LogicAbstractApi? logicAbstractApi)
        {
            this.logicAbstractApi = logicAbstractApi == null ? LogicAbstractApi.Create() : logicAbstractApi;
            this.logicAbstractApi.GetShop().InflationChanged += HandleInflationChanged;
        }

        public void HandleInflationChanged(object sender, LogicInflationChangedEventArgs args)
        {
	        InflationChanged?.Invoke(this, new ModelInflationChangedEventArgs(args));
        }

    }
}
