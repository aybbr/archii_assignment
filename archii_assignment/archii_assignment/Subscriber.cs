using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archii_assignment
{
    public class Subscriber
    {
        Subscription<Note> noteToken;
        Subscription<int> Tokenumber;
        EventAggregator eventAggregator;

        public  Subscriber(EventAggregator evento)
        {
            eventAggregator = evento;
            evento.Subscribe<Note>(this.Testnote);
            evento.Subscribe<int>(this.Testnumber);
        }

        private void Testnumber(int obj)
        {
            Console.WriteLine(obj);
            eventAggregator.UnSubscribe(Tokenumber);
        }

        private void Testnote(Note testnote)
        {
            Console.WriteLine(testnote.ToString());
            eventAggregator.UnSubscribe(noteToken);
        }
    }
}
