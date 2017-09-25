using System.Collections.Generic;

namespace Ships_Server {
    
    public class EventSystem<T> {

        private readonly List<Handler> triggers;
        
        public EventSystem() {
            triggers = new List<Handler>();
        }

        public void addTrigger(Handler trigger) {
            triggers.Add(trigger);
        }

        public void removeTrigger(Handler trigger) {
            if (!triggers.Contains(trigger))
                return;
            triggers.Remove(trigger);
        }

        public void Invoke(string trigger, T data) {
            foreach (Handler handler in triggers) {
                if (handler.trigger == trigger)
                    handler.handler.Invoke(data);
            }
        }
        
        public delegate void OnTrigger(T data);

        public struct Handler {
            public readonly string trigger;
            public readonly OnTrigger handler;

            public Handler(string trigger, OnTrigger handler) {
                this.trigger = trigger;
                this.handler = handler;
            }
        }
        
        public static EventSystem<T> operator +(EventSystem<T> system, Handler handler) {
            system.addTrigger(handler);
            return system;
        }
        
        public static EventSystem<T> operator -(EventSystem<T> system, Handler handler) {
            system.removeTrigger(handler);
            return system;
        }
    }
}