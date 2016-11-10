using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Lsquared.Extensions
{
    /// <summary>
    /// Provides enum helpers.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public static class EnumHelper<TEnum> where TEnum : struct, IConvertible
    {
        /// <summary>
        /// Gets the number of values defined in the enum.
        /// </summary>
        public static int Count => _entries.Count;

        /// <summary>
        /// 
        /// </summary>
        static EnumHelper()
        {
#if DEBUG
            var s = Stopwatch.StartNew();
#endif
            var enumType = typeof(TEnum);
            if (!enumType.GetTypeInfo().IsEnum) throw new ArgumentException("Generic parameter is not an Enum type", nameof(TEnum));
            var names = Enum.GetNames(enumType);
            var values = Enum.GetValues(enumType).Cast<TEnum>().ToArray();
            for (int i = 0; i < names.Length; i++)
            {
                _entries.Add(values[i], new Entry { Ordinal = i, Object = values[i], Name = names[i] });
            }
#if DEBUG
            s.Stop();
            Debug.WriteLine($"Preparation for {enumType.Name} took {s.ElapsedMilliseconds}ms");
#endif
        }

        /// <summary>
        /// Gets the ordinal position of the specified enum's value.
        /// </summary>
        /// <param name="value">The value of the enum.</param>
        /// <returns>
        /// An integer representing the ordinal position of the enum's value.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetOrdinal(TEnum value)
        {
            return _entries[value].Ordinal;
        }

        /// <summary>
        /// Gets the name of the specified enum's value.
        /// </summary>
        /// <param name="value">The value of the enum.</param>
        /// <returns>
        /// An instance of <see cref="string"/> representing the name of the enum's value.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetName(TEnum value)
        {
            return _entries[value].Name;
        }

        /// <summary>
        /// Gets the integer value of the specified enum's value.
        /// </summary>
        /// <param name="value">The value of the enum.</param>
        /// <returns>
        /// An integer representing the value of the enum's value.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetValue(TEnum value)
        {
            return value.ToInt32(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets all values of a <typeparamref name="TEnum"/>.
        /// </summary>
        /// <returns>
        /// An array of all enum's values.
        /// </returns>
        public static TEnum[] GetValues()
        {
            return _entries.Keys.ToArray();
        }

        /// <summary>
        /// Gets the name of the specified enum's value.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns>
        /// An instance of <see cref="string"/> representing the enum's name.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">ordinal</exception>
        internal static string GetName(int ordinal)
        {
            foreach (var entry in _entries)
                if (entry.Value.Ordinal == ordinal)
                    return entry.Value.Name;

            throw new ArgumentOutOfRangeException(nameof(ordinal));
        }

        /// <summary>
        /// Gets the object for the specified ordinal position.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns>
        /// An instance of <typeparamref name="TEnum"/> representing the enum's value at the specified ordinal position.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="ordinal"/> is not in a valid range.</exception>
        internal static TEnum GetObject(int ordinal)
        {
            foreach (var entry in _entries)
                if (entry.Value.Ordinal == ordinal)
                    return entry.Value.Object;

            throw new ArgumentOutOfRangeException(nameof(ordinal));
        }

        #region Fields

        private static readonly Dictionary<TEnum, Entry> _entries = new Dictionary<TEnum, Entry>();

        #endregion

        #region Nested

        private struct Entry
        {
            public int Ordinal;
            public int Value;
            public TEnum Object;
            public string Name;
        }

        #endregion
    }
}