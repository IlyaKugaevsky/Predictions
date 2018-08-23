﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Core.Models;
using Core.Models.Dtos;
using Persistence.DAL;
using Persistence.DAL.EntityFrameworkExtensions;
using Persistence.DAL.FetchStrategies;
using Persistence.DAL.FetchStrategies.TournamentsFetchStrategies;
using Persistence.DAL.FetchStrategies.ToursFetchStrategies;

namespace Services.Services
{
    public class TourService
    {
        private readonly IPredictionsContext _context;

        public TourService(IPredictionsContext context)
        {
            _context = context;
        }

        public TourDto GetTourDto(int tourId)
        {
            return _context.Tours.Single(t => t.TourId == tourId).GetDto();
        }

        public bool IsPlayoff(int tourId)
        {
            var tour = _context.Tours.Find(tourId);
            Debug.Assert(tour != null, nameof(tour) + " != null");
            return tour.IsPlayoff;
        }

        //tourNumber?
        public void UpdateTour(TourDto tourDto)
        {
            var tour = _context.Tours.Find(tourDto.TourId);
            if (tour == null) throw new KeyNotFoundException("no tour with Id = " + tourDto.TourId);
            
            tour.StartDate = tourDto.StartDate;
            tour.EndDate = tourDto.EndDate;
            _context.SaveChanges();
        }

        public List<Tour> GetLastTournamentTours()
        {
            var fetchStrategies = new IFetchStrategy<Tournament>[] { new FetchTours() };
            return _context.GetLastTournamentTours(fetchStrategies).ToList();
        }

        public List<Tour> GetLastTournamentSchedule()
        {
            var fetchStrategies = new IFetchStrategy<Tournament>[]
            {
                new FetchToursWithMatchesWithHomeTeam(),
                new FetchToursWithMatchesWithAwayTeam()
            };
            return _context.GetLastTournamentTours(fetchStrategies).ToList();
        }

        //not sure
        public List<Tuple<Expert, int>> GenerateTourPreresultlist(int tourId)
        {
            //var tour = EagerLoad(tourId, t => t.Matches.Select(m => m.Predictions));

            var fetchStrategies = new IFetchStrategy<Tour>[] { new FetchMatchesWithPredictions() };
            var tour = _context.GetTours(fetchStrategies).Single(t => t.TourId == tourId);
            var matches = tour.Matches;
            var predictions = matches.SelectMany(m => m.Predictions).ToList();
            var experts = _context.Experts.ToList();

            var tourPreresultList = new List<Tuple<Expert, int>>();

            for (var i = 0; i < experts.Count(); i++)
            {
                tourPreresultList.Add(
                    new Tuple<Expert, int>(
                        experts[i], 
                        predictions.Count(p => p.ExpertId == experts[i].ExpertId 
                        && !string.IsNullOrEmpty(p.Value))));
            }
            return tourPreresultList;
        }
    }
}