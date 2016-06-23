using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archii_assignment
{
    public class Publisher
    {
        EventAggregator EventAggregator;
        public Publisher(EventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }
        public void PublishNote()
        {
            EventAggregator.Publish(new Note());
            EventAggregator.Publish(2);
        }
    }
}
