﻿using Autofac;
using JetBrains.Annotations;
using Lykke.Common;
using Lykke.Job.TokensStatistics.Domain.Services;
using Lykke.Job.TokensStatistics.DomainServices.Services;
using Lykke.Job.TokensStatistics.PeriodicalHandlers;
using Lykke.Job.TokensStatistics.Services;
using Lykke.Job.TokensStatistics.Settings;
using Lykke.Sdk;
using Lykke.Sdk.Health;
using Lykke.Service.PrivateBlockchainFacade.Client;
using Lykke.SettingsReader;

namespace Lykke.Job.TokensStatistics.Modules
{
    [UsedImplicitly]
    public class JobModule : Module
    {
        private readonly IReloadingManager<AppSettings> _appSettings;

        public JobModule(IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>()
                .SingleInstance();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>()
                .AutoActivate()
                .SingleInstance();

            builder.RegisterType<TokensStatisticsService>()
                .As<ITokensStatisticsService>()
                .WithParameter("tokenDeviationTolerance", _appSettings.CurrentValue.TokensStatisticsJob.TokenDeviationTolerance)
                .SingleInstance();

            RegisterPeriodicalHandlers(builder);

            builder.RegisterPrivateBlockchainFacadeClient(_appSettings.CurrentValue.PrivateBlockchainFacadeService, null);
        }

        private void RegisterPeriodicalHandlers(ContainerBuilder builder)
        {
            builder.RegisterType<StoreTotalTokensPeriodicalHandler>()
                .As<IStartStop>()
                .SingleInstance();
        }
    }
}
