﻿using System;

namespace Alitz.Collections;
public class NegativeIndexExtractedException : Exception
{
    public NegativeIndexExtractedException(Delegate extractorFunc)
    {
        ExtractorFunc = extractorFunc;
    }

    public Delegate ExtractorFunc { get; }
    public override string Message =>
        $"Extractor function {ExtractorFunc.Method.Name} has returned a negative index";
}