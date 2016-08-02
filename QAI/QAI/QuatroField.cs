using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAI
{
    public class QuatroField
    {
        int[,] field;
        int[] heads;

        List<int> plays;

        public int PlayerTurn
        {
            get { return plays.Count % 2 + 1;  }
        }

        public bool PlayerWon;

        public enum GameState { Playing, Player1Victory, Player2Victory, Tie }
        public GameState State { get; private set; }

        public QuatroField()
        {
            field = new int[7, 9];
            heads = new int[9];
            plays = new List<int>();
            State = GameState.Playing;
        }

        private QuatroField(QuatroField original)
        {
            this.field = (int[,])original.field.Clone();
            this.heads = (int[])original.heads.Clone();
            this.plays = new List<int>(original.plays);
            this.State = original.State;
            this.PlayerWon = original.PlayerWon;
        }

        public bool canPlay(int column)
        {
            return heads[column] < 7;
        }

        public int head(int column)
        {
            return heads[column];
        }

        public void play(int column)
        {
            int currentPlayer = PlayerTurn;
            field[heads[column], column] = currentPlayer;
            heads[column]++;
            plays.Insert(0, column);

            if(checkWin())
            {
                PlayerWon = true;

                if (currentPlayer == 1)
                    State = GameState.Player1Victory;
                else
                    State = GameState.Player2Victory;
            }
            else if (plays.Count == 7 * 9)
                State = GameState.Tie;
        }

        private bool checkWin()
        {
            int column = plays[0];
            int line = heads[column] - 1;
            int currentPlayer = (plays.Count - 1) % 2 + 1;


            int row = 0;            // numero de peças consecutivas.


            // ---------------------------- Iniciar o calculo das possiveis vitórias ------------------------------------
            // Iniciar o calculo da possivél vitória na horizontal-------------------------------------------------------
            row = 1;
            for (int i = 1; column + i < 9; i++)
            {
                if (field[line, column + i] == currentPlayer) { row++; } else { break; }     //  verificar o numero de peças do jogador para a direita na linha.
            }
            for (int i = 1; column - i > -1; i++)
            {
                if (field[line, column - i] == currentPlayer) { row++; } else { break; }     //  verificar o numero de peças do jogador para a esquerda na linha
            }
            if (row > 3) { return true; }  // devolver vitória caso o jogador tenha ganho horizontal.

            // Iniciar o calculo da possivél vitória na vertical--------------------------------------------------------
            row = 1;
            for (int i = 1; line - i > -1; i++)
            {
                if (field[line - i, column] == currentPlayer) { row++; } else { break; }
            }
            if (row == 4) { return true; }  // devolver vitória caso o jogador tenha ganho na vertical.


            // Iniciar o calculo das vitórias na diagonal--------------------------------------------------------------------
            // caso: (declive -1)--------------------------------------------------------------------------------------------
            row = 1;
            for (int i = 1; (column + i < 9) && (line - i > -1); i++)
            {
                if (field[line - i, column + i] == currentPlayer) { row++; } else { break; }     //  verificar o numero de peças do jogador para a direita na diagonal(-1).
            }
            for (int i = 1; (column - i > -1) && (line + i < 7); i++)
            {
                if (field[line + i, column - i] == currentPlayer) { row++; } else { break; }     //  verificar o numero de peças do jogador para a esquerda na diagonal(-1).
            }
            if (row > 3) { return true; }  // devolver vitória casao o jogador tenha ganho na diagonal (declive -1).


            // caso: (declive 1)-----------------------------------------------------------------------------------------------
            row = 1;
            for (int i = 1; (column + i < 9) && (line + i < 7); i++)
            {
                if (field[line + i, column + i] == currentPlayer) { row++; } else { break; }     //  verificar o numero de peças do jogador para a direita na diagonal(1).
            }
            for (int i = 1; (column - i > -1) && (line - i > -1); i++)
            {
                if (field[line - i, column - i] == currentPlayer) { row++; } else { break; }     //  verificar o numero de peças do jogador para a esquerda na diagonal(1).
            }
            if (row > 3) { return true; }  // devolver vitória caso o jogador tenha ganho na diagonal (declive 1).


            return false;
        }

        public void undo()
        {
            int lastPlay = plays[0];
            plays.RemoveAt(0);
            heads[lastPlay]--;
            field[heads[lastPlay], lastPlay] = 0;
            PlayerWon = false;

            State = GameState.Playing;
        }

        public int getLastPlay()
        {
            if (plays.Count > 0)
                return plays[0];
            else
                return -1;
        }

        public void ResignPlayer(int player)
        {
            if (State == GameState.Playing)
            {
                if (player == 1)
                {
                    State = GameState.Player2Victory;
                    PlayerWon = true;
                }
                else if (player == 2)
                {
                    State = GameState.Player1Victory;
                    PlayerWon = true;
                }
                else
                    throw new Exception("Invalid player exception, \"" + player + "\" is not a valid player");
            }
            else
                throw new Exception("Can't resign when the game is finished.");
        }

        public int this[int line, int column]
        {
            get { return field[line, column]; }
        }

        public int this[int column]
        {
            get { return field[heads[column], column]; }
        }

        public QuatroField getCopy()
        {
            return new QuatroField(this);
        }
    }
}
