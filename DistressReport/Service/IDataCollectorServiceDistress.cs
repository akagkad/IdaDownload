using DistressReport.Model;
using System.Collections.Generic;

namespace DistressReport.Service {
    interface IDataCollectorServiceDistress {
        List<GenericDistressProperty> getDistressList(string salesOrg);
    }
}