using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsyncAwaitSimpleExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Start Thread
            new Thread(() => { 
                string result = LongRunningMethod("World");
                Dispatcher.BeginInvoke((Action)(() => Label1.Content = result)); 
            }).Start();
            Label1.Content = "Working...";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // TPL(Task Parallel Library)
            Task.Run<string>(() => LongRunningMethod("World"))
                .ContinueWith(ant => Label2.Content = ant.Result, 
                              TaskScheduler.FromCurrentSynchronizationContext());
            Label2.Content = "Working...";
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // Async
            CallLongRunningMethod();
            Label3.Content = "Working...";        
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            // Async Anon
            new Action(async () =>
            {
                string result = await Task.Run<string>(() => LongRunningMethod("World"));
                Label4.Content = result;
            }).Invoke();
            Label4.Content = "Working...";
        }

        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            // Async Return
            Label5.Content = "Working...";
            string result = await CallLongRunningMethodReturn();
            Label5.Content = result;
        }

        private async void CallLongRunningMethod()
        {
            string result = await LongRunningMethodAsync("World");
            Label3.Content = result;
        }

        private Task<string> LongRunningMethodAsync(string message)
        {
            return Task.Run<string>(() => LongRunningMethod(message));
        }

        private async Task<string> CallLongRunningMethodReturn()
        {
            string result = await LongRunningMethodAsync("World");
            return result;
        }

        private string LongRunningMethod(string message)
        {
            Thread.Sleep(2000);
            return "Hello " + message;
        }

    }
}
