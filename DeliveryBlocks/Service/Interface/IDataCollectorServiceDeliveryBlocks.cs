using DeliveryBlocks.Model;
using IDAUtil.Model.Properties.ServerProperty;
using lib;
using System.Collections.Generic;

namespace DeliveryBlocks.Service {
    public interface IDataCollectorServiceDeliveryBlocks {
        List<DeliveryBlocksProperty> getDelBlockList();
    }
}