using System;
using System.Collections.Generic;
using System.Linq;
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
using System.ComponentModel;

namespace QAI
{
    /// <summary>
    /// Interaction logic for QuatroInterface.xaml
    /// </summary>
    public partial class QuatroInterface : UserControl
    {
        QuatroField field;

        QuatroPlayer player1;
        QuatroPlayer player2;

        System.Windows.Threading.DispatcherTimer dispatcherTimer;
        int turnTime;

        BackgroundWorker AIworker;

        object notifierLock = new object();
        InterfaceNotifier notifier;

        Button[,] fieldUI;

        public event Action Finished;

        BitmapImage slotImage;
        BitmapImage p1Image;
        BitmapImage p2Image;

        public QuatroInterface(QuatroPlayer player1, QuatroPlayer player2)
        {
            InitializeComponent();

            this.player1 = player1;
            this.player2 = player2;

            fieldUI = new Button[7, 9];

            //visual initialization
            StackPanel columnStacks = new StackPanel();
            columnStacks.Orientation = Orientation.Horizontal;
            playGrid.Children.Add(columnStacks);

            for(int i=0;i<9;i++)
            {
                StackPanel column = new StackPanel();
                column.Orientation = Orientation.Vertical;

                columnStacks.Children.Add(column);

                for(int j=6;j>=0;j--)
                {
                    Button slot = new Button();
                    slot.Width = 50;
                    slot.Height = 50;
                    fieldUI[j, i] = slot;
                    GameCoords coords = new GameCoords(j, i);

                    slot.DataContext = coords;
                    slot.Click += slot_Click;
                    slot.Content = "(" + j + "," + i + ")";

                    column.Children.Add(slot);
                }
            }

            slotImage = new BitmapImage(new Uri("s0.png", UriKind.Relative));
            p1Image = new BitmapImage(new Uri("s1.png", UriKind.Relative));
            p2Image = new BitmapImage(new Uri("s2.png", UriKind.Relative));


            //time mechanics
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += UpdateTimer;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            
            StartNewGame();
        }

        void StartNewGame()
        {
            field = new QuatroField();
            interactionGrid.Children.Clear();
            UpdateFieldUI();

            NextGameStep();
        }

        void NextGameStep()
        {
            switch (field.State)
            {
                case QuatroField.GameState.Playing:
                    UpdateFieldUI();
                    mainLabel.Content = "Player " + field.PlayerTurn + " Turn";
                    KickStartNextPlayer();
                    break;
                case QuatroField.GameState.Player1Victory:
                    UpdateFieldUI();
                    mainLabel.Content = "Player 1 Wins!!!";
                    ShowEndGameInteraction();
                    break;
                case QuatroField.GameState.Player2Victory:
                    UpdateFieldUI();
                    mainLabel.Content = "Player 2 Wins!!!";
                    ShowEndGameInteraction();
                    break;
                case QuatroField.GameState.Tie:
                    UpdateFieldUI();
                    mainLabel.Content = "--- Tie ---";
                    ShowEndGameInteraction();
                    break;
            }
        }

        void KickStartNextPlayer()
        {
            turnTime = 500;
            AIworker = new BackgroundWorker();
            AIworker.WorkerSupportsCancellation = true;
            AIworker.DoWork += PlayerPlay;
            AIworker.RunWorkerCompleted += PlayerFinish;
            AIworker.RunWorkerAsync();

            dispatcherTimer.Start();
        }

        private void UpdateTimer(object sender, EventArgs e)
        {
            turnTime--;
            if (turnTime < 0)
                turnTime = 0;

            String centiSeconds = ("" + turnTime % 100).PadLeft(2,'0');
            String seconds = ("" + (turnTime / 100) % 60).PadLeft(2,'0');;
            String minutes = ("" + ((turnTime / 100) / 60) % 60).PadLeft(2,'0');;
            Timer.Content = "" + minutes + ":" + seconds + ":" + centiSeconds;

            if(turnTime == 0)
            {
                //TODO Proper Locks, play can happen at same time than cancelation
                AIworker.CancelAsync();
                field.ResignPlayer(field.PlayerTurn);
                dispatcherTimer.Stop();
                NextGameStep();
            }
        }

        void PlayerPlay(object sender, DoWorkEventArgs e)
        {
            int player = field.PlayerTurn;
            int play;

            QuatroField fieldCopy = field.getCopy();

            lock (notifierLock)
            {
                notifier = new InterfaceNotifier();
            }

            if (player == 1)
                play = player1.playI(fieldCopy, notifier);
            else
                play = player2.playI(fieldCopy, notifier);

            if(field.canPlay(play))
                field.play(play);
            else
            {
                //invalid play by AI
                field.ResignPlayer(player);
            }
        }

        void PlayerFinish(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer.Stop();
            NextGameStep();
        }

        void slot_Click(object sender, RoutedEventArgs e)
        {
            Button slot = sender as Button;
            GameCoords coords = slot.DataContext as GameCoords;

            notifier.setClick(coords.line, coords.column);
        }

        void UpdateFieldUI()
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    switch(field[i,j])
                    {
                        case 0:
                            Image p0i = new Image();
                            p0i.Source = slotImage;
                            fieldUI[i, j].Content = p0i;
                            break;
                        case 1:
                            Image p1i = new Image();
                            p1i.Source = p1Image;
                            fieldUI[i, j].Content = p1i;
                            break;
                        case 2:
                            Image p2i = new Image();
                            p2i.Source = p2Image;
                            fieldUI[i, j].Content = p2i;
                            break;
                    }
                }
            }
        }

        void ShowEndGameInteraction()
        {
            StackPanel sPanel = new StackPanel();
            sPanel.Orientation = Orientation.Horizontal;
            sPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            sPanel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            interactionGrid.Children.Add(sPanel);

            Button playAgain = new Button();
            playAgain.Content = "Play Again";
            playAgain.Width = 120;
            playAgain.Height = 40;
            playAgain.Click += playAgain_Click;
            sPanel.Children.Add(playAgain);

            Button quit = new Button();
            quit.Content = "Quit";
            quit.Width = 120;
            quit.Height = 40;
            quit.Click += quit_Click;
            sPanel.Children.Add(quit);
        }

        void quit_Click(object sender, RoutedEventArgs e)
        {
            if (Finished != null)
                Finished();
        }

        void playAgain_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }
    }
}
