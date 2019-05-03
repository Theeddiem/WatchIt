using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ViewUI
{
    /// <summary>
    /// Interaction logic for TimedLabel.xaml
    /// </summary>
    public partial class TimedLabel : UserControl
    {
        public double I_interval { get; set; }
        public TimedLabel()
        {
            InitializeComponent();
            I_interval = 1200;
        }

        public void startTimedLabel()
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Visibility = Visibility.Visible;
                Timer timer = new Timer(I_interval);
                timer.Elapsed += dispatcherTimer_Tick;
                timer.Start();

            });

        }
        private void dispatcherTimer_Tick(Object obj, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Visibility = Visibility.Hidden;
            });
        }
    }
}
