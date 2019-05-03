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
        public double m_Interval { get; set; }
        private Timer m_Timer;
        public TimedLabel()
        {
            InitializeComponent();
            m_Interval = 1200;
            m_Timer = new Timer(m_Interval); ;
        }

        public void startTimedLabel()
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Visibility = Visibility.Visible;

                m_Timer.Elapsed += dispatcherTimer_Tick;
                m_Timer.Start();

            });

        }
        private void dispatcherTimer_Tick(Object obj, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Visibility = Visibility.Hidden;
                m_Timer.Stop();

            });
        }
    }
}
