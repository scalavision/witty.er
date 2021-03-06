﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Ilmn.Das.App.Wittyer.Vcf.Variants;
using Ilmn.Das.Core.Tries.Extensions;
using Ilmn.Das.Std.BioinformaticUtils.Genomes;
using Ilmn.Das.Std.VariantUtils.Vcf.Parsers;
using Ilmn.Das.Std.VariantUtils.Vcf.Variants;
using Ilmn.Das.Std.XunitUtils;
using JetBrains.Annotations;
using Xunit;

namespace Ilmn.Das.App.Wittyer.Test
{
    public static class BorderDistanceTest
    {
        private const string Query =
            "15\t83895501\tDRAGEN:LOSS:83895501:83905001\tN\t<DEL>\t45\tPASS\tSVTYPE=CNV;END=83905001;SVLEN=9500\tSM:CN:BC:PE\t0.504136:1:19:41,52";

        private const string Truth = "15\t83895314\t.\tN\t<CNV>\t.\tPASS\tEND=83905837;MCC=.;SVTYPE=CNV\tCN\t1";

        private const string QueryWithCiPos =
            "15\t83895501\tDRAGEN:LOSS:83895501:83905001\tN\t<DEL>\t45\tPASS\tSVTYPE=CNV;END=83905001;SVLEN=9500;CIPOS=-100,100\tSM:CN:BC:PE\t0.504136:1:19:41,52";

        private const string TruthWithCiEnd =
            "15\t83895314\t.\tN\t<CNV>\t.\tPASS\tEND=83905837;MCC=.;SVTYPE=CNV;CIEND=-150,150\tCN\t1";

        //breakend test
        private const string QueryBnd1 =
            "1\t231176\tMantaBND:0:4499:4500:0:0:0:1\tA\t[4:191034952[A\t303\tPASS\tSVTYPE=BND;CIPOS=-101,101\tGT\t0/1";

        private const string QueryBnd2 =
            "4\t191034952\tMantaBND:0:4499:4500:0:0:0:0\tT\t[1:231176[T\t303\tPASS\tSVTYPE=BND\tGT\t0/1";

        private const string TruthBnd1 =
            "1\t231160\tMantaBND:0:4499:4500:0:0:0:1\tA\t[4:191034930[A\t303\tPASS\tSVTYPE=BND\tGT\t0/1";

        private const string TruthBnd2 =
            "4\t191034930\tMantaBND:0:4499:4500:0:0:0:0\tT\t[1:231160[T\t303\tPASS\tSVTYPE=BND;CIPOS=-50,50\tGT\t0/1";

        private const double PercentDistance = 0.05;

        private static readonly IReadOnlyList<uint> Bins = ImmutableList.Create(1000U, 10000U);

        private const uint BasepairDistance = 500;

        [Fact]
        public static void WorksWithouCi()
        {
            var query = CreateWittyerVariant(Query);
            var truth = CreateWittyerVariant(Truth);

            var expected = BorderDistance.CreateFromVariant(query, truth);
            MultiAssert.Equal(187U, expected.PosBorderLeft);
            MultiAssert.Equal(187U, expected.PosBorderRight);
            MultiAssert.Equal(836U, expected.EndBorderLeft);
            MultiAssert.Equal(836U, expected.EndBorderRight);
            MultiAssert.AssertAll();
        }

        [Fact]
        public static void WorksWithCi()
        {
            var query = CreateWittyerVariant(QueryWithCiPos);
            var truth = CreateWittyerVariant(TruthWithCiEnd);
            var expected = BorderDistance.CreateFromVariant(query, truth);

            MultiAssert.Equal(87U, expected.PosBorderLeft);
            MultiAssert.Equal(287U, expected.PosBorderRight);
            MultiAssert.Equal(686U, expected.EndBorderLeft);
            MultiAssert.Equal(986U, expected.EndBorderRight);
            MultiAssert.AssertAll();
        }

        [Fact]
        public static void WorksWithBreakend()
        {
            var query = CreateWittyerBnd(QueryBnd1, QueryBnd2);
            var truth = CreateWittyerBnd(TruthBnd1, TruthBnd2);

            var expected = BorderDistance.CreateFromVariant(query, truth);
            MultiAssert.Equal(85U, expected.PosBorderLeft);
            MultiAssert.Equal(117U, expected.PosBorderRight);
            MultiAssert.Equal(72U, expected.EndBorderLeft);
            MultiAssert.Equal(28U, expected.EndBorderRight);
            MultiAssert.AssertAll();
        }

        [NotNull]
        private static IWittyerSimpleVariant CreateWittyerVariant([NotNull] string vcfline)
        {
            var baseVariant = VcfVariant.TryParse(vcfline,
                    VcfVariantParserSettings.Create(ImmutableList.Create("normal"), GenomeAssembly.Grch37))
                .GetOrThrowDebug();
            WittyerType.ParseFromVariant(baseVariant, true, null, out var svType);

            if (svType == null)
                throw new NotSupportedException("This method does not handle svType null");

            return WittyerVariantInternal
                .Create(baseVariant, baseVariant.Samples["normal"], svType, Bins, PercentDistance, BasepairDistance);
        }


        [NotNull]
        private static IWittyerBnd CreateWittyerBnd([NotNull] string bndLine1, [NotNull] string bndLine2)
        {
            var variant = VcfVariant.TryParse(bndLine1,
                    VcfVariantParserSettings.Create(ImmutableList.Create("normal"), GenomeAssembly.Grch37))
                .GetOrThrowDebug();
            return WittyerBndInternal.Create(variant, variant.Samples["normal"], WittyerType.TranslocationBreakend, Bins,
                BasepairDistance, PercentDistance, VcfVariant.TryParse(bndLine2,
                        VcfVariantParserSettings.Create(ImmutableList.Create("normal"), GenomeAssembly.Grch37))
                    .GetOrThrowDebug());
        }
    }
}