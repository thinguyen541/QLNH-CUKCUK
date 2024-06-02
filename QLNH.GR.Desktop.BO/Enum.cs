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
        Menu,
        OrderList,
        PaymentScreen,
        Receipt,
        Transaction
    }

    public enum ToastType
    {
        Success,
        Warning
    }

    public enum EnumOperator
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

    public enum EnumPropertyType
    {
        isString,
        isInt,
        isBool,
        isDate,
        isGuid
    }

    public enum EnumRelationType
    {
        AND,
        OR
    }

    public enum EnumSortOrder
    {
        ASC,
        DESC
    }

    public enum EnumOrderStatus
    {
        Serving,
        Fired,
        Canceled,
        Hold,
        Done
    }

    public enum EnumOrderType
    {
        DineIn,
        Delivery,
        Pickup
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
        Hold,
        Done
    }
    public enum EnumCardType
    {
        Cash,
        Card,
        Papal,
        Vemmo,
        OtherCard
    }
    public enum EnumDetailItemType
    {
        Normal,
        Group,
        Modifier,
        Combo,
        Promotion
    }

   
}
