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

namespace QAI.AIS.TalkingMonkey
{
    /// <summary>
    /// Interaction logic for TalkingMonkeyControl.xaml
    /// </summary>
    public partial class TalkingMonkeyControl : UserControl
    {
        TalkingMonkey monkey;

        public TalkingMonkeyControl(TalkingMonkey monkey)
        {
            InitializeComponent();

            //store AI reference
            this.monkey = monkey;

            //Register update event
            monkey.Changed += UpdateVisuals;
        }

        public void UpdateVisuals()
        {
            //Update visual speach
            speach.Content = monkey.talk;
        }
    }
}
