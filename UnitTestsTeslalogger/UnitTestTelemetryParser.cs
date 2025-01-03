﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeslaLogger;

namespace UnitTestsTeslalogger
{
    [TestClass]
    public class UnitTestTelemetryParser
    {
        bool expectedACCharge = false;

        [TestMethod]
        public void ACCharging1()
        {
            Car c = new Car(0, "", "", 0, "", DateTime.Now, "", "", "", "", "", "5YJ3E7EA3LF700000", "", null, false);

            var telemetry = new TelemetryParser(c);
            telemetry.databaseCalls = false;
            telemetry.handleACChargeChange += Telemetry_handleACChargeChange;

            var lines = LoadData("../../testdata/ACCharging1.txt");

            for (int i = 0; i < lines.Count; i++)
            {
                if (i == 7)
                    expectedACCharge = true;

                telemetry.handleMessage(lines[i]);

                Assert.AreEqual(expectedACCharge, telemetry.acCharging);
            }
        }

        [TestMethod]
        public void ACChargingJustPreheating()
        {
            Car c = new Car(0, "", "", 0, "", DateTime.Now, "", "", "", "", "", "5YJ3E7EA3LF700000", "", null, false);

            var telemetry = new TelemetryParser(c);
            telemetry.databaseCalls = false;
            telemetry.handleACChargeChange += Telemetry_handleACChargeChange;

            var lines = LoadData("../../testdata/ACChargingJustPreheating.txt");

            for (int i = 0; i < lines.Count; i++)
            {
                
                if (i == 25)
                    expectedACCharge = true;
                
                
                telemetry.handleMessage(lines[i]);

                Assert.AreEqual(expectedACCharge, telemetry.acCharging);
            }
        }

        private void Telemetry_handleACChargeChange(object sender, EventArgs e)
        {
            TelemetryParser telemetryParser = (TelemetryParser)sender;
            Assert.AreEqual(expectedACCharge, telemetryParser.acCharging);
        }

        List<string> LoadData(string path)
        {
            List<string> data = new List<string>();
            string[] lines = System.IO.File.ReadAllLines(path);
            foreach (string line in lines)
            {
                string s = line.Substring(line.IndexOf("*** FT:") + 7);
                data.Add(s);
            }
            return data;
        }
    }
}