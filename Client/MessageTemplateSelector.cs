using System.Windows;
using System.Windows.Controls;

namespace CSharkClient
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
            Message message = item as Message;
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
