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
        QuatroGame game;

        QuatroPlayer player1;
        QuatroPlayer player2;

        int player1Score;
        int player2Score;

        Button[,] fieldUI;

        public event Action Finished;

        BitmapImage slotImage;
        BitmapImage p1Image;
        BitmapImage p2Image;

        List<int> ReviewPlays;

        QuatroOptions options;

        public enum QuatroInterfaceState { Idle, Playing, GameOver, Review}

        public QuatroInterface(QuatroPlayer player1, QuatroPlayer player2, QuatroOptions options)
        {
            InitializeComponent();

            this.player1 = player1;
            this.player2 = player2;

            player1.mainThread = this.Dispatcher;
            player2.mainThread = this.Dispatcher;

            player1Control.Content = player1.GetFeedbackControl();
            player2Control.Content = player2.GetFeedbackControl();

            this.options = options;

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

                Label columnNumber = new Label();
                columnNumber.Content = "" + i;
                columnNumber.FontSize = 16;
                columnNumber.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                column.Children.Add(columnNumber);
            }

            slotImage = new BitmapImage(new Uri("s0.png", UriKind.Relative));
            p1Image = new BitmapImage(new Uri("s1.png", UriKind.Relative));
            p2Image = new BitmapImage(new Uri("s2.png", UriKind.Relative));

            player1ScoreLabel.Content = "0";
            player2ScoreLabel.Content = "0";
            

            StartNewGame();
        }

        void StartNewGame()
        {
            interactionGrid.Children.Clear();
            ReviewPlays = new List<int>();
            game = null;
            UpdateFieldUI();


            game = new QuatroGame(player1, player2, options);

            game.TimerChanged += UpdateTimer;
            game.StateChanged += GameStateChanged;
            game.FieldChanged += UpdateFieldUI;

            if(!options.automaticPlay)
            {
                ShowIdleInteraction();
            }
        }

        void GameStateChanged()
        {
            switch(game.state)
            {
                case QuatroGame.QuatroGameState.Idle:
                    ShowIdleInteraction();
                    break;
                case QuatroGame.QuatroGameState.Playing:
                    UpdateHeadUI();
                    break;
                case QuatroGame.QuatroGameState.Finished:
                    UpdateHeadUI();
                    ShowEndGameInteraction();
                    break;
            }
        }

        void Quit()
        {
            player1.Destroy();
            player2.Destroy();

            if (Finished != null)
                Finished();
        }

        private void UpdateTimer()
        {
            int currentTurnTime = game.currentTurnTime;

            String centiSeconds = ("" + currentTurnTime % 100).PadLeft(2,'0');
            String seconds = ("" + (currentTurnTime / 100) % 60).PadLeft(2,'0');;
            String minutes = ("" + ((currentTurnTime / 100) / 60) % 60).PadLeft(2,'0');;
            Timer.Content = "" + minutes + ":" + seconds + ":" + centiSeconds;
        }

        public void UpdateFieldUI()
        {
            if (game == null)
            {
                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        Image p0i = new Image();
                        p0i.Source = slotImage;
                        fieldUI[i, j].Content = p0i;
                    }
                }
            }
            else
            {


                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        switch (game.field[i, j])
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
        }

        void UpdateHeadUI()
        {
            switch (game.field.State)
            {
                case QuatroField.GameState.Playing:
                    mainLabel.Content = "Player " + game.field.PlayerTurn + " Turn";
                    break;
                case QuatroField.GameState.Player1Victory:
                    mainLabel.Content = "Player 1 Wins!!!";
                    player1Score++;
                    player1ScoreLabel.Content = player1Score;
                    break;
                case QuatroField.GameState.Player2Victory:
                    mainLabel.Content = "Player 2 Wins!!!";
                    player2Score++;
                    player2ScoreLabel.Content = player2Score;
                    break;
                case QuatroField.GameState.Tie:
                    mainLabel.Content = "--- Tie ---";
                    break;
            }
        }

        void ShowIdleInteraction()
        {
            interactionGrid.Children.Clear();
            Button next = new Button();
            next.Content = "Next";
            next.Width = 180;
            next.Height = 40;
            next.Click += next_Click;
            interactionGrid.Children.Add(next);
        }

        void ShowEndGameInteraction()
        {
            interactionGrid.Children.Clear();
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

            Button review = new Button();
            review.Content = "Review Game";
            review.Width = 140;
            review.Height = 40;
            review.Click += review_Click;
            sPanel.Children.Add(review);

            Button quit = new Button();
            quit.Content = "Quit";
            quit.Width = 120;
            quit.Height = 40;
            quit.Click += quit_Click;
            sPanel.Children.Add(quit);
        }

        void ShowNavigateInteraction()
        {
            interactionGrid.Children.Clear();
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

            Button back = new Button();
            back.Content = "<";
            back.Width = 70;
            back.Height = 40;
            back.Click += back_Click;
            sPanel.Children.Add(back);

            Button forward = new Button();
            forward.Content = ">";
            forward.Width = 70;
            forward.Height = 40;
            forward.Click += forward_Click;
            sPanel.Children.Add(forward);

            Button quit = new Button();
            quit.Content = "Quit";
            quit.Width = 120;
            quit.Height = 40;
            quit.Click += quit_Click;
            sPanel.Children.Add(quit);
        }

        void slot_Click(object sender, RoutedEventArgs e)
        {
            if (game != null)
            {
                Button slot = sender as Button;
                GameCoords coords = slot.DataContext as GameCoords;

                game.PutClick(coords);
            }
        }

        void quit_Click(object sender, RoutedEventArgs e)
        {
            Quit();
        }

        void playAgain_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }

        void review_Click(object sender, RoutedEventArgs e)
        {
            ShowNavigateInteraction();
        }

        void back_Click(object sender, RoutedEventArgs e)
        {
            if (game.field.getLastPlay() != -1)
            {
                ReviewPlays.Insert(0, game.field.getLastPlay());
                game.field.undo();
                UpdateFieldUI();
            }
        }

        void forward_Click(object sender, RoutedEventArgs e)
        {
            if (ReviewPlays.Count > 0)
            {
                game.field.play(ReviewPlays[0]);
                ReviewPlays.RemoveAt(0);
                UpdateFieldUI();
            }
        }

        void next_Click(object sender, RoutedEventArgs e)
        {
            interactionGrid.Children.Clear();
            game.NextGameStep();
        }

    }
}
