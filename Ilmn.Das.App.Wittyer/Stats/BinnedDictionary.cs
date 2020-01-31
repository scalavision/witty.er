﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using Ilmn.Das.App.Wittyer.Stats.Counts;
using Ilmn.Das.App.Wittyer.Utilities;
using Ilmn.Das.App.Wittyer.Vcf.Variants;
using JetBrains.Annotations;

namespace Ilmn.Das.App.Wittyer.Stats
{
    internal class BinnedDictionary : IReadOnlyDictionary<uint, IMutableStats>
    {
        [NotNull] public readonly IReadOnlyList<uint> Bins;

        private readonly IDictionary<uint, IMutableStats> _statsDictionary;
        
        public BinnedDictionary([NotNull] IReadOnlyList<uint> bins, WittyerVariantType svType)
        {
            Bins = bins;
            _statsDictionary = bins.ToImmutableDictionary(b => b,
                b => WittyerConstants.BaseLevelStatsTypes.Contains(svType)
                    ? MutableEventAndBasesStats.Create()
                    : MutableEventStats.Create() as IMutableStats);
        }

        public IEnumerator<KeyValuePair<uint, IMutableStats>> GetEnumerator() => _statsDictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _statsDictionary.Count;
        public bool ContainsKey(uint key) => _statsDictionary.ContainsKey(key);

        public bool TryGetValue(uint key, out IMutableStats value) => _statsDictionary.TryGetValue(key, out value);

        public IMutableStats this[uint key] => _statsDictionary[key];

        public IEnumerable<uint> Keys => _statsDictionary.Keys;
        public IEnumerable<IMutableStats> Values => _statsDictionary.Values;
    }
}