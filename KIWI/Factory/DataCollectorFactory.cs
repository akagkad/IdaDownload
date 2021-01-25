using IDAUtil;
using IDAUtil.Service;
using lib;

namespace KIWI {
    static class DataCollectorFactory {
        public static DataCollectorSap createDataCollectorSAP(string salesOrg) {
            return new DataCollectorSap((ISAPLib)new ZV04I(Create.sapLib(), salesOrg, IDAEnum.Task.quantityConversion), Create.exportParses());
        }
    }
}