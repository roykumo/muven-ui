using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.entity
{
    public class ProductEntity
    {
        public String Id { get; set; }
        public String BarCode { get; set; }
        public String Name { get; set; }
        public ProductTypeEntity Type { get; set; }
        public int AlertRed { get; set; }
        public int AlertYellow { get; set; }
        public int AlertGreen { get; set; }
        public String Alerts
        {
            get { return AlertRed + "/" + AlertYellow + "/" + AlertGreen; }
        }
    }
}
