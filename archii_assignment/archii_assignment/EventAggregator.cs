using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace archii_assignment
{
    public class Subscription<Tmessage> : IDisposable
    {
        public MethodInfo MethodInfo;
        private EventAggregator EventAggregator;
        public WeakReference TargetObjet;
        public bool IsStatic;

        private bool IsDisposed;

        public Subscription(Action<Tmessage> action, EventAggregator eventAggregator)
        {
            MethodInfo = action.Method;
            if (action.Target == null)
            {
                IsStatic = true;
            }
            TargetObjet = new WeakReference(action.Target);
            EventAggregator = eventAggregator;
        }
        ~Subscription()
        {
            if (!IsDisposed)
                Dispose();
        }

        public void Dispose()
        {
            EventAggregator.UnSubscribe(this);
            IsDisposed = true;
        }

        public Action<Tmessage> CreateAction()
        {
            if (TargetObjet.Target != null && TargetObjet.IsAlive)
            {
                return (Action<Tmessage>)Delegate.CreateDelegate(typeof(Action<Tmessage>), TargetObjet.Target, MethodInfo);
            }
            if (this.IsStatic)
            {
                return (Action<Tmessage>)Delegate.CreateDelegate(typeof(Action<Tmessage>), MethodInfo);
            }
            return null;
        }
    }

    public class EventAggregator
    {
        private object lockObj = new object();
        private Dictionary<Type, IList> subscriber;

        public EventAggregator()
        {
            subscriber = new Dictionary<Type, IList>();
        }

        public void Publish<TMessageType>(TMessageType message)
        {
            Type t = typeof(TMessageType);
            IList sublist;
            if (subscriber.ContainsKey(t))
            {
                lock(lockObj)
                {
                    sublist = new List<Subscription<TMessageType>>(subscriber[t].Cast<Subscription<TMessageType>>());
                }
                foreach(Subscription<TMessageType> sub in sublist)
                {
                    var action = sub.CreateAction();
                    if (action != null)
                        action(message);
                }
            }
        }

        public Subscription<TMessageType> Subscribe<TMessageType>(Action<TMessageType> action)
        {
            Type t = typeof(TMessageType);
            IList actionlist;
            var actiondetail = new Subscription<TMessageType>(action, this);
            lock(lockObj)
            {
                if (!subscriber.TryGetValue(t, out actionlist))
                {
                    actionlist = new List<Subscription<TMessageType>>();
                    actionlist.Add(actiondetail);
                    subscriber.Add(t, actionlist);
                }
                else
                {
                    actionlist.Add(actiondetail);
                }
            }
            return actiondetail;
        }

        public void UnSubscribe<TMessageType>(Subscription<TMessageType> subscription)
        {
            Type t = typeof(TMessageType);
            if (subscriber.ContainsKey(t))
            {
                lock(lockObj)
                {
                    subscriber[t].Remove(subscription);
                }
                subscription = null;
            }
        }
    }
}
