using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

        private void onRunButtonClicked(object sender, RoutedEventArgs e)
        {
            AsyncVoidMethod();
        }

        private async void AsyncVoidMethod()
        {
            await Task.Run(() =>
            {
                WriteLine($"1");
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
