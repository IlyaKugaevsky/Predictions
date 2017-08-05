﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Predictions.Core.Models;

namespace Predictions.DAL.FetchStrategies.ToursFetchStrategies
{
    public class FetchMatches : IFetchStrategy<Tour>
    {
        public Expression<Func<Tour, object>> Apply()
        {
            return t => t.Matches;
        }
    }
}