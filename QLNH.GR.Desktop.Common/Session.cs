using QLNH.GR.Desktop.BO;

namespace QLNH.GR.Desktop.Common
{
    public static class Session
    {
        public static string ReccentDomain { get; set; }
        public static string UserName { get; set; }
        public static Guid? UserID { get; set; }
        public static string BranchID { get; set; }
        public static string Token { get; set; } = "";

        public static List<FeatureApp> ListFeatureApp { get; set; } = null;


        public static Guid? CurrenOrderId { get; set; }
        public static Order? CurrentOrderPayment { get; set; }

        public static AppPage? PreviousOrderPage { get; set; }
        public static bool IsCreateNewOrder { get; set; }
        public static  EnumOrderType? SelectingOrderType { get; set; }
        public static Guid? TableID { get; set; }
        public static string? TableName { get; set; }


    }
}