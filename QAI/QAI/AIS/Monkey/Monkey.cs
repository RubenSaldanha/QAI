using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAI.AIS.Monkey
{
    class Monkey : QuatroPlayer
    {
        Random rdm = new Random();

        public override int playI(QuatroField field, InterfaceNotifier notifier)
        {
            return rdm.Next(9);
        }
    }
}
