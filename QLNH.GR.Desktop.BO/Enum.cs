using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO
{
    public enum AppPage
    {
        Login,
        LoginBranch,
        MainScreen,
        Order,
        Table,
        Eployee,
        Menu
    }

    public enum ToastType
    {
        Success,
        Warning
    }

    public enum Operator
    {
        LIKE,
        EQUAL,
        NOTEQUAL,
        LESSTHAN,
        GREATERTHAN,
        LESSTHANOREQUAL,
        GREATERTHANOREQUAL,
        NOTLIKE,
        STARTWITH,
        ENDWITH,
    }

    public enum EntityMode
    {
        Add,
        Edit,
        Delete
    }

    public enum PropertyType
    {
        isString,
        isInt,
        isBool,
        isDate,
        isGuid
    }

    public enum RelationType
    {
        AND,
        OR
    }

    public enum SortOrder
    {
        ASC,
        DESC
    }

    public enum EnumOrderStatus
    {
        Serving,
        Canceled,
        Hold
    }
    public enum EnumPaymentStatus
    {
        NotPayment,
        PaidAll,
        PartialPaid
    }

    public enum EnumOrderDetailStatus
    {
        NotSentKitchen,
        Send,
        Hold
    }
    public enum EnumDetailItemType
    {
        Normal,
        Group,
        Modifiere,
        Combo,
        Promotion
    }


}
