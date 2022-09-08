using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
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
    /// Interaction logic for MainWindow.xamlhttps://partner.microsoft.com/ja-jp/dashboard/insights/analytics/overview
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void onRunButton1Clicked(object sender, RoutedEventArgs e)
        {
            await TaskAsync("A");
            await TaskAsync("B");
            VoidAsync("C");
            VoidAsync("D");
        }


        private async void onRunButton2Clicked(object sender, RoutedEventArgs e)
        {
            // A と B は排他的になる。どっちが先かはわからない。
            _ = Task.Run(() => LockedSync("A"));
            _ = Task.Run(() => LockedSync("B"));

            Thread.Sleep(1000);

            // Task.Run のタイミングは排他的だけど、
            // その先の CounterAsync は非同期なので、出力は混ざる。
            _ = Task.Run(() => LockedAsync("Async A"));
            _ = Task.Run(() => LockedAsync("Async B"));
        }

        private static readonly Object LockObject = new Object();
        private void LockedSync(string label)
        {
            lock (LockObject)
            {
                Counter(label);
            }
        }

        private void LockedAsync(string label)
        {
            lock (LockObject)
            {
                Task.Run(() =>
                {
                    Counter(label);
                });
            }
        }



        private async void VoidAsync(string label)
        {
            await Task.Run(() =>
            {
                Counter(label);
            });
        }

        private async Task TaskAsync(string label)
        {
            await Task.Run(() =>
            {
                Counter(label);
            });
        }


        private const int CounterMaxValue = 20;
        private void Counter(string label, int max = CounterMaxValue)
        {
            for (var n = 0; n < max; n++)
            {
                WriteLine($"{label} = {n}");
            }
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
