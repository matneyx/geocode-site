﻿// <copyright file="TestFixture.cs" company="Objectivity Bespoke Software Specialists">
// Copyright (c) Objectivity Bespoke Software Specialists. All rights reserved.
// </copyright>
// <license>
//     The MIT License (MIT)
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//     The above copyright notice and this permission notice shall be included in all
//     copies or substantial portions of the Software.
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//     SOFTWARE.
// </license>

using System;
using Ocaramba;

namespace Geocod.io.Demo.E2E.Framework;

/// <summary>
///     The test fixture class for all tests
///     run once per test suite
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class UiTestFixture : TestBase, IDisposable
{
    public readonly DriverContext DriverContext;
    public readonly AppSettings AppSettings;

    /// <summary>
    ///     Initializes a new instance of the <see cref="UiTestFixture" /> class.
    /// </summary>
    public UiTestFixture(DriverContext driverContext)
    {
        DriverContext = new DriverContext
        {
            CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory
        };

        AppSettings = new AppSettings
        {
            Url = BaseConfiguration.Url
        };

        Console.WriteLine(BaseConfiguration.TestBrowser);

        DriverContext.Start();
    }

    /// <summary>
    ///     After the class.
    /// </summary>
    public void Dispose()
    {
        DriverContext.Stop();
        GC.SuppressFinalize(this);
    }
}
