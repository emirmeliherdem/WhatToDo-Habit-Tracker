﻿using Hangfire;
using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(WhatToDo.Startup))]
namespace WhatToDo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");
            app.UseHangfireDashboard();
            RecurringJob.AddOrUpdate("easyjob", () => Controllers.HabitsController.UpdateHabits(), Cron.Hourly);
            app.UseHangfireServer();
        }
    }
}
