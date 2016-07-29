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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum ManagerState { Menu, Playing }

        public ManagerState state;

        List<AIDefinition> allAIs;

        public MainWindow()
        {
            InitializeComponent();

            state = ManagerState.Menu;

            allAIs = getPossibleAIs();

            player1AIs.ItemsSource = allAIs;
            player2AIs.ItemsSource = allAIs;

            StartGameButton.Click += StartGameButton_Click;
        }

        void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            state = ManagerState.Playing;
            ManagerInterface.SelectedIndex = 1;

            QuatroPlayer player1 = allAIs[player1AIs.SelectedIndex].creator();
            QuatroPlayer player2 = allAIs[player2AIs.SelectedIndex].creator();
            GameControl = new QuatroInterface(player1, player2);
        }

        List<AIDefinition> getPossibleAIs()
        {
            List<AIDefinition> allAIs = new List<AIDefinition>();
            Func<QuatroPlayer> creator;

            //adde new AIs here 
            //creator = delegate{ return new AIS.XXX.XXX();};
            //allAIs.Add(new AIDefinition( creator , "XXX"));

            creator = delegate{ return new AIS.Human.Human();};
            allAIs.Add(new AIDefinition( creator , "Human"));

            return allAIs;
        }        
    }

}
