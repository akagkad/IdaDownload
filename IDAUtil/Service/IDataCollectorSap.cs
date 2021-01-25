using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using System.Collections.Generic;

namespace IDAUtil.Service {
    public interface IDataCollectorSap {
        List<ZV04HNProperty> getZV04HNList(string salesOrg, IDAEnum.Task idaTask);
        List<ZV04IProperty> getZV04IList(string salesOrg, IDAEnum.Task idaTask);
        List<ZV04PProperty> getZV04PList(string salesOrg, IDAEnum.Task idaTask);
    }
}