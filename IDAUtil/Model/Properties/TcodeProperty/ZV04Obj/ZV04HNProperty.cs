﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDAUtil.Model.Properties.TcodeProperty.ZV04Obj {
    public class ZV04HNProperty {
        [Column("[status]")] public string status { get; set; }
        [Column("[order]")] public int order { get; set; }
        [Column("[delivery]")] public int delivery { get; set; }
        [Column("[shipment]")] public string shipment { get; set; }
        [Column("[docTyp]")] public string docTyp { get; set; }
        [Column("[docDate]")] public DateTime docDate { get; set; }
        [Column("[delBlock]")] public string delBlock { get; set; }
        [Column("[delBlockDesc]")] public string delBlockDesc { get; set; }
        [Column("[ordNetValue]")] public double ordNetValue { get; set; }
        [Column("[reqDelDate]")] public DateTime reqDelDate { get; set; }
        [Column("[pricingDate]")] public string pricingDate { get; set; }
        [Column("[pODate]")] public string pODate { get; set; }
        [Column("[loadingDate]")] public DateTime loadingDate { get; set; }
        [Column("[plant]")] public string plant { get; set; }
        [Column("[orderQty]")] public double orderQty { get; set; }
        [Column("[confirmedQty]")] public double confirmedQty { get; set; }
        [Column("[deliveryQty]")] public double deliveryQty { get; set; }
        [Column("[deliveryNetValue]")] public double deliveryNetValue { get; set; }
        [Column("[FillRate]")] public double FillRate { get; set; }
        [Column("[pONumber]")] public string pONumber { get; set; }
        [Column("[soldto]")] public int soldto { get; set; }
        [Column("[soldtoName]")] public string soldtoName { get; set; }
        [Column("[salesOffice]")] public string salesOffice { get; set; }
        [Column("[salesGroup]")] public string salesGroup { get; set; }
        [Column("[salesDistrict]")] public string salesDistrict { get; set; }
        [Column("[backorderIndicator]")] public string backorderIndicator { get; set; }
        [Column("[shipto]")] public int shipto { get; set; }
        [Column("[shiptoName]")] public string shiptoName { get; set; }
        [Column("[cSRASR]")] public string cSRASR { get; set; }
        [Column("[csrasrName]")] public string csrasrName { get; set; }
        [Column("[scjAgent]")] public string scjAgent { get; set; }
        [Column("[scjAgentName]")] public string scjAgentName { get; set; }
        [Column("[scjAgentReg]")] public string scjAgentReg { get; set; }
        [Column("[scjAgentRegName]")] public string scjAgentRegName { get; set; }
        [Column("[orderTlfp]")] public double orderTlfp { get; set; }
        [Column("[orderPlt]")] public double orderPlt { get; set; }
        [Column("[orderLayers]")] public double orderLayers { get; set; }
        [Column("[orderLooseCs]")] public double orderLooseCs { get; set; }
        [Column("[orderTotCs]")] public double orderTotCs { get; set; }
        [Column("[tlfpCmmt]")] public double tlfpCmmt { get; set; }
        [Column("[pltCmmt]")] public double pltCmmt { get; set; }
        [Column("[layersCmmt]")] public double layersCmmt { get; set; }
        [Column("[looseCsCmmt]")] public double looseCsCmmt { get; set; }
        [Column("[totCsCmmt]")] public double totCsCmmt { get; set; }
        [Column("[headerWeight]")] public double headerWeight { get; set; }
        [Column("[wtUnit]")] public string wtUnit { get; set; }
        [Column("[headerCube]")] public double headerCube { get; set; }
        [Column("[volUnit]")] public string volUnit { get; set; }
        [Column("[ediSummaryData]")] public string ediSummaryData { get; set; }
        [Column("[zv04Report]")] public string zv04Report { get; set; }
        [Column("[ordDesc]")] public string ordDesc { get; set; }
        [Column("[description]")] public string description { get; set; }
        [Column("[createdBy]")] public string createdBy { get; set; }
        [Column("[route]")] public string route { get; set; }
        [Column("[shipToPostalCode]")] public string shipToPostalCode { get; set; }
        [Column("[shipToRegion]")] public string shipToRegion { get; set; }
        [Column("[shipToRegionName]")] public string shipToRegionName { get; set; }
        [Column("[shipToCity]")] public string shipToCity { get; set; }
        [Column("[shipToStreet]")] public string shipToStreet { get; set; }
        [Column("[soldToPostalCode]")] public string soldToPostalCode { get; set; }
        [Column("[soldToRegion]")] public string soldToRegion { get; set; }
        [Column("[soldToRegionName]")] public string soldToRegionName { get; set; }
        [Column("[soldToCity]")] public string soldToCity { get; set; }
        [Column("[soldToStreet]")] public string soldToStreet { get; set; }
        [Column("[realOpenOrderQty]")] public string realOpenOrderQty { get; set; }
        [Column("[realOpenOrderNetValue]")] public double realOpenOrderNetValue { get; set; }
        [Column("[appointmentTimes]")] public string appointmentTimes { get; set; }
        [Column("[csrNotes]")] public string csrNotes { get; set; }
        [Column("[ediTransmittedText]")] public string ediTransmittedText { get; set; }
        [Column("[deliveryInstructions]")] public string deliveryInstructions { get; set; }
        [Column("[deliveryNote2]")] public string deliveryNote2 { get; set; }
        [Column("[carrierInstructions]")] public string carrierInstructions { get; set; }
    }
}