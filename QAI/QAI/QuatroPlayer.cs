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
using System.Windows.Threading;

namespace QAI
{
    public abstract class QuatroPlayer
    {
        public Dispatcher mainThread;

        public event Action Changed;

        public virtual Control GetFeedbackControl()
        {
            Label empty = new Label();
            empty.Content = "Nothing to show";
            return empty;
        }

        public virtual void StartGame()
        {

        }
        public abstract int playI(QuatroField field, InterfaceNotifier notifier);
        public virtual void EndGame()
        {

        }
        public virtual void Destroy()
        {

        }


        public virtual Control Analyse(QuatroField field)
        {
            Label empty = new Label();
            empty.Content = "Nothing to show";
            return empty;
        }

        protected void NotifyChanged()
        {
            if (mainThread != null)
            {
                Action fP = delegate { FireEvent(); };
                mainThread.BeginInvoke(fP);
            }
        }
        private void FireEvent()
        {
            if (Changed != null)
                Changed();
        }
    }
}
