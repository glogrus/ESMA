// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Native.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Runtime.InteropServices;

    /// <summary>
    ///     The native functions.
    /// </summary>
    [SuppressMessage("ReSharper", "StyleCop.SA1300", Justification = "Native functions.")]
    internal static class Native
    {
 #if NETCOREAPP
        static Native()
        {
            NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), DllImportResolver);
        }

#endif

        [DllImport("msvcrt", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int memcmp(byte* buffer1, byte* buffer2, int count);

#if NETCOREAPP
        private static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            var platformDependentName = libraryName;

            if (libraryName == "msvcrt" && !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                platformDependentName = "libc";
            }

            NativeLibrary.TryLoad(platformDependentName, assembly, searchPath, out var handle);
            return handle;
        }

#endif
    }
}