using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text;
using static Lsquared.Extensions.Enums;

namespace Lsquared.Extensions
{
    /// <summary>
    /// Represents a set of enum values.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum.</typeparam>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public struct Set<TEnum> : IReadOnlyCollection<TEnum>, IEnumerable<TEnum>, IEnumerable
        where TEnum : struct, IConvertible
    {
        /// <summary>
        /// Represents the empty string.
        /// </summary>
        /// <remarks>
        /// This field is read-only.
        /// </remarks>
        public static readonly Set<TEnum> Empty = new Set<TEnum>();

        /// <summary>
        /// Gets a value indicating whether this set is empty (i.e. contains no values).
        /// </summary>
        /// <value>
        ///   <c>true</c> if this set is empty; otherwise, <c>false</c>.
        /// </value>
        [Pure]
        public bool IsEmpty => _bits == 0;

        /// <inheritdoc />
        [Pure]
        public int Count => _count;

        private string DebuggerDisplay => $"Set of {typeof(TEnum).Name} = {ToString()}";

        /// <summary>
        /// Initializes the <see cref="Set{TEnum}"/> struct.
        /// </summary>
        /// <exception cref="System.NotSupportedException">Do not support enum with more than 32 values</exception>
        static Set()
        {
            if (EnumHelper<TEnum>.Count > 32)
                throw new NotSupportedException("Do not support enum with more than 32 values");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Set{TEnum}"/> struct.
        /// </summary>
        /// <param name="value">One enum value.</param>
        public Set(TEnum value)
        {
            _bits = 0;

            var ord = Ordinal(value);
            _bits |= (1 << (ord % 32));

            _count = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Set{TEnum}"/> struct.
        /// </summary>
        /// <param name="min">The minimum enum's value.</param>
        /// <param name="max">The maximum enum's value.</param>
        public Set(TEnum min, TEnum max)
        {
            _bits = 0;

            var lower = Ordinal(min);
            var upper = Ordinal(max);

            var c = 0;
            for (var ord = lower ; ord <= upper ; ord++)
            {
                _bits |= (1 << (ord % 32));
                c++;
            }

            _count = c;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Set{TEnum}"/> struct.
        /// </summary>
        /// <param name="values">An array of enum's values.</param>
        public Set(params TEnum[] values)
        {
            _bits = 0;

            foreach (var value in values)
                _bits |= (1 << (Ordinal(value) % 32));

            _count = values.Length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Set{TEnum}"/> struct.
        /// </summary>
        /// <param name="bits">The bits.</param>
        private Set(int bits, int count)
        {
            _bits = bits;
            _count = count;
        }

        /// <summary>
        /// Includes the specified enum's value.
        /// </summary>
        /// <param name="value">The value to include.</param>
        /// <returns>
        /// An instance of <see cref="Set{TEnum}"/> containing all values of the current set including the specified value.
        /// </returns>
        [Pure]
        public Set<TEnum> Include(TEnum value)
        {
            var ord = Ordinal(value);
            return new Set<TEnum>(_bits | (1 << (ord % 32)), _count + 1);
        }

        /// <summary>
        /// Includes the specified enum's values.
        /// </summary>
        /// <param name="values">The values to include.</param>
        /// <returns>
        /// An instance of <see cref="Set{TEnum}"/> containing all values of the current set including all specified values.
        /// </returns>
        [Pure]
        public Set<TEnum> Include(params TEnum[] values)
        {
            var bits = _bits;
            var count = _count;
            foreach (var value in values)
            {
                var ord = Ordinal(value);
                bits |= 1 << (ord % 32);
                ++count;
            }
            return new Set<TEnum>(bits, count);
        }

        /// <summary>
        /// Includes the specified <see cref="Set{TEnum}"/>.
        /// </summary>
        /// <param name="other">The values to include.</param>
        /// <returns>
        /// An instance of <see cref="Set{TEnum}"/> containing all values of the current set including all values of the specified set.
        /// </returns>
        [Pure]
        public Set<TEnum> Include(Set<TEnum> other)
        {
            return new Set<TEnum>(_bits | other._bits, _count + other._count);
        }

        /// <summary>
        /// Excludes the specified enum's value.
        /// </summary>
        /// <param name="value">The value to exclude.</param>
        /// <returns>
        /// An instance of <see cref="Set{TEnum}"/> containing all values of the current set excluding the specified value.
        /// </returns>
        [Pure]
        public Set<TEnum> Exclude(TEnum value)
        {
            var ord = Ordinal(value);
            return new Set<TEnum>(_bits & ~(1 << (ord % 32)), _count - 1);
        }

        /// <summary>
        /// Excludes the specified enum's values.
        /// </summary>
        /// <param name="values">The values to exclude.</param>
        /// <returns>
        /// An instance of <see cref="Set{TEnum}"/> containing all values of the current set excluding all specified values.
        /// </returns>
        [Pure]
        public Set<TEnum> Exclude(params TEnum[] values)
        {
            var bits = _bits;
            var count = _count;
            foreach (var value in values)
            {
                var ord = Ordinal(value);
                bits &= ~(1 << (ord % 32));
                --count;
            }
            return new Set<TEnum>(bits, count);
        }

        /// <summary>
        /// Excludes the specified <see cref="Set{TEnum}"/>.
        /// </summary>
        /// <param name="other">The values to exclude.</param>
        /// <returns>
        /// An instance of <see cref="Set{TEnum}"/> containing all values of the current set excluding all values of the specified set.
        /// </returns>
        [Pure]
        public Set<TEnum> Exclude(Set<TEnum> other)
        {
            return new Set<TEnum>(_bits & ~other._bits, _count - other._count);
        }

        /// <summary>
        /// Determines whether the specified enum's value is in the current set.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if enum's value is found in the current set; otherwise, <c>false</c>.
        /// </returns>
        [Pure]
        public bool Contains(TEnum value)
        {
            var ord = Ordinal(value);
            return IsDefined(ord);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        [Pure]
        public override string ToString()
        {
            if (_bits == 0)
                return EmptyStr;

            var b = new StringBuilder(50);
            b.Append("[");
            for (int ord = 0 ; ord < EnumLength ; ord++)
            {
                if (IsDefined(ord))
                {
                    b.Append(EnumHelper<TEnum>.GetName(ord));
                    b.Append(", ");
                }
            }
            b.Length -= 2;
            b.Append("]");
            return b.ToString();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        [Pure]
        public IEnumerator<TEnum> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        [Pure]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="TEnum[]"/> to <see cref="Set{TEnum}"/>.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator Set<TEnum>(TEnum[] values)
            => new Set<TEnum>(values);

        /// <summary>
        /// Performs an explicit conversion from <see cref="TEnum"/> to <see cref="Set{TEnum}"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator Set<TEnum>(TEnum value)
            => new Set<TEnum>(value);

        #region Helpers

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsDefined(int index)
        {
            return (_bits & (1 << (index % 32))) != 0;
        }

        #endregion

        #region Fields

        private const string EmptyStr = "[]";
        private static readonly int EnumLength = EnumHelper<TEnum>.Count;
        private readonly int _count;
        private readonly int _bits;

        #endregion

        #region Nested

        internal class Enumerator : IEnumerator<TEnum>, IEnumerator
        {
            public TEnum Current
                 => EnumHelper<TEnum>.GetObject(_position);

            object IEnumerator.Current
                => EnumHelper<TEnum>.GetObject(_position);

            public Enumerator(Set<TEnum> set)
            {
                _set = set;
            }

            public void Dispose()
            {
                // Method intentionally left empty.
            }

            public bool MoveNext()
            {
                return ++_position < _count;
            }

            public void Reset()
            {
                _position = 0;
            }

            #region Fields

            private readonly int _count = EnumHelper<TEnum>.Count;
            private readonly Set<TEnum> _set;
            private int _position;

            #endregion
        }

        #endregion
    }
}
