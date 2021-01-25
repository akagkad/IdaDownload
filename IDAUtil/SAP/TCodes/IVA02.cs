using lib;

namespace IDAUtil.SAP.TCode {
    public interface IVA02 {
        void bypassInitialPopups();
        OrderStatus checkIfOrderBlocked(int orderNumber);
        OrderStatus enterOrder(int order);
        string getChangedListForCSR(QtyConversionOrderProperty order);
        int getFirstEmptyRowIndex(ITable table);
        OrderStatus getOrderStatusAfterSaving();
        ITable getTable();
        OrderStatus isChangeNeeded();
        bool isLineChanged(ITable table, SwitchesSapLineProperty lineSwitch, int sapLineNumber, ref string reason);
        void moveRejectionCodeColumnToIndexEight(ITable table);
        void moveRouteCodeColumnToIndexEight(ITable table);
        void putBackPreviousSku(ITable table, SwitchesSapLineProperty lineSwitch, int sapLineNumber);
        void save();
        void setCSRNotes(string csr);
        void soarAction(string csrNote, string taskName, int OrderNumber);
        void tryToOpenCSRNotes();
        void tryToSellectAllLines();
        void updateLog(OrderStatus status, string tableName, string orderNumber, string id);
        void updateOrderSavedLog(string tableName, int orderNumber, string id);
    }
}