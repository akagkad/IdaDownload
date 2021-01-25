
namespace RDD {
    public static class BHUtil {
        public static bool isSkipWeekend(string salesOrg) {
            switch (salesOrg) {
                case "RU01":
                case "UA01": {
                        return false;
                    }

                default: {
                        return true;
                    }
            }
        }
    }
}