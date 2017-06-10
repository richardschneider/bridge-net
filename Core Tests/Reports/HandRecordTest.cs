using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Makaretu.Bridge.Reports
{
    [TestClass]
    public class HandRecordTest
    {
        [TestMethod]
        public void Properties()
        {
            var report = new HandRecord();
            Assert.AreEqual(3, report.Columns);
        }

        [TestMethod]
        public void Produce()
        {
            var tournament = new Session();
            tournament.GenerateBoards(2);
            var writer = new StringWriter();
            var report = new HandRecord() { Tournament = tournament };
            report.Produce(writer);
            //File.WriteAllText(@"c:\handout.html", writer.ToString(), Encoding.UTF8);
        }

    }
}
