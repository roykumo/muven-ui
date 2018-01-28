using CakeGUI.classes.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CakeGUI.classes.util
{
    public class Utils
    {
        public static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private static List<ProductGroupEntity> listProductGroup=null;
        public static List<ProductGroupEntity> ListProductGroup
        {
            get
            {
                if (listProductGroup == null)
                {
                    listProductGroup = new List<ProductGroupEntity>();
                    ProductGroupEntity groupNonRepacking = new ProductGroupEntity();
                    groupNonRepacking.Code = "NON";
                    groupNonRepacking.Description = "Non Repacking";

                    ProductGroupEntity groupRepacking = new ProductGroupEntity();
                    groupRepacking.Code = "RPCK";
                    groupRepacking.Description = "Repacking";

                    ProductGroupEntity groupBulk = new ProductGroupEntity();
                    groupBulk.Code = "BULK";
                    groupBulk.Description = "Bulk";

                    listProductGroup.Add(groupNonRepacking);
                    listProductGroup.Add(groupRepacking);
                    listProductGroup.Add(groupBulk);
                }

                return listProductGroup;
            }
        }

        public static List<ProductGroupEntity> ListProductGroupWithBlank
        {
            get
            {
                List<ProductGroupEntity> list = ListProductGroup;
                ProductGroupEntity blankGroup = new ProductGroupEntity();
                blankGroup.Code = "";
                blankGroup.Description = "All";
                list.Insert(0, blankGroup);

                return list;
            }
        }
    }
}
