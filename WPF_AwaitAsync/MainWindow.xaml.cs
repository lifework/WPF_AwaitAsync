using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WPF_AwaitAsync
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

        private async void onRunButtonClicked(object sender, RoutedEventArgs e)
        {
            WriteLine($" BEGIN onRunButtonClicked");
            await AsyncTaskMethod();
            await AsyncTaskMethod();
            AsyncVoidMethod();
            WriteLine($" END onRunButtonClicked");
        }

        private async void AsyncVoidMethod()
        {
            await Task.Run(() =>
            {
                WriteLine($"BEGIN Task.Run() in AsyncVoidMethod");
                Thread.Sleep(5 * 1000);
                WriteLine($"END Task.Run() in AsyncVoidMethod");
            });
        }

        private async Task AsyncTaskMethod()
        {
            await Task.Run(() =>
            {
                WriteLine($"BEGIN Task.Run() in AsyncTaskMethod");
                Thread.Sleep(3 * 1000);
                WriteLine($"END Task.Run() in AsyncTaskMethod");
            });
        }


        private void WriteLine(string text)
        {
            Debug.WriteLine($"[{Caller()}][{DateTime.Now}] {text}");
        }

        private string Caller()
        {
            var caller = new System.Diagnostics.StackFrame(2, false);
            var klass = caller.GetMethod()?.DeclaringType?.FullName;
            var method = caller.GetMethod()?.Name;

            return $"{klass}.{method}";
        }
    }
}
