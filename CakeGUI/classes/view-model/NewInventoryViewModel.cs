using CakeGUI.classes.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.view_model
{
    public class NewInventoryViewModel : INotifyPropertyChanged
    {
        private ProductTypeEntity _type;
        private string _invoice;
        private string _supplier;
        private string _transactionCode;
        private DateTime _date;

        public ProductTypeEntity Type
        {
            get { return _type; }
            set
            {
                this.MutateVerbose(ref _type, value, RaisePropertyChanged());
            }
        }
        public String Invoice
        {
            get { return _invoice; }
            set
            {
                this.MutateVerbose(ref _invoice, value, RaisePropertyChanged());
            }
        }
        public String Supplier
        {
            get { return _supplier; }
            set
            {
                this.MutateVerbose(ref _supplier, value, RaisePropertyChanged());
            }
        }
        public String TransactionCode
        {
            get { return _transactionCode; }
            set
            {
                this.MutateVerbose(ref _transactionCode, value, RaisePropertyChanged());
            }
        }
        public DateTime Date
        {
            get { return _date; }
            set
            {
                this.MutateVerbose(ref _date, value, RaisePropertyChanged());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}