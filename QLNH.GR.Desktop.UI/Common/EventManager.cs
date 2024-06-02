using QLNH.GR.Desktop.BO;
using QLNH.GR.Desktop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QLNH.GR.Desktop.UI
{
    public static class EventManager
    {
        public static event EventHandler<ToastEventArgs> ShowToastEvent;

        public static event EventHandler<DialogEventArgs> ShowDialogEvent;

        public static void RaiseShowToastEvent(object sender, ToastEventArgs e)
        {
            ShowToastEvent?.Invoke(sender, e);
        }

        public static void RaiseShowDialogEvent(object sender, DialogEventArgs e)
        {
            ShowDialogEvent?.Invoke(sender, e);
        }
    }

    public class ToastEventArgs : EventArgs
    {
        public string Message { get; set; }
        public ToastType Type { get; set; }
        ToastEventArgs() { 
        }

        public ToastEventArgs(string message, ToastType type)
        {
            Message = message;
            Type = type;
        }
    } 
    public class DialogEventArgs : EventArgs
    {
        public DialogEventArgs(BaseUserControl dialogInstance)
        {
            this.dialog = dialogInstance;
        }

        public BaseUserControl dialog { get; set; }
       
    }
}
