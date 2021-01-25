using IDAUtil.Model.Properties.ServerProperty;
using lib;
using Microsoft.VisualBasic;
using System.Data;
using System.Linq;

namespace IDAUtil.Service {
    public class DistributionListCalculator: IDistributionListCalculator {
        private readonly IDBServerConnector dbServer;

        //Constructor
        public DistributionListCalculator(IDBServerConnector dbServer) {
            this.dbServer = dbServer;
            dbServer.createConnectionToServerCF();
        }
        
        //Destructor
        ~DistributionListCalculator() {
            dbServer.closeConnection();
        }

        public string getDistList(string salesOrg, string task) {
            var list = dbServer.getObjectListByQuery<DistributionListProperty>($"SELECT [address] FROM DistributionListData WHERE salesOrg = '{salesOrg}' AND [name] = '{task}'");
            var listArray = list.Select(every => every.address).ToArray();
            return Strings.Join(listArray, ";");
        }
    }
}