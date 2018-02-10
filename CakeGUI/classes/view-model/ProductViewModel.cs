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
