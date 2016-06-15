using Starcounter;
using System;
using System.Linq;

namespace Timeline {
    partial class IndexPage : Partial {
        Random r = new Random();
        int max = 100;

        protected override void OnData() {
            base.OnData();
            for (int i = 0; i < 15; i++) {
                Dataset.Items.Add().Data = new TimelineItem(Dataset.Items.Count, r.Next(max), r.Next(max));
            }

            AddTimelineEvent();
            AddTimelineEvent();
            AddTimelineEvent();

            //Dataset.Events.Add().Data = new TimelineEvent(Dataset.Items.Count, "Event" + (Dataset.Events.Count + 1), "Normal", 0M, 1M);
            //Dataset.Events.Add().Data = new TimelineEvent(Dataset.Items.Count, "Event AAA " + (Dataset.Events.Count + 1), "Critical", 1.7M, 2M);
            //Dataset.Events.Add().Data = new TimelineEvent(Dataset.Items.Count, "Event BB" + (Dataset.Events.Count + 1), "Normal", 2.1M, 2.3M);
            //Dataset.Events.Add().Data = new TimelineEvent(Dataset.Items.Count, "Event CCCC " + (Dataset.Events.Count + 1), "Normal", 2.5M, 2.5M);
        }

        void Handle(Input.AddItem action) {
            var item = new TimelineItem(Dataset.Items.Max(val => val.Key) + 1, r.Next(max), r.Next(max));
            if (item.Predicted % 2 != 0) {
                item.Predicted = 0;
            } else if (item.Value % 2 != 0) {
                item.Value = 0;
            }
            Dataset.Items.Add().Data = item;
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
            AddTimelineEvent();
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

        private void AddTimelineEvent() {
            int max = (int)Dataset.Items.Max(val => val.Key);
            decimal from = r.Next(max * 10) / 10M;
            string eventType = r.Next(this.max) % 2 == 0 ? "Normal" : "Critical";
            Dataset.Events.Add().Data = new TimelineEvent(Dataset.Events.Count, "Event" + Dataset.Events.Count, eventType, from, r.Next((int)from * 10, (int)Math.Min((int)(from * 10 * 2), max * 10)) / 10M);
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

        public TimelineEvent(long Key, string Name, string Type, decimal From, decimal To) {
            this.Key = Key;
            this.Name = Name;
            this.Type = Type;
            this.From = From;
            this.To = To;
        }

        public long Key;
        public string Name;
        public string Type;
        public decimal From;
        public decimal To;
    }
}
