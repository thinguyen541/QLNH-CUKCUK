using QLNH.GR.Desktop.BO;

namespace QLNH.GR.Desktop.Common
{
    public static class Session
    {
        public static string ReccentDomain { get; set; } 
        public static string UserName { get; set; }
        public static string UserID { get; set; }
        public static string BranchID { get; set; }
        public static string Token { get; set; } = "";

        public static List<FeatureApp> ListFeatureApp { get; set; } = null;

}
}