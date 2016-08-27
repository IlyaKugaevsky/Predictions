﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Predictions.Models
{
    public class Prediction
    {
        public int PredictionId { get; set; }
        public string Value { get; set; }

        public int? Sum { get; set; }
        public bool? Score { get; set; }
        public bool? Difference { get; set; }
        public bool? Outcome { get; set; }

        public bool IsClosed { get; set; }


        public int MatchId { get; set; }
        public Match Match { get; set; }

        public int ExpertId { get; set; }
        public Expert Expert { get; set; }
    }
}