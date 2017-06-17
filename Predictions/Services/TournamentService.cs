﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Predictions.DAL;

namespace Predictions.Services
{
    public class TournamentService
    {
        private readonly PredictionsContext _context;

        public TournamentService(PredictionsContext context)
        {
            _context = context;
        }

        public int GetCurrentTournamentId()
        {
            return _context.Tournaments
                .OrderByDescending(t => t.TournamentId)
                .First()
                .TournamentId;
        }
    }
}