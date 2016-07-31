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
using System.Windows.Shapes;

namespace QAI.AIS.BruteSearch
{
    /// <summary>
    /// Interaction logic for BruteSearchWindow.xaml
    /// </summary>
    public partial class BruteSearchWindow : Window
    {
        BruteSearch AI;

        float[] localValues;

        System.Windows.Threading.DispatcherTimer dispatcherTimer;

        public BruteSearchWindow(BruteSearch AI)
        {
            this.AI = AI;
            InitializeComponent();

            localValues = new float[9];

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += UpdateTimer;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 250);
            dispatcherTimer.Start();
        }

        private void UpdateTimer(object sender, EventArgs e)
        {
            lock(AI.valuesLock)
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
            }

            UpdateValues(localValues);
        }

        public void UpdateValues(float[] values)
        {
            v0.Content = values[0].ToString("0.00");
            v1.Content = values[1].ToString("0.00");
            v2.Content = values[2].ToString("0.00");
            v3.Content = values[3].ToString("0.00");
            v4.Content = values[4].ToString("0.00");
            v5.Content = values[5].ToString("0.00");
            v6.Content = values[6].ToString("0.00");
            v7.Content = values[7].ToString("0.00");
            v8.Content = values[8].ToString("0.00");
        }
    }
}
