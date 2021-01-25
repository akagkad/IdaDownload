using IDAUtil.Model.Properties.ServerProperty;
using IDAUtil.Model.Properties.TaskProperty;
using System.Collections.Generic;

namespace IDAUtil.Service {
    public interface IDataCollectorServer {
        List<BankHolidayProperty> getBHList(string salesOrg);
        List<CriticalItemsDataProperty> getCriticalItemsDataList(string salesOrg);
        List<CustomerDataProperty> getCustomerDataList(string salesOrg);
        List<MM03Property> getMM03List(string salesOrg);
        List<RejectionsDataProperty> getRejectionsDataList(string salesOrg);
        List<RejectionsProperty> getRejectionsLogList(string salesOrg);
        List<SwitchesDataProperty> getSwitchesDataList(string salesOrg);
        List<SwitchesProperty> getSwitchesLogList(string salesOrg);
        List<SkuDataProperty> getSkuDataList(string salesOrg);
        List<DeliveryBlocksProperty> getDelBlockLogList(string salesOrg);
        List<AppointmentTimesSoldTo> getGBAppointmentTimesCustomers();
    }
}