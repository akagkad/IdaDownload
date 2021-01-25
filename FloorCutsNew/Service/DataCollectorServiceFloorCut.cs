using IDAUtil;
using IDAUtil.Service;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using IDAUtil.Model.Properties.TcodeProperty.ZPURRSObj;
using IDAUtil.Model.Properties.TcodeProperty.MD04Obj;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;

using System;

namespace FloorCutsNew
{
    class DataCollectorServiceFloorCut
    {

        private readonly DataCollectorServer dataCollectorServer;
        private readonly DataCollectorSap dataCollectorSap;
        private int aggregate;
        public DataCollectorServiceFloorCut(DataCollectorServer dataCollectorServer, DataCollectorSap dataCollectorSap)
        {
            this.dataCollectorServer = dataCollectorServer;
            this.dataCollectorSap = dataCollectorSap;
        }

        public List<int> getpastPOdates(string salesOrg)
        {

            List<ZPURRSProperty> PList1 = dataCollectorSap.getZPURRSList(salesOrg, IDAEnum.Task.pastPOdate);

            //var PList3 = dataCollectorSap.getZV04IList(salesOrg,IDAEnum.Task.distress);

            if (PList1 is null) { return null; }
            var query1 = PList1.Select(o => o.Material).Distinct().ToList();

            return query1;
        }

        public List<MD04Property> getMD04(string item, List<MD04Property> listMD04, string salesOrg)
        {

            List<MD04Property> PList2 = dataCollectorSap.getMD04List(item, IDAEnum.Task.MD04report, listMD04, salesOrg);
            return PList2;
        }

        public List<ZV04IProperty> checkStock(List<MD04Property> MD04List, string salesOrg)
        {
            int aggregate = 0;

            var PList3 = dataCollectorSap.getZV04IfloorCuts(MD04List, salesOrg, IDAEnum.Task.distress);

            //aggregate = subList[0].AvailableQty;
            //var query2 = (from p in subList
            //             where ( DateTime.Today<=p.Date) &&  (p.MRPelement=="Delvry" || p.MRPelement == "CuOrd")
            //              select new MD04Property(p.AddlFunc, p.Date, p.MRPelement, p.MRPelementData, p.ReschedulingDate, p.Exception, p.Reqmt, p.AvailableQty, p.Location)
            //            ).ToList();

            //if (PList3 is null) { return null; }
            //// var query1 = PList3.Select(o =>  MD04List.Contains(o.item)).Distinct().ToList();
            //var query = (from zv04ilist in PList3
            //             join skuList in MD04List on new { key0 = zv04ilist.material} equals new { key0 = skuList}

            //             select new ZV04IProperty(zv04ilist.material)
            //             ).ToList();
            return PList3;
        }

    }
}
