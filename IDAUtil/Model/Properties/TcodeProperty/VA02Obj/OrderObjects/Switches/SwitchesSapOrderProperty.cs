using System.Collections.Generic;

namespace IDAUtil {
    public class SwitchesSapOrderProperty {
        public string salesOrg { get; set; }
        public int orderNumber { get; set; }
        public int soldTo { get; set; }

        //this is the list of switch objects that belong to the same order 
        public List<SwitchesSapLineProperty> lineDetails { get; set; }

        public SwitchesSapOrderProperty() {
        }
    }

    public class SwitchesSapLineProperty {
        public int oldSku { get; set; }
        public int lineNumber { get; set; }
        public bool isSameBarcode { get; set; }
        public string reason { get; set; }
        public int newSku { get; set; }

        public SwitchesSapLineProperty() {
        }
    }
}