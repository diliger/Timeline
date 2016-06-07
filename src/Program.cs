using System;
using Starcounter;

namespace Timeline {
    class Program {
        static void Main() {
            Handle.GET("/Timeline", () => {
                return new IndexPage();
            });
        }
    }
}