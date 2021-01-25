
namespace IDAUtil {
    public static class IDAConsts {
        //TODO: Add signature to all attachements
        public const string IDAFileSignature = "!D@";

        public const string adminEmail = "drybak@scj.com";

        public static class DelBlocks {
            public const string leadTimeBlock = "Z9";
            public const string holdOrderBlock = "Z8";
            public const string investigationPendingBlock = "Y2";
            public const string GOEBlock = "ZG";
            public const string AppointmentTimesBlock = "ZV";
            public const string belowMOQDelBlock = "ZF";
            public const string exportPaperMissingBlock = "Z4";
            public const string noBlock = " ";
        }

        public static class RejReasons {

        }

        public static class Paths {
            public const string errorFilePath = @"\\Gbfrimpf000\common\OPEX\OPERATIONAL EXCELLENCE\7.AUTOMATIONS\CF\IDA error log\";
        }
    }

}
