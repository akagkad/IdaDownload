using lib;
using Microsoft.VisualBasic;
using System;

namespace IDAUtil.Support {
    public class GlobalErrorHandler {
            private static readonly IMailUtil mailUtil = Create.mailUtil();
            private static readonly IFileUtil fileUtil = Create.fileUtil();
        public static void handle(string salesOrg, string task, Exception ex) {
            string filepath = $@"{IDAConsts.Paths.errorFilePath}\{salesOrg} {task} error {Strings.Format(DateTime.Now, "yyyy.MM.dd mm.hh")}.txt";

            fileUtil.writeToTxtFile(filepath, ex.Source + Constants.vbCr + ex.Message + Constants.vbCr + ex.StackTrace);
            mailUtil.mailSimple($"{IDAConsts.adminEmail};{Environment.UserName}", $"{salesOrg} {task}: Error {Information.Err().Description}", $"{mailUtil.getLink(filepath, "Your error info is here")}");
        }
    }
}