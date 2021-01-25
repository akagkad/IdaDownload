using lib;
using System.Collections.Generic;

namespace KIWI {
    public class ConversionDAO {
        private readonly IDBServerConnector dbServer;

        public ConversionDAO(IDBServerConnector dbServer) {
            this.dbServer = dbServer;
        }

        public List<ConversionMaterial> getQuantityConversionMaterialList() {
            dbServer.createConnectionToServerCF();
            List<ConversionMaterial> conversionMaterialList;
            conversionMaterialList = dbServer.getObjectListByQuery<ConversionMaterial>("SELECT * FROM QuantityConversionMaterial");
            dbServer.closeConnection();
            return conversionMaterialList;
        }

        public List<ConversionShiptTo> getQuantityConversionShipToList() {
            dbServer.createConnectionToServerCF();
            List<ConversionShiptTo> conversionShipToList;
            conversionShipToList = dbServer.getObjectListByQuery<ConversionShiptTo>("SELECT * FROM QuantityConversionShipTo");
            dbServer.closeConnection();
            return conversionShipToList;
        }

        public List<ConversionLog> getQuantityConversionLogList(long fromDocument, long toDocument) {
            dbServer.createConnectionToServerCF();
            List<ConversionLog> conversionLogList;
            conversionLogList = dbServer.getObjectListByQuery<ConversionLog>($"SELECT * FROM QuantityConversionLog WHERE orderNumber >= {fromDocument} AND orderNumber <= {toDocument}");
            dbServer.closeConnection();
            return conversionLogList;
        }
    }
}