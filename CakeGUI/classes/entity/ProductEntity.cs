﻿using Newtonsoft.Json;
using RestSharp.Deserializers;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.entity
{
    public class ProductEntity
    {
        //[DeserializeAs(Name = "id"), SerializeAs(Name ="id")]
        [JsonProperty("id")]
        public String Id { get; set; }
        [JsonProperty("code")]
        public String Code { get; set; }
        //[DeserializeAs(Name = "barcode"), SerializeAs(Name ="barcode")]
        [JsonProperty("barcode")]
        public String BarCode { get; set; }
        //[DeserializeAs(Name = "name"), SerializeAs(Name = "name")]
        [JsonProperty("name")]
        public String Name { get; set; }
        [JsonProperty("productGroup")]
        public String ProductGroup { get; set; }
        //[DeserializeAs(Name = "type"), SerializeAs(Name = "type")]
        [JsonProperty("category")]
        public ProductCategoryEntity Category { get; set; }
        //[DeserializeAs(Name = "alertRed"), SerializeAs(Name = "alertRed")]
        [JsonProperty("alertRed")]
        public int AlertRed { get; set; }
        //[DeserializeAs(Name = "alertYellow"), SerializeAs(Name = "alertYellow")]
        [JsonProperty("alertYellow")]
        public int AlertYellow { get; set; }
        //[DeserializeAs(Name = "alertGreen"), SerializeAs(Name = "alertGreen")]
        [JsonProperty("alertGreen")]
        public int AlertGreen { get; set; }
        [JsonProperty("alertBlue")]
        public int AlertBlue { get; set; }
        public String Alerts
        {
            get { return AlertRed + "/" + AlertYellow + "/" + AlertGreen + "/" + AlertBlue; }
        }

        public ProductEntity()
        {
            AlertRed = 0;
            AlertYellow = 30;
            AlertGreen = 60;
            AlertBlue = 180;
        }

        public string CodeView
        {
            get
            {
                if (string.IsNullOrEmpty(Code))
                {
                    return "";
                }
                else
                {
                    string[] codes = Code.Split('~');
                    string code = "";
                    if (codes.Length == 1)
                    {
                        code = codes[0];
                    }
                    else if (codes.Length == 2)
                    {
                        code = codes[0] + "_" + codes[1];
                    }else if (codes.Length > 2)
                    {
                        code = codes[0] + "_" + codes[1] + " " + codes[2];
                    }

                    return code;
                }
            }
        }
    }
}
