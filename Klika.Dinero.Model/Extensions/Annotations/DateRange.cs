using System;
using System.ComponentModel.DataAnnotations;

namespace Klika.Dinero.Model.Extensions.Annotations
{
    public class DateRange : ValidationAttribute
    {
        private string _minimum { get; set; }
        public DateRange(string minimum)
        {
            _minimum = minimum; 
        }

        public override bool IsValid(object value)
        {
            if(value != null)
            {
                var date = (DateTime)value;

                if (date <= DateTime.Now && date >= DateTime.Parse(_minimum))
                    return true;

                return false;
            }
            return true;
        }
    }
}
