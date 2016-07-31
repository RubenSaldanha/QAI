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

namespace QAI
{
    public abstract class QuatroPlayer
    {

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
    }
}
