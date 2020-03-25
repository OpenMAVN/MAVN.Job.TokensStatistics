﻿using System.Threading.Tasks;
using Lykke.Job.TokensStatistics.Domain.Models;
using Falcon.Numerics;

namespace Lykke.Job.TokensStatistics.Domain.Repositories
{
    public interface ILastKnownStatsRepository
    {
        Task<ILastKnownStats> GetLastKnownStatsAsync();

        Task UpsertLastKnownStatsAsync(Money18 totalAmount, Money18 totalEarnedAmount, Money18 totalBurnedAmount, Money18 totalInCustomersWallets);
    }
}
