using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QAI.AIS.NoDepth
{
    class NoDepth : QuatroPlayer
    {
        Random rdm = new Random();

        public override int playI(QuatroField field, InterfaceNotifier notifier)
        {
            int play;
            int myNumber = field.PlayerTurn;
            int opNumber = myNumber == 1 ? 2 : 1;


            play = winNext(myNumber, field);
            if (play != -1)
            {
                MessageBox.Show("Eu vou ganhar.", "NoDepth AI Tought");
                return play;
            }
                

            play = winNext(opNumber, field);
            if (play != -1)
            {
                MessageBox.Show("Tu ias ganhar.", "NoDepth AI Tought");
                return play;
            }

            List<int> moves = bestPlay(possibleMoves(field), myNumber, opNumber, field);

            return moves[rdm.Next(moves.Count)];
        }

        int winNext(int player, QuatroField field)
        {
            int count = 1;
            int play = -1;

            for (int y = 0; y < 7; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    // para todas as jogadas verificar todas as possibilidades
                    if (field[y, x] == player)
                    {
                        // horizontal ->
                        if (x < 6)
                        {
                            for (int i = 1; i < 4; i++)
                                if (field[y, x + i] == player)
                                    count++;
                                else
                                    play = x + i;   // checkar gaps e.g: 1011

                            if (count == 3 && field.head(play) == y)
                                return play;
                            else
                                count = 1;
                        }

                        // horizontal <-  
                        // NOTA: Gaps horizontais ja foram chekados em cima.
                        if (x > 2)
                        {
                            for (int i = 1; i < 3; i++)
                                if (field[y, x - i] == player)
                                    count++;

                            if (count == 3 && field.head(x - 3) == y)
                                return x - 3;
                            else
                                count = 1;
                        }

                        // vertical
                        if (y < 4)
                        {
                            for (int i = 1; i < 3; i++)
                                if (field[y + i, x] == player)
                                    count++;

                            if (count == 3 && field.head(x) == y + 3)
                                return x;
                            else
                                count = 1;
                        }

                        // diagonal declive 1 ->
                        if (x < 6 && y < 4)
                        {
                            int line = -10;
                            for (int i = 1; i < 4; i++)
                                if (field[y + i, x + i] == player)
                                    count++;
                                else
                                {
                                    play = x + i;   // checkar gaps
                                    line = y + i;
                                }
                                    

                            if (count == 3 && field.head(play) == line)
                                return play;
                            else
                                count = 1;
                        }

                        // diagonal declive 1 <-
                        // NOTA: Gaps diagonais com declive 1 ja foram chekados em cima.
                        if (x > 2 && y > 2)
                        {
                            for (int i = 1; i < 3; i++)
                                if (field[y - i, x - i] == player)
                                    count++;

                            if (count == 3 && field.head(x - 3) == y - 3)
                                return x - 3;
                            else
                                count = 1;
                        }


                        // diagonal declive -1 ->
                        if (x < 6 && y > 2)
                        {
                            int line = -10;
                            for (int i = 1; i < 4; i++)
                                if (field[y - i, x + i] == player)
                                    count++;
                                else
                                {
                                    play = x + i;   // checkar gaps
                                    line = y - i;
                                }


                            if (count == 3 && field.head(play) == line)
                                return play;
                            else
                                count = 1;
                        }

                        // diagonal declive -1 <-
                        // NOTA: Gaps diagonais com declive -1 ja foram chekados em cima.
                        if (x > 2 && y < 4)
                        {
                            for (int i = 1; i < 3; i++)
                                if (field[y + i, x - i] == player)
                                    count++;

                            if (count == 3 && field.head(x - 3) == y + 3)
                                return x - 3;
                            else
                                count = 1;
                        }


                    }
                }
            }

            return -1;
        }

        bool isSafe(int play, int player, QuatroField field)
        {
            int count = 1;
            int column = -1;
            int nextline = field.head(play) + 1;

            for (int y = 0; y < 7; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    // para todas as jogadas verificar todas as possibilidades
                    if (field[y, x] == player)
                    {
                        // horizontal ->
                        if (x < 6)
                        {
                            for (int i = 1; i < 4; i++)
                                if (field[y, x + i] == player)
                                    count++;
                                else
                                    column = x + i;   // checkar gaps e.g: 1011

                            if (count == 3 && y == nextline && column == play)
                                return false;
                            else
                                count = 1;
                        }

                        // horizontal <-  
                        // NOTA: Gaps horizontais ja foram chekados em cima.
                        if (x > 2)
                        {
                            column = x - 3;
                            for (int i = 1; i < 3; i++)
                                if (field[y, x - i] == player)
                                    count++;

                            if (count == 3 && y == nextline && column == play)
                                return false;
                            else
                                count = 1;
                        }

                        // diagonal declive 1 ->
                        if (x < 6 && y < 4)
                        {
                            int line = -10;
                            for (int i = 1; i < 4; i++)
                                if (field[y + i, x + i] == player)
                                    count++;
                                else
                                {
                                    column = x + i;   // checkar gaps
                                    line = y + i;
                                }


                            if (count == 3 && line == nextline && column == play)
                                return false;
                            else
                                count = 1;
                        }

                        // diagonal declive 1 <-
                        // NOTA: Gaps diagonais com declive 1 ja foram chekados em cima.
                        if (x > 2 && y > 2)
                        {
                            column = x - 3;
                            for (int i = 1; i < 3; i++)
                                if (field[y - i, x - i] == player)
                                    count++;

                            if (count == 3 && y-3 == nextline && column == play)
                                return false;
                            else
                                count = 1;
                        }


                        // diagonal declive -1 ->
                        if (x < 6 && y > 2)
                        {
                            int line = -10;
                            for (int i = 1; i < 4; i++)
                                if (field[y - i, x + i] == player)
                                    count++;
                                else
                                {
                                    column = x + i;   // checkar gaps
                                    line = y - i;
                                }


                            if (count == 3 && line == nextline && column == play)
                                return false;
                            else
                                count = 1;
                        }

                        // diagonal declive -1 <-
                        // NOTA: Gaps diagonais com declive -1 ja foram chekados em cima.
                        if (x > 2 && y < 4)
                        {
                            column = x - 3;
                            for (int i = 1; i < 3; i++)
                                if (field[y + i, x - i] == player)
                                    count++;

                            if (count == 3 && y+3 == nextline && column == play)
                                return false;
                            else
                                count = 1;
                        }


                    }
                }
            }

            return true;
        }

        public List<int> bestPlay(List<int> possibleMoves, int myNumber, int opNumber, QuatroField field)
        {
            List<int> seguros = new List<int>();
            List<int> naoSeguros1 = new List<int>();
            List<int> naoSeguros2 = new List<int>();

            for (int i = 0; i < possibleMoves.Count; i++)
            {
                if (isSafe(possibleMoves[i], myNumber, field) && isSafe(possibleMoves[i], opNumber, field))
                    seguros.Add(possibleMoves[i]);
                else if (!isSafe(possibleMoves[i], myNumber, field))
                {
                    MessageBox.Show("Se jogar na coluna " + possibleMoves[i] + " tu impedes-me a seguir.", "NoDepth AI Tought");
                    naoSeguros1.Add(possibleMoves[i]);
                }
                else if (!isSafe(possibleMoves[i], opNumber, field))
                {
                    MessageBox.Show("Se jogar na coluna " + possibleMoves[i] + " tu ganhas a seguir.", "NoDepth AI Tought");
                    naoSeguros2.Add(possibleMoves[i]);
                }
                    
            }
            if (seguros.Count != 0)
                return seguros;
            else if (naoSeguros1.Count != 0)
                return naoSeguros1;
            else
                return naoSeguros2;
        }

        List<int> possibleMoves(QuatroField field)
        {
            List<int> plays = new List<int>();

            for (int i = 0; i < 9; i++)
            {
                if (field.canPlay(i))
                {
                    plays.Add(i);
                }
            }

            return plays;
        }
    }
}
