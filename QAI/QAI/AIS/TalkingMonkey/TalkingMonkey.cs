using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAI.AIS.TalkingMonkey
{
    public class TalkingMonkey : QuatroPlayer
    {
        Random rdm = new Random();

        //The public variable exposing the talk
        public String talk;

        public override int playI(QuatroField field, InterfaceNotifier notifier)
        {
            int play = rdm.Next(9);

            //Change the talk every time the monkey plays
            int choice = rdm.Next(3);
            switch(choice)
            {
                case 0:
                    talk = "YOU ARE GONNA LOOSE!!!";
                    break;
                case 1:
                    talk = "MuuhaAHahaham Noob!";
                    break;
                case 2:
                    talk = "You are so bad! You Lost IT!!";
                    break;
            }

            //Notify visuals the talk has changed
            NotifyChanged();

            return play;
        }

        public override void StartGame()
        {
            talk = "Lets DO THIS!!";
            NotifyChanged();
        }
        public override void EndGame()
        {
            talk = "I WOOOONNNN!!!!";
            NotifyChanged();
        }

        //Override method to return the visual control
        public override System.Windows.Controls.Control GetFeedbackControl()
        {
            return new TalkingMonkeyControl(this);
        }
    }
}
