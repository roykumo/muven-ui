using CakeGUI.classes.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.view_model
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private string _barcode2;
        private string _id;
        private string _code;
        private string _barCode;
        private string _name;
        private string _productGroup;
        private ProductTypeEntity _type;
        private ProductCategoryEntity _category;
        private int _alertRed;
        private int _alertYellow;
        private int _alertGreen;
        private int _alertBlue;

        public String Id
        {
            get { return _id; }
            set
            {
                this.MutateVerbose(ref _id, value, RaisePropertyChanged());
            }
        }
        public String Code
        {
            get { return _code; }
            set
            {
                this.MutateVerbose(ref _code, value, RaisePropertyChanged());
            }
        }
        public String BarCode
        {
            get { return _barCode; }
            set
            {
                this.MutateVerbose(ref _barCode, value, RaisePropertyChanged());
            }
        }
        public String Name
        {
            get { return _name; }
            set
            {
                this.MutateVerbose(ref _name, value, RaisePropertyChanged());
            }
        }
        public String ProductGroup
        {
            get { return _productGroup; }
            set
            {
                this.MutateVerbose(ref _productGroup, value, RaisePropertyChanged());
            }
        }
        public ProductTypeEntity Type
        {
            get { return _type; }
            set
            {
                this.MutateVerbose(ref _type, value, RaisePropertyChanged());
            }
        }
        public ProductCategoryEntity Category
        {
            get { return _category; }
            set
            {
                this.MutateVerbose(ref _category, value, RaisePropertyChanged());
            }
        }
        public int AlertRed
        {
            get { return _alertRed; }
            set
            {
                this.MutateVerbose(ref _alertRed, value, RaisePropertyChanged());
            }
        }
        public int AlertYellow
        {
            get { return _alertYellow; }
            set
            {
                this.MutateVerbose(ref _alertYellow, value, RaisePropertyChanged());
            }
        }
        public int AlertGreen
        {
            get { return _alertGreen; }
            set
            {
                this.MutateVerbose(ref _alertGreen, value, RaisePropertyChanged());
            }
        }
        public int AlertBlue
        {
            get { return _alertBlue; }
            set
            {
                this.MutateVerbose(ref _alertBlue, value, RaisePropertyChanged());
            }
        }

        public ProductViewModel()
        {
            
        }

        public string Barcode2
        {
            get { return _barcode2; }
            set
            {
                this.MutateVerbose(ref _barcode2, value, RaisePropertyChanged());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}