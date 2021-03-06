﻿using Ilmn.Das.Std.VariantUtils.Vcf.Variants;

namespace Ilmn.Das.App.Wittyer.Vcf.Variants
{
    public interface IWittyerBnd : IWittyerSimpleVariant
    {
        IVcfVariant EndOriginalVariant { get; }
    }

    internal interface IMutableWittyerBnd : IWittyerBnd, IMutableWittyerSimpleVariant
    {

    }
}