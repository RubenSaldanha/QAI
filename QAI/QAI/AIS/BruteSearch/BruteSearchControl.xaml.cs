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

namespace QAI.AIS.BruteSearch
{
    /// <summary>
    /// Interaction logic for BruteSearchControl.xaml
    /// </summary>
    public partial class BruteSearchControl : UserControl
    {
        //Referenciar a AI
        BruteSearch AI;


        float[] localValues;

        public BruteSearchControl(BruteSearch AI)
        {
            this.AI = AI;
            InitializeComponent();

            localValues = new float[9];

            //-------Registar um método no evento Changed da AI-------
            AI.Changed += UpdateValues;
        }

        public void UpdateValues()
        {
            //--------- Utlizar esse método para dar update à parte visual, com base em valores que a AI tem publicos ----------
            lock (AI.valuesLock)
            {
                localValues[0] = AI.values[0];
                localValues[1] = AI.values[1];
                localValues[2] = AI.values[2];
                localValues[3] = AI.values[3];
                localValues[4] = AI.values[4];
                localValues[5] = AI.values[5];
                localValues[6] = AI.values[6];
                localValues[7] = AI.values[7];
                localValues[8] = AI.values[8];
                //LOL WTF, devia estar bebado quando fiz isto
            }

            UpdateValues(localValues);
        }

        public void UpdateValues(float[] values)
        {
            String feedbackString = "Valores:\n\n\n";

            for (int i = 0; i < 9; i++)
                feedbackString += "Jogada " + i + " : " + values[i].ToString("0.00") + "\n\n";

            mainLabel.Content = feedbackString;
        }
    }
}
