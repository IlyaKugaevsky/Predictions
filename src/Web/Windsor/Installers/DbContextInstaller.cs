﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Persistence.DAL;

namespace Web.Windsor.Installers
{
    public class DbContextInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IPredictionsContext>()
                    .ImplementedBy<PredictionsContext>()
                    .LifestylePerWebRequest());
        }
    }
}