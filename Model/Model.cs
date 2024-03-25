using System;
using Logic;

namespace Model
{
	public enum PresentationItemType
	{
        Potion = 0,
        Sword = 1,
        Armor = 2,
        Helmet = 3
    }

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
        
        public WarehousePresentation warehousePresentation { get; private set; }

        public event EventHandler<ModelInflationChangedEventArgs>? InflationChanged;

        public Model(LogicAbstractApi? logicAbstractApi)
        {
            this.logicAbstractApi = logicAbstractApi == null ? LogicAbstractApi.Create() : logicAbstractApi;
            this.logicAbstractApi.GetShop().InflationChanged += HandleInflationChanged;
            this.warehousePresentation = new WarehousePresentation(this.logicAbstractApi.GetShop());
        }

        public void HandleInflationChanged(object sender, LogicInflationChangedEventArgs args)
        {
	        InflationChanged?.Invoke(this, new ModelInflationChangedEventArgs(args));
        }

    }
}
