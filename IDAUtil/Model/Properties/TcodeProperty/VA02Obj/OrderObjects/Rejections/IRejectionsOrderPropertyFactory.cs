using IDAUtil.Model.Properties.TcodeProperty.VA02;
using System.Collections.Generic;

namespace IDAUtil.Model.Properties.TaskProperty {
    public interface IRejectionsOrderPropertyFactory {
        List<RejectionsSapOrderProperty> getSapRejectionsObjectList(List<RejectionsProperty> rpl);
    }
}