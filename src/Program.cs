using System;
using Starcounter;

namespace Timeline {
    class Program {
        static void Main() {
            Handle.GET("/Timeline", () => {
                var p = new IndexPage();
                p.Data = new object();
                return p;
            });
        }
    }
}