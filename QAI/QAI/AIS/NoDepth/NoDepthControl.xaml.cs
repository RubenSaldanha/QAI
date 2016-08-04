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

namespace QAI.AIS.NoDepth
{
    /// <summary>
    /// Interaction logic for NoDepthControl.xaml
    /// </summary>
    public partial class NoDepthControl : UserControl
    {
        NoDepth noDepthAI;

        const int MaxLength = 2000;
        
        public NoDepthControl(NoDepth noDepthAI)
        {
            InitializeComponent();

            this.noDepthAI = noDepthAI;

            noDepthAI.Changed += UpdateVisuals;
        }
        
        public void UpdateVisuals()
        {
            if(speach.Content != null)
                if (speach.Content.ToString().Length > MaxLength)
                    speach.Content = speach.Content.ToString().Substring(0, MaxLength);

            speach.Content = "\n" + noDepthAI.talk + "\n" + speach.Content;
        }
    }
}
