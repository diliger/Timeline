using Starcounter;
using System;
using System.Linq;

namespace Timeline {
    partial class IndexPage : Partial {
        Random r = new Random();
        int max = 100;

        protected override void OnData() {
            base.OnData();
            Dataset.Items.Add().Data = new TimelineItem(Dataset.Items.Count, r.Next(max), r.Next(max));
            Dataset.Items.Add().Data = new TimelineItem(Dataset.Items.Count, r.Next(max), r.Next(max));
            Dataset.Items.Add().Data = new TimelineItem(Dataset.Items.Count, r.Next(max), r.Next(max));

            Dataset.Events.Add().Data = new TimelineEvent("Event" + (Dataset.Events.Count + 1), "Normal", 0M, 1M);
            Dataset.Events.Add().Data = new TimelineEvent("Event" + (Dataset.Events.Count + 1), "Critical", 1.7M, 2M);
            Dataset.Events.Add().Data = new TimelineEvent("Event" + (Dataset.Events.Count + 1), "Normal", 2.1M, 2.3M);
        }

        void Handle(Input.AddItem action) {
            Dataset.Items.Add().Data = new TimelineItem(Dataset.Items.Max(val => val.Key) + 1, r.Next(max), r.Next(max));
        }

        void Handle(Input.RemoveItem action) {
            int count = Dataset.Items.Count;
            if (count == 0)
                return;
            int i = r.Next(count);
            Dataset.Items.RemoveAt(i);
        }

        void Handle(Input.UpdateItem action) {
            int count = Dataset.Items.Count;
            if (count == 0)
                return;
            int i = r.Next(count);

            var item = Dataset.Items[i];
            item.Value = r.Next(max);
            item.Predicted = r.Next(max);
        }

        void Handle(Input.AddEvent action) {
            int max = (int)Dataset.Items.Max(val => val.Key);
            decimal from = r.Next(max * 10) / 10M;
            string eventType = r.Next(this.max) % 2 == 0 ? "Normal" : "Critical";
            Dataset.Events.Add().Data = new TimelineEvent("Event" + Dataset.Events.Count, eventType, from, r.Next((int)(from * 10), max * 10) / 10M);
        }

        void Handle(Input.RemoveEvent action) {
            int count = Dataset.Events.Count;
            if (count == 0)
                return;
            int i = r.Next(count);
            Dataset.Events.RemoveAt(i);
        }

        void Handle(Input.UpdateEvent action) {
            int count = Dataset.Events.Count;
            if (count == 0)
                return;
            int i = r.Next(count);

            var item = Dataset.Events[i];

            int max = (int)Dataset.Items.Max(val => val.Key);
            decimal from = r.Next(max * 10) / 10M;
            string eventType = r.Next(this.max) % 2 == 0 ? "Normal" : "Critical";

            item.From = from;
            item.Type = eventType;
            item.To = r.Next((int)(from * 10), max * 10) / 10M;
            item.Name = "Event" + DateTime.Now.Second;
        }
    }

    public class TimelineItem {
        public TimelineItem() { }

        public TimelineItem(long Key, long Value, long Predicted) {
            this.Key = Key;
            this.Value = Value;
            this.Predicted = Predicted;
        }

        public long Key;
        public long Value;
        public long Predicted;
    }

    public class TimelineEvent {
        public TimelineEvent() { }

        public TimelineEvent(string Name, string Type, decimal From, decimal To) {
            this.Name = Name;
            this.Type = Type;
            this.From = From;
            this.To = To;
        }

        public string Name;
        public string Type;
        public decimal From;
        public decimal To;
    }
}
