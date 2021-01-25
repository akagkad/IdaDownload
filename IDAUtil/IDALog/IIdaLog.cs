using ADODB;

namespace IDAUtil {
    public interface IIdaLog {
        bool insert(string tableName, string columnNames, string values);
        void insertToActivityLog(string taskName, string status, string id, string salesOrg);
        void populateFromRs(Recordset rs);
        void update(string tableName, string[] columnNames, string[] values, string[] conditionName, string[] conditionValue, string customCondition = "");
    }
}