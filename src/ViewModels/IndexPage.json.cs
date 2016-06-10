using Starcounter;
using System;

namespace Timeline {
    partial class IndexPage : Partial {
        Random r = new Random();
        protected override void OnData() {
            base.OnData();
            Dataset.Items.Add().Data = new TimelineItem(Dataset.Items.Count + 1, r.Next(0, 100), r.Next(0, 100));
            Dataset.Items.Add().Data = new TimelineItem(Dataset.Items.Count + 1, r.Next(0, 100), r.Next(0, 100));
            Dataset.Items.Add().Data = new TimelineItem(Dataset.Items.Count + 1, r.Next(0, 100), r.Next(0, 100));
        }

        void Handle(Input.AddItem action) {
            Dataset.Items.Add().Data = new TimelineItem(Dataset.Items.Count + 1, r.Next(0, 100), r.Next(0, 100));
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
}
