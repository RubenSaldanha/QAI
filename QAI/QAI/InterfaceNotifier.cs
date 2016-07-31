using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAI
{
    public class InterfaceNotifier
    {
        object clickLock = new object();
        GameCoords click;

        public void setClick(int line, int column)
        {
            lock (clickLock)
            {
                click = new GameCoords(line, column);
            }
        }
        public GameCoords getClick()
        {
            lock(clickLock)
            {
                if (click != null)
                    return new GameCoords(click.line, click.column);
                else
                    return null;
            }
        }
    }
}
