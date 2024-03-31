﻿using System;
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
        public WarehousePresentation WarehousePresentation { get; private set; }
        public ModelConnectionService ModelConnectionService { get; private set; }
        
	    public event Action? ItemsUpdated;  

        public Model(LogicAbstractApi? logicAbstractApi)
        {
            this.logicAbstractApi = logicAbstractApi ?? LogicAbstractApi.Create();
            
            WarehousePresentation = new WarehousePresentation(this.logicAbstractApi.GetShop());
            ModelConnectionService = new ModelConnectionService(this.logicAbstractApi.GetConnectionService());
        }

        public void SellItem(Guid itemId)
        {
            logicAbstractApi.GetShop().SellItem(itemId);
        }
    }
}
