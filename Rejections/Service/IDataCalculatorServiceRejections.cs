using IDAUtil.Model.Properties.TaskProperty;
using System.Collections.Generic;

namespace Rejections.Service {
    public interface IDataCalculatorServiceRejections {
        List<RejectionsProperty> getRejectionsList(IDataCollectorServiceRejections dcsr);
    }
}