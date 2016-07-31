using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace QAI.AIS.Human
{
    class Human : QuatroPlayer
    {
        public override int playI(QuatroField field, InterfaceNotifier notifier)
        {
            GameCoords click;

            while (true)
            {
                click = notifier.getClick();

                if (click == null)
                    Thread.Sleep(100);
                else
                    break;
            }

            return click.column;
        }
    }
}
