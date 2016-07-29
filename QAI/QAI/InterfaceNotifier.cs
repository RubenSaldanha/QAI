using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAI
{
    public class InterfaceNotifier
    {
        object clickLock;
        PointClick click;

        public void setClick(int x, int y)
        {
            lock (clickLock)
            {
                click = new PointClick(x, y);
            }
        }
        public PointClick getClick()
        {
            lock(clickLock)
            {
                if (click != null)
                    return new PointClick(click.x, click.y);
                else
                    return null;
            }
        }


        public class PointClick
        {
            public int x;
            public int y;

            public PointClick(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
    }
}
