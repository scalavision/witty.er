﻿using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Ilmn.Das.App.Wittyer.Infrastructure;
using Ilmn.Das.App.Wittyer.Input;
using Ilmn.Das.App.Wittyer.Stats;
using Ilmn.Das.Core.Tries.Extensions;
using Ilmn.Das.Std.AppUtils.Misc;
using Xunit;

namespace Ilmn.Das.App.Wittyer.Test
{
    public class TinyTest
    {
        private static readonly FileInfo TinyTruth =
            Path.Combine("Resources", "Tiny", "truth.vcf").ToFileInfo();

        private static readonly FileInfo TinyQuery =
            Path.Combine("Resources", "Tiny", "query.vcf").ToFileInfo();
        
        [Fact]
        public void CrossType_Works()
        {
            var outputDirectory = Path.GetRandomFileName().ToDirectoryInfo();
            var inputSpecs = InputSpec.GenerateDefaultInputSpecs(false).Select(i => InputSpec.Create(i.VariantType, i.BinSizes,
                    10000, i.PercentDistance, i.ExcludedFilters, i.IncludedFilters, i.IncludedRegions))
                .ToDictionary(i => i.VariantType, i => i);
            var wittyerSettings = WittyerSettings.Create(outputDirectory, TinyTruth, TinyQuery,
                ImmutableList<ISamplePair>.Empty, EvaluationMode.CrossTypeAndSimpleCounting,
                inputSpecs);

            var (_, query, truth) = MainLauncher.GenerateResults(wittyerSettings).EnumerateSuccesses().First();
            var results = MainLauncher.GenerateSampleMetrics(truth, query, false, inputSpecs);
            Assert.Equal(4U, results.OverallStats[StatsType.Event].QueryStats.TrueCount);
        }
    }
}