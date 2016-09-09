﻿using Predictions.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Predictions.Models;
using Predictions.ViewModels;
using System.Net;
using System.Data.Entity;
using Predictions.Services;
using Predictions.ViewModels.Basis;

namespace Predictions.Controllers
{
    public class PredictionsController : Controller
    {
        private readonly PredictionsContext _context;
        private readonly ExpertService _expertService;
        private readonly TourService _tourService;
        private readonly PredictionService _predictionService;
        private readonly MatchService _matchService;

        public PredictionsController()
        {
            _context = new PredictionsContext();
            _expertService = new ExpertService(_context);
            _tourService = new TourService(_context);
            _predictionService = new PredictionService(_context);
            _matchService = new MatchService(_context);
        }

        public ActionResult PredictionsDisplay()
        {
            var headers = new List<string>() { "Дата", "Дома", "В гостях", "Прогноз" };
            var matchTable = new MatchTableViewModel(headers);
            var expertlist = _expertService.GenerateSelectList();
            var tourlist = _tourService.GenerateSelectList();
            var viewModel = new PredictionsDisplayViewModel(expertlist, tourlist, matchTable);
            return View(viewModel);
        }

        //bind, invalide model
        [HttpPost]
        public ActionResult PredictionsDisplay(PredictionsDisplayViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var headers = new List<string>() { "Дата", "Дома", "В гостях", "Прогноз" };
                var matchlist = _matchService.GenerateMatchlist(viewModel.SelectedTourId);
                var scorelist = _predictionService.GeneratePredictionlist(viewModel.SelectedTourId, viewModel.SelectedExpertId);
                var matchtable = new MatchTableViewModel(headers, matchlist, scorelist);
                viewModel.MatchTable = matchtable;
                return View(viewModel);
            }
            //wtf!
            else return HttpNotFound();
        }

        [HttpPost]
        public ActionResult GetMatchTable(int SelectedTourId, int SelectedExpertId)
        {
            var headers = new List<string>() { "Дата", "Дома", "В гостях", "Прогноз" };
            var matchlist = _matchService.GenerateMatchlist(SelectedTourId);
            var scorelist = _predictionService.GeneratePredictionlist(SelectedTourId, SelectedExpertId);
            var matchTable = new MatchTableViewModel(headers, matchlist, scorelist);
            return PartialView("MatchTable", matchTable);
        }
    }
}