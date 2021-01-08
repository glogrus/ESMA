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
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    ///     The native.
    /// </summary>
    internal static class Native
    {
#if NETCOREAPP
        /// <summary>
        /// Initializes static members of the Native class.
        /// </summary>
        static Native()
        {
            NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), DllImportResolver);
        }
#endif

        /// <summary>
        /// The memcmp native method.
        /// </summary>
        /// <param name="buffer1">
        /// The buffer 1.
        /// </param>
        /// <param name="buffer2">
        /// The buffer 2.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DllImport("msvcrt", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int memcmp(byte* buffer1, byte* buffer2, int count);

#if NETCOREAPP
        /// <summary>
        /// The dll import resolver.
        /// </summary>
        /// <param name="libraryName">
        /// The library name.
        /// </param>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        /// <param name="searchPath">
        /// The search path.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
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