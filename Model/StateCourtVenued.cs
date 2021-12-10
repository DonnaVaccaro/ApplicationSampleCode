using System;
using System.Collections.Generic;
using System.Text;

namespace OTS.Model
{
    public class StateCourtVenued
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(StateCourtVenued));
        private string id;
        private string stateCircuit;
        private string courtVenued;

        public StateCourtVenued(string id, string stateCircuit, string courtVenued)
        {
            this.Id = id;
            this.StateCircuit = stateCircuit;
            this.CourtVenued = courtVenued;
        }

        public string Id { get => id; set => id = value; }
        public string StateCircuit { get => stateCircuit; set => stateCircuit = value; }
        public string CourtVenued { get => courtVenued; set => courtVenued = value; }
    }
}
