﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.ViewModels
{
    public class SubmitTextAreaViewModel
    {
        public SubmitTextAreaViewModel()
        { }
        public SubmitTextAreaViewModel(int tourId)
        {
            TourId = tourId;
        }

        public int TourId { get; set; }
        public string InputText { get; set; }
    }
}