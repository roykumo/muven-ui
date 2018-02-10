using CakeGUI.classes.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.view_model
{
    public class InventoryViewModel : INotifyPropertyChanged
    {
        private string _id;
        private ProductEntity _product;
        private DateTime _expiredDate=DateTime.Now;
        private Int32 _quantity;
        private Decimal _purchasePrice;
        private string _remarks;
        private InventoryEntity _inventory;
        
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
        public DateTime ExpiredDate
        {
            get { return _expiredDate; }
            set
            {
                this.MutateVerbose(ref _expiredDate, value, RaisePropertyChanged());
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
        public string Remarks
        {
            get { return _remarks; }
            set
            {
                this.MutateVerbose(ref _remarks, value, RaisePropertyChanged());
            }
        }
        public InventoryEntity Inventory
        {
            get { return _inventory; }
            set
            {
                this.MutateVerbose(ref _inventory, value, RaisePropertyChanged());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}