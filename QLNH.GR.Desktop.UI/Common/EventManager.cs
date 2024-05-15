using QLNH.GR.Desktop.BO;
using QLNH.GR.Desktop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.UI
{
    public static class EventManager
    {
        public static event EventHandler<ToastEventArgs> ShowToastEvent;

        public static void RaiseShowToastEvent(object sender, ToastEventArgs e)
        {
            ShowToastEvent?.Invoke(sender, e);
        }
    }

    public class ToastEventArgs : EventArgs
    {
        public string Message { get; set; }
        public ToastType Type { get; set; }

        public ToastEventArgs(string message, ToastType type)
        {
            Message = message;
            Type = type;
        }
    }
}
