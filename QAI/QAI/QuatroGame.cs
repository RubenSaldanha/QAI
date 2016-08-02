using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace QAI
{
    class QuatroGame
    {
        public QuatroField field;

        QuatroPlayer player1;
        QuatroPlayer player2;

        System.Windows.Threading.DispatcherTimer dispatcherTimer;
        int turnTime;
        public int currentTurnTime;

        BackgroundWorker AIworker;

        InterfaceNotifier notifier;

        QuatroOptions options;

        public enum QuatroGameState { Idle, Playing, Finished}
        public QuatroGameState state;

        public event Action StateChanged;
        public event Action FieldChanged;
        public event Action TimerChanged;

        public QuatroGame(QuatroPlayer player1, QuatroPlayer player2, QuatroOptions options)
        {
            this.options = options;
            turnTime = (int)(options.turnSeconds * 100);

            state = QuatroGameState.Idle;

            this.player1 = player1;
            this.player2 = player2;

            //time mechanics
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += UpdateTimer;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            field = new QuatroField();

            player1.StartGame();
            player2.StartGame();

            if (options.automaticPlay)
                NextGameStep();
        }

        public void NextGameStep()
        {
            switch(state)
            {
                case QuatroGameState.Idle:
                    //KickStartNextPlayer
                    KickStartNextPlayer();
                    state = QuatroGameState.Playing;

                    //Fire State change event
                    if (StateChanged != null)
                        StateChanged();

                    break;
                case QuatroGameState.Playing:

                    //Check the result of the playing and act accordingly
                    if (field.State == QuatroField.GameState.Playing)
                        if (options.automaticPlay)
                            KickStartNextPlayer();
                        else
                            state = QuatroGameState.Idle;
                    else
                        state = QuatroGameState.Finished;

                    //Fire State change event
                    if (StateChanged != null)
                        StateChanged();

                    break;
                case QuatroGameState.Finished:
                    break;
            }
        }

        void KickStartNextPlayer()
        {
            currentTurnTime = turnTime;
            AIworker = new BackgroundWorker();
            AIworker.WorkerSupportsCancellation = true;
            AIworker.DoWork += PlayerPlay;
            AIworker.RunWorkerCompleted += PlayerFinish;
            AIworker.RunWorkerAsync();

            dispatcherTimer.Start();
        }

        void PlayerPlay(object sender, DoWorkEventArgs e)
        {
            int player = field.PlayerTurn;
            int play;

            QuatroField fieldCopy = field.getCopy();

            notifier = new InterfaceNotifier();

            if (player == 1)
                play = player1.playI(fieldCopy, notifier);
            else
                play = player2.playI(fieldCopy, notifier);

            if (field.canPlay(play))
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

            if (FieldChanged != null)
                FieldChanged();

            NextGameStep();
        }

        private void UpdateTimer(object sender, EventArgs e)
        {
            currentTurnTime--;
            if (currentTurnTime < 0)
                currentTurnTime = 0;

            if (TimerChanged != null)
                TimerChanged();

            if(currentTurnTime == 0)
            {
                //TODO Proper Locks, play can happen at same time than cancelation
                AIworker.CancelAsync();
                field.ResignPlayer(field.PlayerTurn);
                dispatcherTimer.Stop();

                NextGameStep();
            }
        }

        public void PutClick(GameCoords coords)
        {
            if(notifier != null)
                notifier.setClick(coords.line, coords.column);
        }
    }
}
