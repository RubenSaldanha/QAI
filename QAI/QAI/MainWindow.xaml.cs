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

        QuatroInterface quatroInterface;

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
            StartGame();
        }

        public void StartGame()
        {
            state = ManagerState.Playing;
            ManagerInterface.SelectedIndex = 1;

            //fecth selection
            int AI1 = player1AIs.SelectedIndex;
            int AI2 = player2AIs.SelectedIndex;

            //In case of unselected AI's, go with first one
            if (AI1 == -1)
                AI1 = 0;
            if (AI2 == -1)
                AI2 = 0;

            QuatroPlayer player1 = allAIs[AI1].creator();
            QuatroPlayer player2 = allAIs[AI2].creator();
            quatroInterface = new QuatroInterface(player1, player2);
            quatroInterface.Finished += GoToAIPanel;

            GameControl.Content = quatroInterface;
        }
        public void GoToAIPanel()
        {
            state = ManagerState.Menu;
            ManagerInterface.SelectedIndex = 0;

            GameControl.Content = null;
            quatroInterface = null;
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

            creator = delegate { return new AIS.Monkey.Monkey(); };
            allAIs.Add(new AIDefinition(creator, "Monkey"));

            creator = delegate { return new AIS.BruteSearch.BruteSearch(); };
            allAIs.Add(new AIDefinition(creator, "BruteSearch"));

            creator = delegate { return new AIS.NoDepth.NoDepth(); };
            allAIs.Add(new AIDefinition(creator, "NoDepth"));


            return allAIs;
        }        
    }

}
