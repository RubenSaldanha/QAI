using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAI
{
    public class QuatroOptions
    {
        public float turnSeconds;

        public AutomaticPlay automaticPlay;
        public enum AutomaticPlay { StepAnalysis, Regular, Automatic}
    }
}
