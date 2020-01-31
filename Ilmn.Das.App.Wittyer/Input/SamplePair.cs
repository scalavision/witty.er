﻿using Ilmn.Das.App.Wittyer.Utilities;
using JetBrains.Annotations;

namespace Ilmn.Das.App.Wittyer.Input
{
    public interface ISamplePair
    {
        /// <summary>
        /// Gets the name of the query sample.
        /// </summary>
        /// <value>
        /// The name of the query sample.
        /// </value>
        [CanBeNull]
        string QuerySampleName { get; }

        /// <summary>
        /// Gets the name of the truth sample.
        /// </summary>
        /// <value>
        /// The name of the truth sample.
        /// </value>
        [CanBeNull]
        string TruthSampleName { get; }
    }

    public class SamplePair : ISamplePair
    {
        /// <inheritdoc />
        public string QuerySampleName { get; }

        /// <inheritdoc />
        public string TruthSampleName { get; }

        private SamplePair([CanBeNull] string truth, [CanBeNull] string query)
        {
            QuerySampleName = query;
            TruthSampleName = truth;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SamplePair"/> class.
        /// </summary>
        /// <param name="truthName">The truth.</param>
        /// <param name="queryName">The query.</param>
        [NotNull, Pure]
        public static ISamplePair Create([NotNull] string truthName, [NotNull] string queryName)
            => new SamplePair(truthName, queryName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SamplePair"/> class.
        /// </summary>
        /// <param name="truthName">The truth.</param>
        [NotNull, Pure]
        public static ISamplePair CreateTruthOnly([NotNull] string truthName)
            => new SamplePair(truthName, null);

        /// <summary>
        /// Initializes a new instance of the <see cref="SamplePair"/> class.
        /// </summary>
        /// <param name="queryName">The truth.</param>
        [NotNull, Pure]
        public static ISamplePair CreateQueryOnly([NotNull] string queryName)
            => new SamplePair(null, queryName);

        public static readonly ISamplePair NullPair = new SamplePair(null, null);

        internal static readonly ISamplePair Default
            = Create(WittyerConstants.WittyMetaInfoLineKeys.DefaultTruthSampleName, WittyerConstants.WittyMetaInfoLineKeys.DefaultQuerySampleName);
    }
}