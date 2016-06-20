using System;
using Starcounter;

namespace Timeline {
    class Program {
        static void Main() {
            var app = Application.Current;
            app.Use(new HtmlFromJsonProvider());
            app.Use(new PartialToStandaloneHtmlProvider());

            Handle.GET("/Timeline", () => {
                var p = new IndexPage();
                p.Data = new object();
                p.Session = new Session(SessionOptions.PatchVersioning);
                return p;
            });
        }
    }
}