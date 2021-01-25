using lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSumn {
    class Program {
        static void Main(string[] args) {

            string[] salesOrgArr = { "FR01", "NL01", "DE01", "IT01", "GR01", "PL01", "CZ01", "PT01", "ES01", "RO01", "GB01", "TR01", "UA01", "RU01", "ZA01", "KE02", "NG01","SA01","EG01","EG01" };
            int myMinute = 10;
            foreach (var item in salesOrgArr) {
                
                executeZV04HN(item, myMinute);
                myMinute++;
                executeWE05XL(item, myMinute);
                myMinute++;
            }
            
        }

        private static void executeZV04HN(string salesOrg, int timeTime) {
            var f = Create.fileSystem();
            var q = f.getFilesInfo($"K:\\SOAR\\OTD\\DOMESTIC SOAR\\{salesOrg}\\ZV04HN", "17-06-2020 14*");
            var d = DateTime.Parse($"17/06/2020 14:{timeTime.ToString()}:50");
            changeFileDate(q[0], d);
        }

        private static void executeWE05XL(string salesOrg, int timeTime) {
            var f = Create.fileSystem();
            var q = f.getFilesInfo($"K:\\SOAR\\OTD\\DOMESTIC SOAR\\{salesOrg}\\WE05", "17-06-2020 14*");
            var d = DateTime.Parse($"17/06/2020 14:{timeTime.ToString()}:50");
            try {
                changeFileDate(q[0], d);
                changeFileDate(q[1], d);
            } catch (Exception) { }
        }

        static void changeFileDate(FileInfo fileInfo, DateTime d) {
            fileInfo.CreationTime = d.AddHours(1);
            fileInfo.CreationTimeUtc = d;
            fileInfo.LastAccessTime = d.AddHours(1);
            fileInfo.LastAccessTimeUtc = d;
            fileInfo.LastWriteTime = d.AddHours(1);
            fileInfo.LastWriteTimeUtc = d;
        }
    }
}
