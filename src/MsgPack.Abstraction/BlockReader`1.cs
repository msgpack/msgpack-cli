// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Internal.Runtime.CompilerServices;

namespace System.Buffers
{
    public ref partial struct SequenceReader<T> where T : unmanaged, IEquatable<T>
    {
        private SequencePosition _currentPosition;
        private SequencePosition _nextPosition;
        private bool _moreData;
        private readonly long _length;

        /// <summary>
        /// Create a <see cref="SequenceReader{T}"/> over the given <see cref="ReadOnlySequence{T}"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SequenceReader(ReadOnlySequence<T> sequence)
        {
            this.CurrentSpanIndex = 0;
            this.Consumed = 0;
            this.Sequence = sequence;
            this._currentPosition = sequence.Start;
            this._length = -1;

            sequence.GetFirstSpan(out ReadOnlySpan<T> first, out this._nextPosition);
            this.CurrentSpan = first;
            this._moreData = first.Length > 0;

            if (!this._moreData && !sequence.IsSingleSegment)
            {
                this._moreData = true;
                this.GetNextSpan();
            }
        }

        /// <summary>
        /// True when there is no more data in the <see cref="Sequence"/>.
        /// </summary>
        public readonly bool End => !this._moreData;

        /// <summary>
        /// The underlying <see cref="ReadOnlySequence{T}"/> for the reader.
        /// </summary>
        public readonly ReadOnlySequence<T> Sequence { get; }

        /// <summary>
        /// Gets the unread portion of the <see cref="Sequence"/>.
        /// </summary>
        /// <value>
        /// The unread portion of the <see cref="Sequence"/>.
        /// </value>
        public readonly ReadOnlySequence<T> UnreadSequence => this.Sequence.Slice(this.Position);

        /// <summary>
        /// The current position in the <see cref="Sequence"/>.
        /// </summary>
        public readonly SequencePosition Position
            => this.Sequence.GetPosition(this.CurrentSpanIndex, this._currentPosition);

        /// <summary>
        /// The current segment in the <see cref="Sequence"/> as a span.
        /// </summary>
        public ReadOnlySpan<T> CurrentSpan { readonly get; private set; }

        /// <summary>
        /// The index in the <see cref="CurrentSpan"/>.
        /// </summary>
        public int CurrentSpanIndex { readonly get; private set; }

        /// <summary>
        /// The unread portion of the <see cref="CurrentSpan"/>.
        /// </summary>
        public readonly ReadOnlySpan<T> UnreadSpan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.CurrentSpan.Slice(this.CurrentSpanIndex);
        }

        /// <summary>
        /// The total number of <typeparamref name="T"/>'s processed by the reader.
        /// </summary>
        public long Consumed { readonly get; private set; }

        /// <summary>
        /// Remaining <typeparamref name="T"/>'s in the reader's <see cref="Sequence"/>.
        /// </summary>
        public readonly long Remaining => this.Length - this.Consumed;

        /// <summary>
        /// Count of <typeparamref name="T"/> in the reader's <see cref="Sequence"/>.
        /// </summary>
        public readonly long Length
        {
            get
            {
                if (this._length < 0)
                {
                    // Cast-away readonly to initialize lazy field
                    Volatile.Write(ref Unsafe.AsRef(this._length), this.Sequence.Length);
                }
                return this._length;
            }
        }

        /// <summary>
        /// Peeks at the next value without advancing the reader.
        /// </summary>
        /// <param name="value">The next value or default if at the end.</param>
        /// <returns>False if at the end of the reader.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool TryPeek(out T value)
        {
            if (this._moreData)
            {
                value = this.CurrentSpan[this.CurrentSpanIndex];
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        /// <summary>
        /// Peeks at the next value at specific offset without advancing the reader.
        /// </summary>
        /// <param name="offset">The offset from current position.</param>
        /// <param name="value">The next value, or the default value if at the end of the reader.</param>
        /// <returns><c>true</c> if the reader is not at its end and the peek operation succeeded; <c>false</c> if at the end of the reader.</returns>
        public readonly bool TryPeek(long offset, out T value)
        {
            if (offset < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException_OffsetOutOfRange();

            // If we've got data and offset is not out of bounds
            if (!this._moreData || this.Remaining <= offset)
            {
                value = default;
                return false;
            }

            // Sum CurrentSpanIndex + offset could overflow as is but the value of offset should be very large
            // because we check Remaining <= offset above so to overflow we should have a ReadOnlySequence close to 8 exabytes
            Debug.Assert(this.CurrentSpanIndex + offset >= 0);

            // If offset doesn't fall inside current segment move to next until we find correct one
            if ((this.CurrentSpanIndex + offset) <= this.CurrentSpan.Length - 1)
            {
                Debug.Assert(offset <= int.MaxValue);

                value = this.CurrentSpan[this.CurrentSpanIndex + (int)offset];
                return true;
            }
            else
            {
                long remainingOffset = offset - (this.CurrentSpan.Length - this.CurrentSpanIndex);
                SequencePosition nextPosition = this._nextPosition;
                ReadOnlyMemory<T> currentMemory = default;

                while (this.Sequence.TryGet(ref nextPosition, out currentMemory, advance: true))
                {
                    // Skip empty segment
                    if (currentMemory.Length > 0)
                    {
                        if (remainingOffset >= currentMemory.Length)
                        {
                            // Subtract current non consumed data
                            remainingOffset -= currentMemory.Length;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                value = currentMemory.Span[(int)remainingOffset];
                return true;
            }
        }

        /// <summary>
        /// Read the next value and advance the reader.
        /// </summary>
        /// <param name="value">The next value or default if at the end.</param>
        /// <returns>False if at the end of the reader.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryRead(out T value)
        {
            if (this.End)
            {
                value = default;
                return false;
            }

            value = this.CurrentSpan[this.CurrentSpanIndex];
            this.CurrentSpanIndex++;
            this.Consumed++;

            if (this.CurrentSpanIndex >= this.CurrentSpan.Length)
            {
                this.GetNextSpan();
            }

            return true;
        }

        /// <summary>
        /// Move the reader back the specified number of items.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if trying to rewind a negative amount or more than <see cref="Consumed"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Rewind(long count)
        {
            if ((ulong)count > (ulong)this.Consumed)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count);
            }

            this.Consumed -= count;

            if (this.CurrentSpanIndex >= count)
            {
                this.CurrentSpanIndex -= (int)count;
                this._moreData = true;
            }
            else
            {
                // Current segment doesn't have enough data, scan backward through segments
                this.RetreatToPreviousSpan(Consumed);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void RetreatToPreviousSpan(long consumed)
        {
            this.ResetReader();
            this.Advance(consumed);
        }

        private void ResetReader()
        {
            this.CurrentSpanIndex = 0;
            this.Consumed = 0;
            this._currentPosition = this.Sequence.Start;
            this._nextPosition = this._currentPosition;

            if (this.Sequence.TryGet(ref this._nextPosition, out ReadOnlyMemory<T> memory, advance: true))
            {
                this._moreData = true;

                if (memory.Length == 0)
                {
                    this.CurrentSpan = default;
                    // No data in the first span, move to one with data
                    this.GetNextSpan();
                }
                else
                {
                    this.CurrentSpan = memory.Span;
                }
            }
            else
            {
                // No data in any spans and at end of sequence
                this._moreData = false;
                this.CurrentSpan = default;
            }
        }

        /// <summary>
        /// Get the next segment with available data, if any.
        /// </summary>
        private void GetNextSpan()
        {
            if (!this.Sequence.IsSingleSegment)
            {
                SequencePosition previousNextPosition = this._nextPosition;
                while (this.Sequence.TryGet(ref this._nextPosition, out ReadOnlyMemory<T> memory, advance: true))
                {
                    this._currentPosition = previousNextPosition;
                    if (memory.Length > 0)
                    {
                        this.CurrentSpan = memory.Span;
                        this.CurrentSpanIndex = 0;
                        return;
                    }
                    else
                    {
                        this.CurrentSpan = default;
                        this.CurrentSpanIndex = 0;
                        previousNextPosition = this._nextPosition;
                    }
                }
            }
            this._moreData = false;
        }

        /// <summary>
        /// Move the reader ahead the specified number of items.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Advance(long count)
        {
            const long TooBigOrNegative = unchecked((long)0xFFFFFFFF80000000);
            if ((count & TooBigOrNegative) == 0 && this.CurrentSpan.Length - this.CurrentSpanIndex > (int)count)
            {
                this.CurrentSpanIndex += (int)count;
                this.Consumed += count;
            }
            else
            {
                // Can't satisfy from the current span
                this.AdvanceToNextSpan(count);
            }
        }

        /// <summary>
        /// Unchecked helper to avoid unnecessary checks where you know count is valid.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void AdvanceCurrentSpan(long count)
        {
            Debug.Assert(count >= 0);

            this.Consumed += count;
            this.CurrentSpanIndex += (int)count;
            if (this.CurrentSpanIndex >= this.CurrentSpan.Length)
                this.GetNextSpan();
        }

        /// <summary>
        /// Only call this helper if you know that you are advancing in the current span
        /// with valid count and there is no need to fetch the next one.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void AdvanceWithinSpan(long count)
        {
            Debug.Assert(count >= 0);

            this.Consumed += count;
            this.CurrentSpanIndex += (int)count;

            Debug.Assert(this.CurrentSpanIndex < this.CurrentSpan.Length);
        }

        private void AdvanceToNextSpan(long count)
        {
            if (count < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count);
            }

            this.Consumed += count;
            while (this._moreData)
            {
                int remaining = this.CurrentSpan.Length - this.CurrentSpanIndex;

                if (remaining > count)
                {
                    this.CurrentSpanIndex += (int)count;
                    count = 0;
                    break;
                }

                // As there may not be any further segments we need to
                // push the current index to the end of the span.
                this.CurrentSpanIndex += remaining;
                count -= remaining;
                Debug.Assert(count >= 0);

                this.GetNextSpan();

                if (count == 0)
                {
                    break;
                }
            }

            if (count != 0)
            {
                // Not enough data left- adjust for where we actually ended and throw
                this.Consumed -= count;
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count);
            }
        }

        /// <summary>
        /// Copies data from the current <see cref="Position"/> to the given <paramref name="destination"/> span if there
        /// is enough data to fill it.
        /// </summary>
        /// <remarks>
        /// This API is used to copy a fixed amount of data out of the sequence if possible. It does not advance
        /// the reader. To look ahead for a specific stream of data <see cref="IsNext(ReadOnlySpan{T}, bool)"/> can be used.
        /// </remarks>
        /// <param name="destination">Destination span to copy to.</param>
        /// <returns>True if there is enough data to completely fill the <paramref name="destination"/> span.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool TryCopyTo(Span<T> destination)
        {
            // This API doesn't advance to facilitate conditional advancement based on the data returned.
            // We don't provide an advance option to allow easier utilizing of stack allocated destination spans.
            // (Because we can make this method readonly we can guarantee that we won't capture the span.)

            ReadOnlySpan<T> firstSpan = this.UnreadSpan;
            if (firstSpan.Length >= destination.Length)
            {
                firstSpan.Slice(0, destination.Length).CopyTo(destination);
                return true;
            }

            // Not enough in the current span to satisfy the request, fall through to the slow path
            return this.TryCopyMultisegment(destination);
        }

        internal readonly bool TryCopyMultisegment(Span<T> destination)
        {
            // If we don't have enough to fill the requested buffer, return false
            if (this.Remaining < destination.Length)
                return false;

            ReadOnlySpan<T> firstSpan = this.UnreadSpan;
            Debug.Assert(firstSpan.Length < destination.Length);
            firstSpan.CopyTo(destination);
            int copied = firstSpan.Length;

            SequencePosition next = this._nextPosition;
            while (this.Sequence.TryGet(ref next, out ReadOnlyMemory<T> nextSegment, true))
            {
                if (nextSegment.Length > 0)
                {
                    ReadOnlySpan<T> nextSpan = nextSegment.Span;
                    int toCopy = Math.Min(nextSpan.Length, destination.Length - copied);
                    nextSpan.Slice(0, toCopy).CopyTo(destination.Slice(copied));
                    copied += toCopy;
                    if (copied >= destination.Length)
                    {
                        break;
                    }
                }
            }

            return true;
        }
    }
}