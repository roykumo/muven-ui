﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CakeGUI.classes.entity.validation
{
    public class NotEmptyValidationRules : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
            //? new ValidationResult(false, "Field is required.")
            ? new ValidationResult(false, null)
            : ValidationResult.ValidResult;
        }
    }
}