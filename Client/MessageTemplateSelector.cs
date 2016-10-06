using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Client
{
    class MessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MyMessageTemplate { get; set; }
        public DataTemplate OthersMessageTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return null;

            FrameworkElement frameworkElement = container as FrameworkElement;
            Client.MainWindow.Message message = item as Client.MainWindow.Message;
            if (frameworkElement != null)
            {
                bool myMessage = message.Username.Equals("Me");
                if (myMessage)
                {
                    MyMessageTemplate = frameworkElement.FindResource("MyMessagesTemplate") as DataTemplate;
                    return MyMessageTemplate;
                }
                else
                {
                    OthersMessageTemplate = frameworkElement.FindResource("OthersMessagesTemplate") as DataTemplate;
                    return OthersMessageTemplate;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
