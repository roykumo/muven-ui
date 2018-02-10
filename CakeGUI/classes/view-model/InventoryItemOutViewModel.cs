using CakeGUI.classes.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.view_model
{
    public class InventoryItemOutViewModel : INotifyPropertyChanged
    {
        private string _id;
        private ProductEntity _product;
        private Int32 _quantity;
        private Decimal _purchasePrice;
        private Decimal _sellPriceTrx;
        private SellPrice _sellPrice;
        private string _remarks;
        private InventoryOutEntity _inventoryOut;
        
        public string Id
        {
            get { return _id; }
            set
            {
                this.MutateVerbose(ref _id, value, RaisePropertyChanged());
            }
        }
        public ProductEntity Product
        {
            get { return _product; }
            set
            {
                this.MutateVerbose(ref _product, value, RaisePropertyChanged());
            }
        }
        public Int32 Quantity
        {
            get { return _quantity; }
            set
            {
                this.MutateVerbose(ref _quantity, value, RaisePropertyChanged());
            }
        }
        public Decimal PurchasePrice
        {
            get { return _purchasePrice; }
            set
            {
                this.MutateVerbose(ref _purchasePrice, value, RaisePropertyChanged());
            }
        }
        public Decimal SellPriceTrx
        {
            get { return _sellPriceTrx; }
            set
            {
                this.MutateVerbose(ref _sellPriceTrx, value, RaisePropertyChanged());
            }
        }
        public SellPrice SellPrice
        {
            get { return _sellPrice; }
            set
            {
                this.MutateVerbose(ref _sellPrice, value, RaisePropertyChanged());
            }
        }
        public string Remarks
        {
            get { return _remarks; }
            set
            {
                this.MutateVerbose(ref _remarks, value, RaisePropertyChanged());
            }
        }
        public InventoryOutEntity InventoryOut
        {
            get { return _inventoryOut; }
            set
            {
                this.MutateVerbose(ref _inventoryOut, value, RaisePropertyChanged());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}