using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAI.AIS.BruteSearch
{
    public class BruteSearch : QuatroPlayer
    {
        private int profundidade;
        private int player_;
        private float valorDeFundo;
        private int operations;
        Random rdm;

        BruteSearchWindow win;

        bool playing;
        private float[] jogadas;

        public Object valuesLock = new object();
        public float[] values;

        public BruteSearch()
        {
            profundidade = 3;
            valorDeFundo = 0.5f;
            operations = 0;
            rdm = new Random();

            jogadas = new float[9];
            values = new float[9];

            win = new BruteSearchWindow(this);
            win.Show();
        }

        public override int playI(QuatroField field, InterfaceNotifier notifier)
        {
            playing = true;
            lock(valuesLock)
            {
                for (int i = 0; i < values.Length; i++)
                    values[i] = -1;
            }

            int bestPlay = search(field);

            playing = false;
            lock(valuesLock)
            {
                for (int i = 0; i < values.Length; i++)
                    values[i] = jogadas[i];
            }

            return bestPlay;
	    }

        private int search(QuatroField field)
        {
            player_ = field.PlayerTurn;

            float[] jogadasAdversarias = new float[9];
            bool victoriaAdversaria = false;
            float valorMaximo = 0;
            float random = (float)rdm.NextDouble();
            int numeroMaximos = 0;
            operations = 0;


            for (int k = 0; k < jogadas.Length; k++)
            {
                jogadas[k] = -1;
            }

            for (int i = 0; i < 9; i++)
            {
                if (field.canPlay(i))
                {
                    field.play(i);

                    if (field.PlayerWon)
                    {
                        field.undo();
                        jogadas[i] = 1;
                        return i;
                    }
                    else
                        field.undo();
                }
            }


            for (int i = 0; i < 9; i++)
            {
                victoriaAdversaria = false;

                if (field.canPlay(i))
                {
                    field.play(i);


                    for (int j = 0; j < 9; j++)
                    {
                        if (field.canPlay(j))
                        {
                            field.play(j);

                            if (field.PlayerWon)
                            {
                                jogadas[i] = -1;
                                victoriaAdversaria = true;
                                field.undo();
                                break;
                            }
                            field.undo();
                        }
                    }

                    if (!victoriaAdversaria)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            if (field.canPlay(j))
                            {
                                field.play(j);

                                jogadasAdversarias[j] = valor(field, 1);

                                field.undo();
                            }
                        }

                        jogadas[i] = min(jogadasAdversarias);
                    }

                    field.undo();
                }
            }


            numeroMaximos = 1;
            valorMaximo = jogadas[0];
            for (int i = 1; i < 9; i++)
            {
                if (jogadas[i] >= valorMaximo && field.canPlay(i))
                {
                    if (jogadas[i] == valorMaximo)
                    {
                        numeroMaximos++;
                    }
                    else
                    {
                        numeroMaximos = 1;
                        valorMaximo = jogadas[i];
                    }
                }
            }

            random = (random * numeroMaximos) + 1;
            numeroMaximos = 1;
            for (int i = 0; i < 9; i++)
            {
                if (jogadas[i] == valorMaximo && field.canPlay(i))
                {
                    if ((int)(random) == numeroMaximos)
                    {
                        return i;
                    }
                    else
                    {
                        numeroMaximos++;
                    }
                }
            }

            return 0;
        }

        private float valor(QuatroField field, int profundidadeActual)
        {
            float media = 0;
            float[] plays = new float[9];
            float[] valoresNoFimDoAdversario = new float[9];
            int count = 0;
            bool victoriaAdversaria = false;
            //int ope = operations;

            //operations++;

            // --------------- caso limite de profundidade ---------------
            if (profundidadeActual >= profundidade)
                return valorDeFundo;


            // --------------- calcular vitoria imediata ---------------
            for (int i = 0; i < 9; i++)
            {
                if (field.canPlay(i))
                {
                    field.play(i);

                    if (field.PlayerWon)
                    {
                        field.undo();
                        return 1;
                    }
                    else
                        field.undo();
                }
            }


            // --------------- calculo geral ------------------------------
            for (int i = 0; i < 9; i++)
            {
                if (field.canPlay(i))
                {
                    victoriaAdversaria = false;
                    count++;

                    field.play(i);


                    // --------------- calcular os valores consoante as jogadas adversarias ---------------
                    for (int j = 0; j < 9; j++)
                    {
                        if (field.canPlay(j))
                        {
                            field.play(j);

                            if (field.PlayerWon)
                            {
                                victoriaAdversaria = true;
                                field.undo();
                                break;
                            }
                            field.undo();
                        }
                    }

                    if (!victoriaAdversaria)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            if (field.canPlay(j))
                            {
                                field.play(j);


                                valoresNoFimDoAdversario[j] = valor(field, profundidadeActual + 1);
                                //System.out.println(ope);


                                field.undo();
                            }
                        }

                        //media += min(valoresNoFimDoAdversario);
                        plays[i] = min(valoresNoFimDoAdversario);
                    }
                    else
                    {
                        //media += 0; //o jogador adversario conseguiu ganhar, SUCKER!!!
                        plays[i] = -1;
                    }

                    field.undo();
                }
                else
                    plays[i] = -1;
            }

            //media = media / count;

            //return media;
            return max(plays);
        }

        public float max(float[] array)
        {
            float max = array[0];

            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] > max)
                    max = array[i];
            }

            return max;
        }
        public int maxIndex(double[] array)
        {
            double max = array[0];
            int indice = 0;

            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] > max)
                {
                    max = array[i];
                    indice = i;
                }
            }

            return indice;
        }
        public float min(float[] array)
        {
            float min = array[0];

            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] < min)
                    min = array[i];
            }

            return min;
        }
        public float mean(float[] array)
        {
            float mean = 0;
            for (int i = 0; i < array.Length; i++)
                mean += array[i];

            return mean / array.Length;
        }

        public override void Destroy()
        {
            win.Close();
        }
    }
}
