using System.Collections.Generic;

namespace IDAUtil.Model.Properties.TcodeProperty.VA02 {
    public class RejectionsSapOrderProperty {
        public string salesOrg { get; set; }
        public int orderNumber { get; set; }
        public List<RejectionsSapLineProperty> lineDetails { get; set; }

        public RejectionsSapOrderProperty() { }
    }

    public class RejectionsSapLineProperty {
        public int sku { get; set; }
        public int lineNumber { get; set; }
        public double orderedQty { get; set; }
        public double confirmedQty { get; set; }
        public string rejectionCode { get; set; }
        public bool isReplacePartialCut { get; set; }
        public string reason { get; set; }

        public RejectionsSapLineProperty() { }
    }
}