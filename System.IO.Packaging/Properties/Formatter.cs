// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.IO.Packaging.Properties
{
    internal static class Formatter
    {
        internal static string Format(string resourceFormat, object p1)
        {
            return String.Format(resourceFormat, p1);
        }

        internal static string Format(string resourceFormat, object p1, object p2)
        {
            return String.Format(resourceFormat, p1, p2);
        }
    }
}
