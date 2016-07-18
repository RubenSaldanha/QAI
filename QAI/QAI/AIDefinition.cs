using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAI
{
    class AIDefinition
    {
        public Func<QuatroPlayer> creator;
        public string name;

        public AIDefinition(Func<QuatroPlayer> creator, string name)
        {
            this.creator = creator;
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
