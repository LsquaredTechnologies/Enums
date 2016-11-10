using System;
using System.Reflection;

namespace Lsquared.Extensions
{
    /// <summary>
    /// Contains methods to manipulate enums.
    /// </summary>
    /// <remarks>
    /// Can be used with <c>using static Lsquared.Extensions.<see cref="Enums"/>.</c>
    /// </remarks>
    public static class Enums
    {
        /// <summary>
        /// Gets the name of the specified enum's value.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="value">The value of the enum.</param>
        /// <returns>
        /// An instance of <see cref="string"/> representing the name of the enum's value.
        /// </returns>
        /// <exception cref="System.ArgumentException">Parameter is not an Enum type - value</exception>
        public static string Name<TEnum>(TEnum value) where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).GetTypeInfo().IsEnum) throw new ArgumentException("Parameter is not an Enum type", nameof(value));
            return EnumHelper<TEnum>.GetName(value);
        }

        /// <summary>
        /// Gets the ordinal position of the specified enum's value.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="value">The value of the enum.</param>
        /// <returns>
        /// An integer representing the ordinal position of the enum's value.
        /// </returns>
        /// <exception cref="System.ArgumentException">Parameter is not an Enum type - value</exception>
        public static int Ordinal<TEnum>(TEnum value) where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).GetTypeInfo().IsEnum) throw new ArgumentException("Parameter is not an Enum type", nameof(value));
            return EnumHelper<TEnum>.GetOrdinal(value);
        }

        /// <summary>
        /// Gets the integer representation of the specified enum's value.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="value">The value of the enum.</param>
        /// <returns>
        /// An integer representing the value of the enum's value.
        /// </returns>
        /// <exception cref="System.ArgumentException">Parameter is not an Enum type - value</exception>
        public static int Value<TEnum>(TEnum value) where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).GetTypeInfo().IsEnum) throw new ArgumentException("Parameter is not an Enum type", nameof(value));
            return EnumHelper<TEnum>.GetValue(value);
        }

        /// <summary>
        /// Gets the predecessor value of the specified enum's value.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="value">The value of the enum which next value is searched.</param>
        /// <returns>
        /// An instance of <typeparamref name="TEnum"/> representing the predecessor value of the enum's value.
        /// </returns>
        /// <exception cref="System.ArgumentException">Parameter is not an Enum type - value</exception>
        public static TEnum Pred<TEnum>(TEnum value) where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).GetTypeInfo().IsEnum) throw new ArgumentException("Parameter is not an Enum type", nameof(value));
            var ord = EnumHelper<TEnum>.GetOrdinal(value);
            return EnumHelper<TEnum>.GetObject(ord - 1);
        }

        /// <summary>
        /// Gets the successor value of the specified enum's value.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="value">The value of the enum which next value is searched.</param>
        /// <returns>
        /// An instance of <typeparamref name="TEnum"/> representing the successor value of the enum's value.
        /// </returns>
        /// <exception cref="System.ArgumentException">Parameter is not an Enum type - value</exception>
        public static TEnum Succ<TEnum>(TEnum value) where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).GetTypeInfo().IsEnum) throw new ArgumentException("Parameter is not an Enum type", nameof(value));
            var ord = EnumHelper<TEnum>.GetOrdinal(value);
            return EnumHelper<TEnum>.GetObject(ord + 1);
        }

        public static int Len<TEnum>() where TEnum : struct, IConvertible
        {
            return EnumHelper<TEnum>.Count;
        }
    }
}