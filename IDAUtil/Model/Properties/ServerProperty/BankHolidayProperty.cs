using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDAUtil.Model.Properties.ServerProperty {
    public class BankHolidayProperty {
        [Column("[salesOrg]")] public string salesOrg { get; set; }
        [Column("[country]")] public string country { get; set; }
        [Column("[nationalDate]")] public DateTime nationalDate { get; set; }
        [Column("[region]")] public string region { get; set; }

        public BankHolidayProperty() { }
        public BankHolidayProperty(string salesOrg, string country, DateTime nationalDate, string region) {
            this.salesOrg = salesOrg;
            this.country = country;
            this.nationalDate = nationalDate;
            this.region = region;
        }

        public override bool Equals(object obj) {
            return obj is BankHolidayProperty property &&
                   salesOrg == property.salesOrg &&
                   country == property.country &&
                   nationalDate == property.nationalDate &&
                   region == property.region;
        }

        public override int GetHashCode() {
            var hashCode = -303098340;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(salesOrg);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(country);
            hashCode = hashCode * -1521134295 + nationalDate.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(region);
            return hashCode;
        }
    }
}