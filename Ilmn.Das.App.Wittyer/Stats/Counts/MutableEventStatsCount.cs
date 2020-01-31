﻿using Ilmn.Das.Std.AppUtils.Comparers;
using JetBrains.Annotations;

namespace Ilmn.Das.App.Wittyer.Stats.Counts
{
    internal interface IMutableEventStatsCount : IBasicStatsCount
    {
        void AddTrueEvent();

        void AddFalseEvent();
    }
    internal class MutableEventStatsCount : IMutableEventStatsCount
    { 
        private MutableEventStatsCount(uint overlapCount, uint nonOverlapCount)
        {
            TrueCount = overlapCount;
            FalseCount = nonOverlapCount;
        }

        internal static IMutableEventStatsCount Create(uint overlapCount, uint nonOverlapCount) 
            => new MutableEventStatsCount(overlapCount, nonOverlapCount);

        internal static IMutableEventStatsCount Create() => Create(0, 0);

        public uint TrueCount { get; private set; }
        public uint FalseCount { get; private set; }
        public void AddTrueEvent() => TrueCount++;

        public void AddFalseEvent() => FalseCount++;

        protected bool Equals(MutableEventStatsCount other) 
            => TrueCount == other.TrueCount && FalseCount == other.FalseCount;

        public bool Equals([CanBeNull] IBasicStatsCount other) 
            => ComparerUtils.HandleNullEqualityComparison(this, other) ?? 
               // ReSharper disable once PossibleNullReferenceException
               TrueCount == other.TrueCount && FalseCount == other.FalseCount;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MutableEventStatsCount) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = 418394411;
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            hashCode = hashCode * -1521134295 + TrueCount.GetHashCode();
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            hashCode = hashCode * -1521134295 + FalseCount.GetHashCode();
            return hashCode;
        }
    }
}
