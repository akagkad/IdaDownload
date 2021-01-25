using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistressReport.Model {
    class ESDistressProperty {
        [Column("[Fecha de liberacion]")] public string releaseDate { get; set; } //Loading Date
        [Column("[Fecha de entrega]")] public string rdd { get; set; }
        [Column("[Pedido Johnson Wax]")] public int order { get; set; }
        [Column("[Numero de pedido cliente]")] public string poNumber { get; set; }
        [Column("[Plataforma]")] public string shipToName { get; set; }
        [Column("[SKU]")] public int sku { get; set; }
        [Column("[Dchain status]")] public string dChain { get; set; }
        [Column("[EAN]")] public string unitBarcode { get; set; }
        [Column("[ITF]")] public string caseBarcode { get; set; }
        [Column("[Descripcion]")] public string materialDescription { get; set; }
        [Column("[Comentarios]")] public string comments { get; set; }
        [Column("[Stock]")] public double atpQty { get; set; }
        [Column("[Próxima PO]")] public string recoveryDate { get; set; }
        [Column("[Cantidad de cajas a recibir]")] public double recoveryQty { get; set; }
        [Column("[Switch posible]")] public int possibleSwitch { get; set; }
        [Column("[Descripción del switch]")] public string possibleSwitchDescription { get; set; }
        [Column("[Cajas solicitadas]")] public double orderQty { get; set; }
        [Column("[Cajas entregadas]")] public double confirmedQty { get; set; }
        [Column("[Comercial]")] public string accountManager { get; set; }
        [Column("[cajas en rotura]")] public double cutQty { get; set; }

        public ESDistressProperty(GenericDistressProperty genericDistressProperty) {
            this.releaseDate = genericDistressProperty.loadingDate;
            this.rdd = genericDistressProperty.rdd;
            this.order = genericDistressProperty.order;
            this.poNumber = genericDistressProperty.poNumber;
            this.shipToName = genericDistressProperty.shipToName;
            this.sku = genericDistressProperty.material;
            this.dChain = genericDistressProperty.dChainStatus;
            this.caseBarcode = genericDistressProperty.caseBarcode;
            this.unitBarcode = genericDistressProperty.unitBarcode;
            this.materialDescription = genericDistressProperty.materialDescription;
            this.comments = genericDistressProperty.criticalItemComment;
            this.atpQty = genericDistressProperty.atp;
            this.recoveryDate = genericDistressProperty.recoveryDate;
            this.recoveryQty = genericDistressProperty.recoveryQty;
            this.possibleSwitch = genericDistressProperty.possibleSwitch;
            this.possibleSwitchDescription = genericDistressProperty.possibleSwitchDescription;
            this.orderQty = genericDistressProperty.orderQty;
            this.confirmedQty = genericDistressProperty.confirmedQty;
            this.accountManager = genericDistressProperty.accountManager;
            this.cutQty = genericDistressProperty.cutQty;
        }

        public override bool Equals(object obj) {
            return obj is ESDistressProperty property &&
                   releaseDate == property.releaseDate &&
                   rdd == property.rdd &&
                   order == property.order &&
                   poNumber == property.poNumber &&
                   shipToName == property.shipToName &&
                   sku == property.sku &&
                   dChain == property.dChain &&
                   caseBarcode == property.caseBarcode &&
                   unitBarcode == property.unitBarcode &&
                   materialDescription == property.materialDescription &&
                   comments == property.comments &&
                   atpQty == property.atpQty &&
                   recoveryDate == property.recoveryDate &&
                   recoveryQty == property.recoveryQty &&
                   possibleSwitch == property.possibleSwitch &&
                   possibleSwitchDescription == property.possibleSwitchDescription &&
                   orderQty == property.orderQty &&
                   confirmedQty == property.confirmedQty &&
                   accountManager == property.accountManager &&
                   cutQty == property.cutQty;
        }

        public override int GetHashCode() {
            var hashCode = 1704268500;
            hashCode = hashCode * -1521134295 + releaseDate.GetHashCode();
            hashCode = hashCode * -1521134295 + rdd.GetHashCode();
            hashCode = hashCode * -1521134295 + order.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(poNumber);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(shipToName);
            hashCode = hashCode * -1521134295 + sku.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(dChain);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(caseBarcode);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(unitBarcode);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(materialDescription);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(comments);
            hashCode = hashCode * -1521134295 + atpQty.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(recoveryDate);
            hashCode = hashCode * -1521134295 + recoveryQty.GetHashCode();
            hashCode = hashCode * -1521134295 + possibleSwitch.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(possibleSwitchDescription);
            hashCode = hashCode * -1521134295 + orderQty.GetHashCode();
            hashCode = hashCode * -1521134295 + confirmedQty.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(accountManager);
            hashCode = hashCode * -1521134295 + cutQty.GetHashCode();
            return hashCode;
        }
    }
}
