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

        private static List<MonthEntity> listMonth;
        public static List<MonthEntity> ListMonth
        {
            get
            {
                if (listMonth == null)
                {
                    listMonth = new List<MonthEntity>();
                    MonthEntity jan = new MonthEntity();
                    jan.Id = 1;
                    jan.Description = "Januari";

                    MonthEntity feb = new MonthEntity();
                    feb.Id = 2;
                    feb.Description = "Februari";

                    MonthEntity mar = new MonthEntity();
                    mar.Id = 3;
                    mar.Description = "Maret";

                    MonthEntity apr = new MonthEntity();
                    apr.Id = 4;
                    apr.Description = "April";

                    MonthEntity mei = new MonthEntity();
                    mei.Id = 5;
                    mei.Description = "Mei";

                    MonthEntity jun = new MonthEntity();
                    jun.Id = 6;
                    jun.Description = "Juni";

                    MonthEntity jul = new MonthEntity();
                    jul.Id = 7;
                    jul.Description = "Juli";

                    MonthEntity agu = new MonthEntity();
                    agu.Id = 8;
                    agu.Description = "Agustus";

                    MonthEntity sep = new MonthEntity();
                    sep.Id = 9;
                    sep.Description = "September";

                    MonthEntity okt = new MonthEntity();
                    okt.Id = 10;
                    okt.Description = "Oktober";

                    MonthEntity nov = new MonthEntity();
                    nov.Id = 11;
                    nov.Description = "November";

                    MonthEntity des = new MonthEntity();
                    des.Id = 12;
                    des.Description = "Desember";

                    listMonth.Add(jan);
                    listMonth.Add(feb);
                    listMonth.Add(mar);
                    listMonth.Add(apr);
                    listMonth.Add(mei);
                    listMonth.Add(jun);
                    listMonth.Add(jul);
                    listMonth.Add(agu);
                    listMonth.Add(sep);
                    listMonth.Add(okt);
                    listMonth.Add(nov);
                    listMonth.Add(des);
                }

                return listMonth;
            }
        }

        public static List<DateEntity> ListDayByMonth(int year, int month)
        {
            //return Enumerable.Range(1, DateTime.DaysInMonth(year, month)).ToList();
            int days = DateTime.DaysInMonth(year, month);

            List<DateEntity> listDay = new List<DateEntity>();
            DateEntity dateEntityAll = new DateEntity();
            dateEntityAll.Id = 0;
            dateEntityAll.Description = "All";
            listDay.Add(dateEntityAll);

            for(int i = 1; i <= days; i++)
            {
                DateEntity dateEntity = new DateEntity();
                dateEntity.Id = i;
                dateEntity.Description = i.ToString();

                listDay.Add(dateEntity);
            }

            return listDay;
        }
    }
}
