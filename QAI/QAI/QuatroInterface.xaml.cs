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

namespace QAI
{
    /// <summary>
    /// Interaction logic for QuatroInterface.xaml
    /// </summary>
    public partial class QuatroInterface : UserControl
    {
        QuatroGame game;

        QuatroPlayer player1;
        QuatroPlayer player2;

        public QuatroInterface(QuatroPlayer player1, QuatroPlayer player2)
        {
            InitializeComponent();

            this.player1 = player1;
            this.player2 = player2;
        }
    }
}
