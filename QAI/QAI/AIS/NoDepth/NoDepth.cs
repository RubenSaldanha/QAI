using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QAI.AIS.NoDepth
{
    public class NoDepth : QuatroPlayer
    {
        Random rdm = new Random();

        public String talk;

        public override int playI(QuatroField field, InterfaceNotifier notifier)
        {
            int play;
            int myNumber = field.PlayerTurn;
            int opNumber = myNumber == 1 ? 2 : 1;


            play = winNext(myNumber, field);
            if (play != -1)
            {
                talk = "Eu vou ganhar na coluna " + play;
                NotifyChanged();
                return play;
            }
                

            play = winNext(opNumber, field);
            if (play != -1)
            {
                talk = "Tu ias ganhar na coluna " + play;
                NotifyChanged();
                return play;
            }

            List<int> moves = triar(possibleMoves(field), myNumber, opNumber, field);
            
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
        
        public List<int> triar(List<int> possibleMoves, int myNumber, int opNumber, QuatroField field)
        {
            List<int> opWin = new List<int>();
            List<int> meTwoWays = new List<int>();
            List<int> opTwoWays = new List<int>();
            List<int> opOneWay = new List<int>();
            List<int> meOneWay = new List<int>();
            List<int> opCanStop = new List<int>();
            List<int> safe = new List<int>();
            
            for (int i = 0; i < possibleMoves.Count; i++)
            {
                // selection logic
                if (!isSafe(possibleMoves[i], opNumber, field))
                    opWin.Add(possibleMoves[i]);
                else if (nCaminhos(myNumber, possibleMoves[i], myNumber, field) > 1)
                    meTwoWays.Add(possibleMoves[i]);
                else if (nCaminhos(opNumber, possibleMoves[i], opNumber, field) > 1)
                    opTwoWays.Add(possibleMoves[i]);
                else if (!isSafe(possibleMoves[i], myNumber, field))
                    opCanStop.Add(possibleMoves[i]);
                else if (nCaminhos(myNumber, possibleMoves[i], myNumber, field) > 0)
                    meOneWay.Add(possibleMoves[i]);
                else if (nCaminhos(opNumber, possibleMoves[i], opNumber, field) > 0)
                    opOneWay.Add(possibleMoves[i]);
                else
                    safe.Add(possibleMoves[i]);
            }

            talk =  "meTwoWays -> " + string.Join(", ", meTwoWays) +
                    "\nopTwoWays -> " + string.Join(", ", opTwoWays) +
                    "\nmeOneWay -> " + string.Join(", ", meOneWay) +
                    "\nopOneWay -> " + string.Join(", ", opOneWay) +
                    "\nsafe -> " + string.Join(", ", safe) +
                    "\nopCanStop -> " + string.Join(", ", opCanStop) +
                    "\nopWin -> " + string.Join(", ", opWin) + "\n";
            
            if (meTwoWays.Count > 0)
            {
                talk += "Vou fazer dois caminhos.";
                NotifyChanged();
                return meTwoWays;
            }
            else if (opTwoWays.Count > 0)
            {
                talk += "Podias fazer dois caminhos!";
                NotifyChanged();
                return opTwoWays;
            }
            else if (meOneWay.Count > 0)
            {
                talk += "Vou fazer um caminho";
                NotifyChanged();
                return meOneWay;
            }
            else if (opOneWay.Count > 0)
            {
                talk += "Podias fazer um caminho";
                NotifyChanged();
                return opOneWay;
            }
            else if (safe.Count > 0)
            {
                talk += "Safe Monkey Move";
                NotifyChanged();
                return safe;
            }
            else if (opCanStop.Count > 0)
            {
                talk += "Podes impedir-me :c";
                NotifyChanged();
                return opCanStop;
            }
            else
            {
                talk += "Ganhas-te";
                NotifyChanged();
                return opWin;
            }
        }

        public int nCaminhos(int playerToPlay, int columnToPlay, int playerToScan, QuatroField field)
        {
            field.forcePlay(columnToPlay, playerToPlay);

            int countCaminhos = 0;
            int count = 1;
            int column = -1;

            for (int y = 0; y < 7; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    // para todas as jogadas verificar todas as possibilidades
                    if (field[y, x] == playerToScan)
                    {
                        // horizontal ->
                        if (x < 6)
                        {
                            for (int i = 1; i < 4; i++)
                                if (field[y, x + i] == playerToScan)
                                    count++;
                                else
                                    column = x + i;   // checkar gaps e.g: 1011

                            if (count == 3 && field.head(column) == y && field[y, x + 1] == playerToScan)
                                countCaminhos++;
                            else
                                count = 1;
                        }

                        // horizontal <-  
                        // NOTA: Gaps horizontais ja foram chekados em cima.
                        if (x > 2)
                        {
                            column = x - 3;
                            for (int i = 1; i < 3; i++)
                                if (field[y, x - i] == playerToScan)
                                    count++;

                            if (count == 3 && field.head(column) == y && field[y, x - 1] == playerToScan)
                                countCaminhos++;
                            else
                                count = 1;
                        }
                        
                        // vertical
                        if (y < 4)
                        {
                            for (int i = 1; i < 3; i++)
                                if (field[y + i, x] == playerToScan)
                                    count++;

                            if (count == 3 && field.head(x) == y + 3 && field[y + 1, x] == playerToScan)
                                countCaminhos++;
                            else
                                count = 1;
                        }

                        // diagonal declive 1 ->
                        if (x < 6 && y < 4)
                        {
                            int line = -10;
                            for (int i = 1; i < 4; i++)
                                if (field[y + i, x + i] == playerToScan)
                                    count++;
                                else
                                {
                                    column = x + i;   // checkar gaps
                                    line = y + i;
                                }


                            if (count == 3 && field.head(column) == line)
                                countCaminhos++;
                            else
                                count = 1;
                        }

                        // diagonal declive 1 <-
                        // NOTA: Gaps diagonais com declive 1 ja foram chekados em cima.
                        if (x > 2 && y > 2)
                        {
                            column = x - 3;
                            for (int i = 1; i < 3; i++)
                                if (field[y - i, x - i] == playerToScan)
                                    count++;

                            if (count == 3 && field.head(column) == y - 3 && field[y - 1, x - 1] == playerToScan)
                                countCaminhos++;
                            else
                                count = 1;
                        }


                        // diagonal declive -1 ->
                        if (x < 6 && y > 2)
                        {
                            int line = -10;
                            for (int i = 1; i < 4; i++)
                                if (field[y - i, x + i] == playerToScan)
                                    count++;
                                else
                                {
                                    column = x + i;   // checkar gaps
                                    line = y - i;
                                }


                            if (count == 3 && field.head(column) == line)
                                countCaminhos++;
                            else
                                count = 1;
                        }

                        // diagonal declive -1 <-
                        // NOTA: Gaps diagonais com declive -1 ja foram chekados em cima.
                        if (x > 2 && y < 4)
                        {
                            column = x - 3;
                            for (int i = 1; i < 3; i++)
                                if (field[y + i, x - i] == playerToScan)
                                    count++;

                            if (count == 3 && field.head(column) == y + 3 && field[y + 1, x - 1] == playerToScan)
                                countCaminhos++;
                            else
                                count = 1;
                        }


                    }
                }
            }
            field.undo();

            return countCaminhos;
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



        //Override method to return the visual control
        public override System.Windows.Controls.Control GetFeedbackControl()
        {
            return new NoDepthControl(this);
        }

        //public List<int> bestPlay(List<int> possibleMoves, int myNumber, int opNumber, QuatroField field)
        //{
        //    List<int> seguros = new List<int>();
        //    List<int> naoSeguros1 = new List<int>();
        //    List<int> naoSeguros2 = new List<int>();

        //    for (int i = 0; i < possibleMoves.Count; i++)
        //    {
        //        if (isSafe(possibleMoves[i], myNumber, field) && isSafe(possibleMoves[i], opNumber, field))
        //            seguros.Add(possibleMoves[i]);
        //        else if (!isSafe(possibleMoves[i], myNumber, field))
        //        {
        //            //talk = "Se jogar na coluna " + possibleMoves[i] + " tu impedes-me a seguir";
        //            //NotifyChanged();
        //            naoSeguros1.Add(possibleMoves[i]);
        //        }
        //        else if (!isSafe(possibleMoves[i], opNumber, field))
        //        {
        //            //talk = "Se jogar na coluna " + possibleMoves[i] + " tu ganhas a seguir";
        //            //NotifyChanged();
        //            naoSeguros2.Add(possibleMoves[i]);
        //        }

        //    }
        //    if (seguros.Count != 0)
        //        return seguros;
        //    else if (naoSeguros1.Count != 0)
        //        return naoSeguros1;
        //    else
        //        return naoSeguros2;
        //}
    }
}
