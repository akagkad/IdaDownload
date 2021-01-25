using IDAUtil.Model.Properties.TaskProperty;
using IDAUtil.Model.Properties.ServerProperty;
using System.Collections.Generic;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;

namespace Rejections {
    public interface IDataCollectorServiceRejections {
        List<CustomerDataProperty> cdList { get; set; }
        List<RejectionsDataProperty> rdjList { get; set; }
        List<ZV04IProperty> zvList { get; set; }
        List<RejectionsProperty> getFinalListWithStockDetails(List<RejectionsProperty> list, string salesOrg);
        List<RejectionsProperty> getReleaseRejectionsListFromLog(string salesOrg);
        void populateReleaseRejectionList(string salesOrg);
    }
}