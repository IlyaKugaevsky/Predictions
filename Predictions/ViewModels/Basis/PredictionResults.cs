﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Predictions.ViewModels.Basis
{
    public class PredictionResults
    {
        public int Sum { get; set; }
        public bool Score { get; set; }
        public bool Difference { get; set; }
        public bool Outcome { get; set; }

    }
}