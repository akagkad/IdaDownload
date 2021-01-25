using IDAUtil.Model.Properties.ServerProperty;
using IDAUtil.Model.Properties.TaskProperty;
using lib;
using System;
using System.Collections.Generic;

namespace IDAUtil.Service {
    public class DataCollectorServer : IDataCollectorServer {
        private readonly IDBServerConnector dbServer;

        public DataCollectorServer(IDBServerConnector dbServer) {
            this.dbServer = dbServer;
          dbServer.createConnectionToServerCF();
        }

        public List<CustomerDataProperty> getCustomerDataList(string salesOrg) {
            string query = $"SELECT * FROM CustomerData WHERE salesOrg = '{salesOrg}'";
            return dbServer.getObjectListByQuery<CustomerDataProperty>(query);
        }

        public List<SwitchesDataProperty> getSwitchesDataList(string salesOrg) {
            string query = $"SELECT * FROM SwitchesData WHERE salesOrg = '{salesOrg}'";
            return dbServer.getObjectListByQuery<SwitchesDataProperty>(query);
        }

        public List<RejectionsDataProperty> getRejectionsDataList(string salesOrg) {
            string query = $"SELECT * FROM RejectionsData WHERE salesOrg = '{salesOrg}'";
            return dbServer.getObjectListByQuery<RejectionsDataProperty>(query);
        }

        public List<RejectionsProperty> getRejectionsLogList(string salesOrg) {
            string query = $"Select * from RejectionsLog where [salesOrg] = '{salesOrg}' AND (status is null or status <> 'success') AND isDuringRelease = 0 AND [endTime] is null";
            return dbServer.getObjectListByQuery<RejectionsProperty>(query);
        }

        public List<SwitchesProperty> getSwitchesLogList(string salesOrg) {
            string query = $"Select * from SwitchLog where [salesOrg] = '{salesOrg}' AND [endTime] is null AND [id] like '%{DateTime.Today.ToString().Replace(" 00:00:00", "")}%' ";
            return dbServer.getObjectListByQuery<SwitchesProperty>(query);
        }

        public List<BankHolidayProperty> getBHList(string salesOrg) {
            if (!((salesOrg ?? "") == "NL01")) {
                return dbServer.getObjectListByQuery<BankHolidayProperty>($"SELECT * FROM BankHoliday WHERE salesOrg = '{salesOrg}'");
            } else {
                return dbServer.getObjectListByQuery<BankHolidayProperty>($"SELECT * FROM BankHoliday WHERE salesOrg = '{salesOrg}' OR salesOrg = 'DE01'");
            }
        }

        public List<MM03Property> getMM03List(string salesOrg) {
            string query = $"SELECT [material],[organisation],[unitEAN],[caseEAN],[dChainStatus],[unitPerCase] FROM frimcfs.dbo.MM03 WHERE organisation = '{salesOrg}'";
            return dbServer.getObjectListByQuery<MM03Property>(query);
        }

        public List<CriticalItemsDataProperty> getCriticalItemsDataList(string salesOrg) {
            string query = $"Select * from CriticalItems where [salesOrg] = '{salesOrg}'";
            return dbServer.getObjectListByQuery<CriticalItemsDataProperty>(query);
        }

        public List<SkuDataProperty> getSkuDataList(string salesOrg) {
            string query = $"Select * from SkuData where [salesOrg] = '{salesOrg}'";
            return dbServer.getObjectListByQuery<SkuDataProperty>(query);
        }

        public List<DeliveryBlocksProperty> getDelBlockLogList(string salesOrg) {
            string query = $"Select distinct * from DeliveryBlockLog where [salesOrg] = '{salesOrg}'";
            return dbServer.getObjectListByQuery<DeliveryBlocksProperty>(query);
        }

        public List<AppointmentTimesSoldTo> getGBAppointmentTimesCustomers() {
            string query = $"Select * from AppointmentTimesSoldTo";
            return dbServer.getObjectListByQuery<AppointmentTimesSoldTo>(query);
        }
    }
}