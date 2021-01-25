using lib;
using Microsoft.VisualBasic;
using System;
using System.Linq;

namespace IDAUtil {
    public class IdaLog : IIdaLog {
        private readonly IDBServerConnector db = Create.dbServer();

        public IdaLog() {
            db.createConnectionToServerCF();
        }

        ~IdaLog() {
            db.closeConnection();
        }

        public void insertToActivityLog(string taskName, string status, string id, string salesOrg) {
            db.createConnectionToServerCF();
            db.executeQuery($"INSERT INTO ActivityLog ([id],[taskName],[salesOrg],[taskStatus],[updateTime],[pcName]) VALUES ('{id}','{taskName}','{salesOrg}','{status}','{DateTime.Now.ToDBFormat()}','{Environment.MachineName}')");
        }

        public void populateFromRs(ADODB.Recordset rs) {
            db.rsToServer(rs, "DeliveryDatesLog");
        }

        public void update(string tableName, string[] columnNames, string[] values, string[] conditionName, string[] conditionValue, string customCondition = "") {
            var db = Create.dbServer();
            string columnsAndValuesStr = "";
            string conditionStr = "";
            db.createConnectionToServerCF();
            for (int i = 0, loopTo = columnNames.Count() - 1; i <= loopTo; i++) {
                columnsAndValuesStr += $"[{columnNames[i]}] = '{values[i]}'";
                if (!(i == columnNames.Count() - 1))
                    columnsAndValuesStr += ", ";
            }

            for (int i = 0, loopTo1 = conditionName.Count() - 1; i <= loopTo1; i++) {
                conditionStr += $"{conditionName[i]} = {Interaction.IIf(Information.IsNumeric(conditionValue[i]), conditionValue[i], "'" + conditionValue[i] + "'")}";
                if (!(i == conditionName.Count() - 1))
                    conditionStr += " AND ";
            }

            if (!string.IsNullOrEmpty(customCondition)) {
                customCondition = " AND " + customCondition;
                conditionStr += customCondition;
            }

            string query;
            query = $"UPDATE {tableName} SET {columnsAndValuesStr} WHERE {conditionStr}";
            db.executeQuery(query);
            db.closeConnection();
        }

        public bool insert(string tableName, string columnNames, string values) {
            try {
                string query;
                query = $"INSERT INTO {tableName} ({columnNames}) VALUES ({values})";
                db.executeQuery(query);
                return true;
            } catch (Exception) {
                return false;
            }
        }
    }
}