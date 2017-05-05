using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlackHen.Bridge.Reports
{
    [TestClass]
    public class HandoutHtmlTest
    {
        [TestMethod]
        public void Properties()
        {
            var report = new HandoutHtml();
            Assert.AreEqual(3, report.Columns);
        }

        [TestMethod]
        public void Produce()
        {
            var tournament = new Tournament();
            tournament.GenerateBoards(25);
            var writer = new StringWriter();
            var report = new HandoutHtml() { Tournament = tournament };
            report.Produce(writer);
            File.WriteAllText(@"c:\handout.html", writer.ToString(), Encoding.UTF8);
        }

    }
}
