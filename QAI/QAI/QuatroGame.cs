using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAI
{
    public class QuatroGame
    {
        QuatroField field;
        public bool finished;
        private int _turn;
        public int Turn
        {
            get { return _turn; }
        }

        public QuatroGame()
        {
            field = new QuatroField();
            _turn = 1;
        }

    }
}
