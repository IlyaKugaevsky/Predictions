﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Predictions.Models;
using Predictions.DAL;
using Predictions.ViewModels;
using Predictions.ViewModels.Basis;
using System.Data.Entity;
using Predictions.DAL.FetchStrategies;
using Predictions.DAL.FetchStrategies.TournamentFetchStrategies;

namespace Predictions.Services
{
    public class MatchService
    {
        private readonly PredictionsContext _context;

        public MatchService(PredictionsContext context)
        {
            _context = context;
        }


        public List<Match> GetLastTournamentMatchesByTourId(int? tourId)
        {
            if (tourId == null) return null;

            var fetchStrategies = new IFetchStrategy<Tournament>[]
            {
                new ToursWithMatchesWithHomeTeam(),
                new ToursWithMatchesWithAwayTeam()
            };

            var tours = _context.GetLastTournamentTours(fetchStrategies);
            var tour = tours.Single(t => t.TourId == tourId.Value);

            return tour.Matches.ToList();
        }

        public List<MatchInfo> GenerateMatchlist(int tournamentId, int tourId)
        {
            var fetchStrategies = new IFetchStrategy<Tournament>[]
            {
                new ToursWithMatchesWithHomeTeam(),
                new ToursWithMatchesWithAwayTeam()
            };

            var tours = _context.GetLastTournamentTours(fetchStrategies);
            var tour = tours.Single(t => t.TourId == tourId);

            return tour.Matches.Select(m => m.GetMatchInfo()).ToList();
        }

        public List<FootballScore> GenerateScorelist(int? tourId, bool editable = false, string emptyDisplay = "-")
        {
            if (tourId == null) return null;

            //var tour = _context.Tours
            //        .Include(t => t.Matches
            //            .Select(m => m.HomeTeam))
            //        .Include(t => t.Matches
            //            .Select(m => m.AwayTeam))
            //        .Single(t => t.TourId == tourId);

            var tour = _context.ToursWithMatchesWithTeams().Single(t => t.TourId == tourId);

            if (tour == null) return null;
            //return tour.Matches.Select(m => new FootballScore
            //{
            //    Value = (String.IsNullOrEmpty(m.Score) && editable == false) ? emptyDisplay : m.Score,
            //    Editable = editable 
            //}).ToList();

            return tour.Matches.Select(m => m.GetFootballScore(editable, emptyDisplay)).ToList();

        }

        public List<FootballScore> GenerateScorelist(List<Match> matches, bool editable = false, string emptyDisplay = "-")
        {
            //return matches.Select(m => new FootballScore
            //{
            //    Value = (String.IsNullOrEmpty(m.Score) && editable == false) ? emptyDisplay : m.Score,
            //    Editable = editable
            //}).ToList();

            return matches.Select(m => m.GetFootballScore(editable, emptyDisplay)).ToList();
        }

        //comand + query
        //move to Model
        //public Match CreateMatch(DateTime date, int homeId, int awayId, int tourId)
        //{
        //    var match = new Match()
        //    {
        //        Date = date,
        //        HomeTeam = _context.Teams.Find(homeId),
        //        AwayTeam = _context.Teams.Find(awayId),
        //        TourId = tourId, 
        //        Score = string.Empty
        //    };
        //    return match;
        //}

        public List<Match> CreateMatches(List<MatchInfo> matches, int tourId)
        {
            var teams = _context.Teams.ToList();
            return matches.Select(m => new Match()
            {
                Date = m.Date,
                HomeTeam = teams.Single(t => t.Title == m.HomeTeamTitle),
                AwayTeam = teams.Single(t => t.Title == m.AwayTeamTitle),
                TourId = tourId,
                Score = string.Empty
            }).ToList();
        }

        public void AddMatch(Match match)
        {
            _context.Matches.Add(match);
            _context.SaveChanges();
        }

        public void AddMatches(List<Match> matches)
        {
            _context.Matches.AddRange(matches);
            _context.SaveChanges();
        }


        public int? GetTourId(int? matchId)
        {
            if (matchId == null) return null;
            var match = _context.Matches.Find(matchId);
            if (match == null) return null;
            return match.TourId;
        }

        //always null-check before execute this
        public void DeleteMatch(int? id)
        {
            var match = _context.Matches.Find(id);
            _context.Matches.Remove(match);
            _context.SaveChanges();
        }

        public void AddScores(IList<Match> matches, IList<FootballScore> scorelist)
        {
            for (var i = 0; i < matches.Count(); i++)
            {
                matches[i].Score = scorelist[i].Value;
            }
            _context.SaveChanges();
        }
    }
}