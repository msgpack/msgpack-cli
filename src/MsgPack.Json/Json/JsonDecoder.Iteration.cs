// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System.Buffers;
using System.Diagnostics;
using MsgPack.Internal;

namespace MsgPack.Json
{
	public partial class JsonDecoder
	{
		private sealed class ArrayIterator
		{
			private const byte ArrayStartToken = (byte)'[';
			private const byte ArrayEndToken = (byte)']';
			private const byte ItemDelimiterToken = (byte)',';

			private readonly JsonDecoder _decoder;
			private State _state;

			public ArrayIterator(JsonDecoder decoder)
			{
				this._decoder = decoder;
				this._state = State.Head;
			}

			public bool DetectArrayEnds(in SequenceReader<byte> source, ref long nextItemIndex, long itemsCount, out int requestHint)
			{
				if (this._state == State.Tail)
				{
					requestHint = 0;
					return true; // End
				}

				this._decoder.ReadTrivia(source, out _);

				switch (this._state)
				{
					case State.Head:
					{
						if (!source.TryPeek(out var shouldBeArrayStart))
						{
							requestHint = 2;
							return false;
						}

						if (shouldBeArrayStart != ArrayStartToken)
						{
							// Invalid
							JsonThrow.UnexpectedToken(source.Consumed, shouldBeArrayStart);
						}

						source.Advance(1);
						break;
					} // case State.Head
					default:
					{
						Debug.Assert(this._state == State.Item, $"this._state {(this._state)} == State.Item");

						if (!source.TryPeek(out var mayBeDelimiterOrArrayEnd))
						{
							requestHint = 1;
							return false;
						}

						switch (mayBeDelimiterOrArrayEnd)
						{
							case ArrayEndToken:
							{
								requestHint = 0;
								source.Advance(1);
								this._state = State.Tail;
								return true;
							} // case ArrayEndToken:
							case ItemDelimiterToken:
							{
								source.Advance(1);
								// goto maybe-item
								break;
							} // ItemDelimiterToken
							default:
							{
								JsonThrow.UnexpectedToken(source.Consumed, mayBeDelimiterOrArrayEnd);
								// never
								break;
							}
						} // switch (mayBeDelimiterOrArrayEnd)

						break;
					} // default:
				} // swtich (this._state)

				// Handle 'maybe item' state
				this._decoder.ReadTrivia(source, out _);
				if (!source.TryPeek(out var mayBeArrayEnd))
				{
					requestHint = 1;
					return false;
				}

				requestHint = 0;

				if (mayBeArrayEnd == ArrayEndToken)
				{
					source.Advance(1);
					this._state = State.Tail;
					return true;
				}
				else
				{
					this._state = State.Item;
					return false;
				}
			}

			private enum State
			{
				Head = 0,
				Item,
				Tail
			}
		}

		private sealed class ObjectPropertyIterator
		{
			private const byte ObjectStartToken = (byte)'{';
			private const byte ObjectEndToken = (byte)'}';
			private const byte KeyValueSepratorToken = (byte)':';
			private const byte AltKeyValueSepratorToken = (byte)'=';
			private const byte ItemDelimiterToken = (byte)',';

			private readonly JsonDecoder _decoder;
			private readonly bool _isEqualSignAllowed;
			private State _state;

			public ObjectPropertyIterator(JsonDecoder decoder)
			{
				this._decoder = decoder;
				this._state = State.Head;
				this._isEqualSignAllowed = (decoder.Options.ParseOptions & JsonParseOptions.AllowEqualSignSeparator) != 0;
			}

			public bool DetectObjectEnds(in SequenceReader<byte> source, ref long nextItemIndex, long itemsCount, out int requestHint)
			{
				if (this._state == State.Tail)
				{
					requestHint = 0;
					return true; // End
				}

				this._decoder.ReadTrivia(source, out _);

				switch (this._state)
				{
					case State.Head:
					{
						if (!source.TryPeek(out var shouldBeObjectStart))
						{
							requestHint = 2;
							return false;
						}

						if (shouldBeObjectStart != ObjectStartToken)
						{
							// Invalid
							JsonThrow.UnexpectedToken(source.Consumed, shouldBeObjectStart);
						}

						source.Advance(1);
						break;
					} // case State.Head
					case State.Key:
					{
						if (!source.TryPeek(out var mayBeSeparator))
						{
							requestHint = 3;
							return false;
						}

						switch (mayBeSeparator)
						{
							case AltKeyValueSepratorToken:
							{
								if (this._isEqualSignAllowed)
								{
									goto case KeyValueSepratorToken;
								}
								else
								{
									goto default;
								}
							} // case AltKeyValueSepratorToken
							case KeyValueSepratorToken:
							{
								source.Advance(1);
								this._state = State.Value;
								requestHint = 0;
								return false;
							} // case KeyValueSepratorToken
							default:
							{
								JsonThrow.UnexpectedToken(source.Consumed, mayBeSeparator);
								// never
								requestHint = default;
								return false;
							}
						} // switch (mayBeSeparator)

					} // case State.Key
					default:
					{
						Debug.Assert(this._state == State.Value, $"this._state {(this._state)} == State.Value");

						if (!source.TryPeek(out var mayBeDelimiterOrObjectEnd))
						{
							requestHint = 1;
							return false;
						}

						switch (mayBeDelimiterOrObjectEnd)
						{
							case ObjectEndToken:
							{
								source.Advance(1);
								this._state = State.Tail;
								requestHint = 0;
								return true;
							} // case ObjectEndToken
							case ItemDelimiterToken:
							{
								source.Advance(1);
								// goto maybe-key
								break;
							} // case ItemDelimiterToken
							default:
							{
								JsonThrow.UnexpectedToken(source.Consumed, mayBeDelimiterOrObjectEnd);
								// never
								break;
							}
						} // switch (mayBeDelimiterOrArrayEnd)

						break;
					} // default:
				} // swtich (this._state)

				// Handle 'maybe key' state
				this._decoder.ReadTrivia(source, out _);
				if (!source.TryPeek(out var mayBeArrayEnd))
				{
					requestHint = 1;
					return false;
				}

				requestHint = 0;

				if (mayBeArrayEnd == ObjectEndToken)
				{
					source.Advance(1);
					this._state = State.Tail;
					return true;
				}
				else
				{
					this._state = State.Key;
					return false;
				}
			}

			private enum State
			{
				Head = 0,
				Key,
				Value,
				Tail
			}
		}

		public override CollectionType DecodeArrayOrMap(in SequenceReader<byte> source, out CollectionItemIterator iterator, out int requestHint)
		{
			this.ReadTrivia(source, out _);

			if (!this.TryPeek(source, out var token))
			{
				requestHint = 1;
				iterator = default;
				return CollectionType.None;
			}

			requestHint = 0;
			switch (token)
			{
				case (byte)'[':
				{
					iterator = this.CreateArrayIterator();
					return CollectionType.Array;
				}
				case (byte)'{':
				{
					iterator = this.CreateObjectPropertyIterator();
					return CollectionType.Map;
				}
				default:
				{
					var offset = source.Consumed;
					var kind = TryGetUtf8Unit(source, out var sequence);
					if (kind == Utf8UnitStatus.Valid)
					{
						JsonThrow.IsNotArrayNorObject(sequence, offset);
					}
					else
					{
						JsonThrow.MalformedUtf8(sequence, offset);
					}

					// never
					iterator = default;
					return default;
				}
			}
		}

		private CollectionItemIterator CreateArrayIterator()
			=> new CollectionItemIterator(new ArrayIterator(this).DetectArrayEnds, -1);

		private CollectionItemIterator CreateObjectPropertyIterator()
			=> new CollectionItemIterator(new ObjectPropertyIterator(this).DetectObjectEnds, -1);

		public override CollectionItemIterator DecodeArray(in SequenceReader<byte> source, out int requestHint)
		{
			var type = this.DecodeArrayOrMap(source, out var iterator, out requestHint);
			if (requestHint != 0)
			{
				return default;
			}

			if (!type.IsArray)
			{
				JsonThrow.IsNotArray(source.Consumed);
			}

			return iterator;
		}

		public override CollectionItemIterator DecodeMap(in SequenceReader<byte> source, out int requestHint)
		{
			var type = this.DecodeArrayOrMap(source, out var iterator, out requestHint);
			if (requestHint != 0)
			{
				return default;
			}

			if (!type.IsMap)
			{
				JsonThrow.IsNotObject(source.Consumed);
			}

			return iterator;
		}
	}
}
