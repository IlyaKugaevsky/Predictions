﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Predictions.Models;
using Predictions.DAL;
using Predictions.ViewModels;

namespace Predictions.Services
{
    public class MatchService
    {
        public Match CreateMatch(int homeId, int awayId, int tourId, DateTime date, PredictionsContext context)
        {
            var match = new Match()
            {
                HomeTeam = context.Teams.Find(homeId),
                AwayTeam = context.Teams.Find(awayId),
                Date = date,
                TourId = tourId //TO FIX 
            };

            return match;
        }

        public void AddMatch(Match match, PredictionsContext context)
        {
            context.Matches.Add(match);
            context.SaveChanges();
        } 

        public List<MatchInfo> GenerateMatchlist(List<Match> matches)
        {
            var matchlist = new List<MatchInfo>();
            for (var i = 0; i < matches.Count; i++)
            {
                matchlist.Add(new MatchInfo(
                    matches[i].Date,
                    matches[i].HomeTeam.Title,
                    matches[i].AwayTeam.Title));
            }
            return matchlist;
        }
    }
}