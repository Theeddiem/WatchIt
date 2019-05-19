using System;
using System.Collections.Generic;
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

namespace ViewUI
{
    /// <summary>
    /// Interaction logic for BlinkingButton.xaml
    /// </summary>
    public partial class BlinkingButton : UserControl
    {
        public BlinkingButton()
        {
            InitializeComponent();
        }

        public void activeOpacity()
        {
            if(IsMouseOver)
            {
                this.Opacity = 1;
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.Opacity = 0.5;
        }


    }
}
