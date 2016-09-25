#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
#endregion -- License Terms --

using System;
using System.Collections.Generic;
#if CORE_CLR || NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR || NETSTANDARD1_1
using System.Globalization;
using System.Linq;

namespace MsgPack.Serialization.AbstractSerializers
{
	/// <summary>
	///		Defines common interfaces and features for context objects for serializer generation.
	/// </summary>
	/// <typeparam name="TConstruct">The contextType of the code construct for serializer builder.</typeparam>
	internal abstract class SerializerGenerationContext<TConstruct>
	{
		/// <summary>
		///		Gets the code construct which represents 'context' parameter of generated methods.
		/// </summary>
		/// <value>
		///		The code construct which represents 'context' parameter of generated methods.
		///		Its type is <see cref="SerializationContext"/>, and it holds dependent serializers.
		///		This value will not be <c>null</c>.
		/// </value>
		public virtual TConstruct Context
		{
			get { throw new NotSupportedException(); }
		}

		/// <summary>
		///		Gets the serialization context which holds various serialization configuration.
		/// </summary>
		/// <value>
		///		The serialization context. This value will not be <c>null</c>.
		/// </value>
		public SerializationContext SerializationContext { get; private set; }

		/// <summary>
		///		Gets the code construct which represents the argument for the packer.
		/// </summary>
		/// <value>
		///		The code construct which represents the argument for the packer.
		///		This value will not be <c>null</c>.
		/// </value>
		public TConstruct Packer { get; protected set; }

		/// <summary>
		///		Gets the code construct which represents the argument for the packing target object tree root.
		/// </summary>
		/// <returns>
		///		The code construct which represents the argument for the packing target object tree root.
		///		This value will not be <c>null</c>.
		/// </returns>
		public TConstruct PackToTarget { get; protected set; }

		/// <summary>
		///		Gets the code construct which represents the argument for the single argument for null checking.
		/// </summary>
		/// <returns>
		///		The code construct which represents the argument for the single argument for null checking.
		///		This value will not be <c>null</c>.
		/// </returns>
		public TConstruct NullCheckTarget { get; protected set; }

		/// <summary>
		///		Gets the code construct which represents the argument for the unpacker.
		/// </summary>
		/// <value>
		///		The code construct which represents the argument for the unpacker.
		///		This value will not be <c>null</c>.
		/// </value>
		public TConstruct Unpacker { get; protected set; }

		/// <summary>
		///		Gets the code construct which represents the argument for the collection which will hold unpacked items.
		/// </summary>
		/// <returns>
		///		The code construct which represents the argument for the collection which will hold unpacked items.
		///		This value will not be <c>null</c>.
		/// </returns>
		public TConstruct UnpackToTarget { get; protected set; }

		/// <summary>
		///		Gets the code construct which represents the argument for the collection which will be added new unpacked item.
		/// </summary>
		/// <returns>
		///		The code construct which represents the argument for the collection which will be added new unpacked item.
		///		This value will not be <c>null</c>.
		/// </returns>
		public TConstruct CollectionToBeAdded { get; protected set; }

		/// <summary>
		///		Gets the code construct which represents the argument for the item to be added to the collection.
		/// </summary>
		/// <returns>
		///		The code construct which represents the argument for the item to be added to the collection.
		///		This value will not be <c>null</c>.
		/// </returns>
		public TConstruct ItemToAdd { get; protected set; }

		/// <summary>
		///		Gets the code construct which represents the argument for the key to be added to the dictionary.
		/// </summary>
		/// <returns>
		///		The code construct which represents the argument for the key to be added to the dictionary.
		///		This value will not be <c>null</c>.
		/// </returns>
		public TConstruct KeyToAdd { get; protected set; }

		/// <summary>
		///		Gets the code construct which represents the argument for the value to be added to the dictionary.
		/// </summary>
		/// <returns>
		///		The code construct which represents the argument for the key to be added to the dictionary.
		///		This value will not be <c>null</c>.
		/// </returns>
		public TConstruct ValueToAdd { get; protected set; }

		/// <summary>
		///		Gets the code construct which represents the argument for the initial capacity of the new collection.
		/// </summary>
		/// <returns>
		///		The code construct which represents the argument for the initial capacity of the new collection.
		///		This value will not be <c>null</c>.
		/// </returns>
		public TConstruct InitialCapacity { get; protected set; }

		/// <summary>
		///		Gets the code construct which represents the unpacking context for unpacking operations.
		/// </summary>
		/// <value>
		///		The code construct which represents the the unpacking context for unpacking operations.
		///		This value is initialized in <see cref="DefineUnpackingContext"/>.
		/// </value>
		public TConstruct UnpackingContextInUnpackValueMethods { get; private set; }

		/// <summary>
		///		Gets the code construct which represents the unpacking context for unpacking operations.
		/// </summary>
		/// <value>
		///		The code construct which represents the the unpacking context for unpacking operations.
		///		This value is initialized in <see cref="DefineUnpackingContext"/>.
		/// </value>
		public TConstruct UnpackingContextInSetValueMethods { get; private set; }

		/// <summary>
		///		Gets the code construct which represents the unpacking context for CreateObjectFromContext method.
		/// </summary>
		/// <value>
		///		The code construct which represents the the unpacking context for CreateObjectFromContext method.
		///		This value is initialized in <see cref="DefineUnpackingContext"/>.
		/// </value>
		public TConstruct UnpackingContextInCreateObjectFromContext { get; private set; }

		/// <summary>
		///		Gets the code construct which represents the index of unpacking item in the source array or map.
		/// </summary>
		/// <value>
		///		The code construct which represents the index of unpacking item in the source array or map.
		///		This value will not be <c>null</c>.
		/// </value>
		public TConstruct IndexOfItem { get; protected set; }

		/// <summary>
		///		Gets the code construct which represents the count of unpacking items in the source array or map.
		/// </summary>
		/// <value>
		///		The code construct which represents the count of unpacking items in the source array or map.
		///		This value will not be <c>null</c>.
		/// </value>
		public TConstruct ItemsCount { get; protected set; }

		/// <summary>
		///		Gets the configured nil-implication for collection items.
		/// </summary>
		/// <value>
		///		The configured nil-implication for collection items.
		/// </value>
		public NilImplication CollectionItemNilImplication { get; private set; }

		/// <summary>
		///		Gets the configured nil-implication for dictionary keys.
		/// </summary>
		/// <value>
		///		The configured nil-implication for dictionary keys.
		/// </value>
		public NilImplication DictionaryKeyNilImplication { get; private set; }

		// NOTE: Missing map value is MemberDefault

		/// <summary>
		///		Gets the configured nil-implication for tuple items.
		/// </summary>
		/// <value>
		///		The configured nil-implication for tuple items.
		/// </value>
		public NilImplication TupleItemNilImplication { get; private set; }

		private readonly IDictionary<string, MethodDefinition> _declaredMethods;

		/// <summary>
		///		Gets the declared method.
		/// </summary>
		/// <param name="name">The name of the method.</param>
		/// <returns>
		///		The <see cref="MethodDefinition"/>. This value will not be <c>null</c>.
		/// </returns>
		/// <exception cref="InvalidOperationException">The specified method has not been declared yet.</exception>
		public MethodDefinition GetDeclaredMethod( string name )
		{
			var method = this.TryGetDeclaredMethod( name );
			if ( method == null )
			{
				throw new InvalidOperationException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Method '{0}' is not declared yet.",
						name
					)
				);
			}
			return method;
		}

		/// <summary>
		///		Gets the declared method.
		/// </summary>
		/// <param name="name">The name of the method.</param>
		/// <returns>
		///		The <see cref="MethodDefinition"/>. This value will be <c>null</c> when the specified method is not declared.
		/// </returns>
		/// <exception cref="InvalidOperationException">The specified method has not been declared yet.</exception>
		public MethodDefinition TryGetDeclaredMethod( string name )
		{
			MethodDefinition method;
			if ( !this._declaredMethods.TryGetValue( name, out method ) )
			{
				return null;
			}
			return method;
		}

		/// <summary>
		///		Determines whether specified named private method is already declared or not.
		/// </summary>
		/// <param name="name">The name of the method.</param>
		/// <returns><c>true</c>, if specified named method is already declared; <c>fale</c>, otherwise.</returns>
		public bool IsDeclaredMethod( string name )
		{
			return this._declaredMethods.ContainsKey( name );
		}

		private readonly IDictionary<string, FieldDefinition> _declaredFields;

		/// <summary>
		///		Gets the declared field.
		/// </summary>
		/// <param name="name">The name of the field.</param>
		/// <returns>
		///		The <see cref="FieldDefinition"/>. This value will not be <c>null</c>.
		/// </returns>
		/// <exception cref="FieldDefinition">The specified field has not been declared yet.</exception>
		public FieldDefinition GetDeclaredField( string name )
		{
			FieldDefinition field;
			if ( !this._declaredFields.TryGetValue( name, out field ) )
			{
				throw new InvalidOperationException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Field '{0}' is not declared yet.",
						name
					)
				);
			}
			return field;
		}

		private readonly IDictionary<string, CachedDelegateInfo> _cachedDelegateInfos;

		public FieldDefinition GetCachedPrivateMethodDelegate( MethodDefinition method, TypeDefinition delegateType )
		{
			return this.GetCachedDelegateCore( method, delegateType, "this", true );
		}

		public FieldDefinition GetCachedStaticMethodDelegate( MethodDefinition method, TypeDefinition delegateType )
		{
			return this.GetCachedDelegateCore( method, delegateType, method.DeclaringType.TypeName, false );
		}

		private FieldDefinition GetCachedDelegateCore( MethodDefinition method, TypeDefinition delegateType, string prefix, bool isThis )
		{
			var key = prefix + "." + method.MethodName;
			CachedDelegateInfo delegateInfo;
			if ( !this._cachedDelegateInfos.TryGetValue( key, out delegateInfo ) )
			{
				delegateInfo = new CachedDelegateInfo( isThis, method, this.DeclarePrivateField( key.Replace( '.', '_' ) + "Delegate", delegateType ) );
				this._cachedDelegateInfos.Add( key, delegateInfo );
			}

			return delegateInfo.BackingField;
		}

		public IEnumerable<CachedDelegateInfo> GetCachedDelegateInfos()
		{
			return this._cachedDelegateInfos.Values;
		}

		private KeyValuePair<TypeDefinition, ConstructorDefinition> _unpackingContextDefinition;

		/// <summary>
		///		Gets the type of the unpacking context.
		/// </summary>
		/// <value>
		///		The type of the unpacking context. This value is <c>null</c> until <see cref="DefineUnpackingContext"/> called after last <see cref="Reset"/>.
		/// </value>
		public TypeDefinition UnpackingContextType { get { return this._unpackingContextDefinition.Key; } }

		/// <summary>
		///		Gets or sets a value indicating whether an UnpackTo method call is emitted or not. 
		/// </summary>
		/// <value>
		/// <c>true</c> if an UnpackTo method call is emitted; otherwise, <c>false</c>.
		/// </value>
		public bool IsUnpackToUsed { get; set; }

#if DEBUG
		private KeyValuePair<string, TypeDefinition>[] _lastUnpackingContextFields;
#endif // DEBUG

		/// <summary>
		///		Initializes a new instance of the <see cref="SerializerGenerationContext{TConstruct}"/> class.
		/// </summary>
		/// <param name="context">The serialization context.</param>
		protected SerializerGenerationContext( SerializationContext context )
		{
			this.SerializationContext = context;
			this.CollectionItemNilImplication = NilImplication.Null;
			this.DictionaryKeyNilImplication = NilImplication.Prohibit;
			this.TupleItemNilImplication = NilImplication.Null;
			this._declaredMethods = new Dictionary<string, MethodDefinition>();
			this._declaredFields = new Dictionary<string, FieldDefinition>();
			this._cachedDelegateInfos = new Dictionary<string, CachedDelegateInfo>();
		}

		/// <summary>
		///		Resets internal states for specified target type.
		/// </summary>
		/// <param name="targetType">Type of the serialization target.</param>
		/// <param name="baseClass">Type of base class of the target.</param>
		public void Reset( Type targetType, Type baseClass )
		{
			this.Packer = default( TConstruct );
			this.PackToTarget = default( TConstruct );
			this.NullCheckTarget = default ( TConstruct );
			this.Unpacker = default( TConstruct );
			this.UnpackToTarget = default( TConstruct );
			this.CollectionToBeAdded = default( TConstruct );
			this.ItemToAdd = default( TConstruct );
			this.KeyToAdd = default( TConstruct );
			this.ValueToAdd = default( TConstruct );
			this.InitialCapacity = default( TConstruct );
			this.UnpackingContextInUnpackValueMethods = default( TConstruct );
			this.UnpackingContextInSetValueMethods = default( TConstruct );
			this.UnpackingContextInCreateObjectFromContext = default( TConstruct );
			this.IndexOfItem = default( TConstruct );
			this.ItemsCount = default( TConstruct );
			this.ResetCore( targetType, baseClass );
			this._declaredMethods.Clear();
			this._declaredFields.Clear();
			this._cachedDelegateInfos.Clear();
			this._unpackingContextDefinition = default( KeyValuePair<TypeDefinition, ConstructorDefinition> );
			this.IsUnpackToUsed = false;
#if DEBUG
			this._lastUnpackingContextFields = null;
#endif // DEBUG
		}

		/// <summary>
		///		Resets internal states for specified target type.
		/// </summary>
		/// <param name="targetType">Type of the serialization target.</param>
		/// <param name="baseClass">Type of base class of the target.</param>
		protected abstract void ResetCore( Type targetType, Type baseClass );

		/// <summary>
		///		Gets a unique name of a local variable.
		/// </summary>
		/// <param name="prefix">The prefix of the variable.</param>
		/// <returns>A unique name of a local variable.</returns>
		public virtual string GetUniqueVariableName( string prefix )
		{
			// Many implementations do not need local variable name, so this method is not needed to do anything.
			return prefix;
		}

		/// <summary>
		///		Begins implementing overriding method.
		/// </summary>
		/// <param name="name">The name of the method.</param>
		public abstract void BeginMethodOverride( string name );

		/// <summary>
		///		Ends implementing overriding method.
		/// </summary>
		/// <param name="name">The name of the method.</param>
		/// <param name="body">The construct which represents whole method body.</param>
		/// <returns>
		///		The method definition of the overridden method.
		/// </returns>
		public MethodDefinition EndMethodOverride( string name, TConstruct body )
		{
			var method = this.EndMethodOverrideCore( name, body );
			this._declaredMethods[ name ] = method;
			return method;
		}

		/// <summary>
		///		Ends implementing overriding method.
		/// </summary>
		/// <param name="name">The name of the method.</param>
		/// <param name="body">The construct which represents whole method body.</param>
		/// <returns>
		///		The method definition of the overridden method.
		/// </returns>
		protected abstract MethodDefinition EndMethodOverrideCore( string name, TConstruct body );

		/// <summary>
		///		Begins implementing private method.
		/// </summary>
		/// <param name="name">The name of the method.</param>
		/// <param name="isStatic"><c>true</c> for static method.</param>
		/// <param name="returnType">The type of the method return value.</param>
		/// <param name="parameters">The name and type pairs of the method parameters.</param>
		public abstract void BeginPrivateMethod( string name, bool isStatic, TypeDefinition returnType, params TConstruct[] parameters );

		/// <summary>
		///		Ends current implementing private method.
		/// </summary>
		/// <param name="name">The name of the method.</param>
		/// <param name="body">The construct which represents whole method body.</param>
		/// <returns>
		///		The method definition of the private method.
		/// </returns>
		public MethodDefinition EndPrivateMethod( string name, TConstruct body )
		{
			var method = this.EndPrivateMethodCore( name, body );
			this._declaredMethods[ name ] = method;
			return method;
		}

		/// <summary>
		///		Ends current implementing private method.
		/// </summary>
		/// <param name="name">The name of the method.</param>
		/// <param name="body">The construct which represents whole method body.</param>
		/// <returns>
		///		The method definition of the private method.
		/// </returns>
		protected abstract MethodDefinition EndPrivateMethodCore( string name, TConstruct body );

		/// <summary>
		///		Declares new private field.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public FieldDefinition DeclarePrivateField( string name, TypeDefinition type )
		{
			var field = this.DeclarePrivateFieldCore( name, type );
			this._declaredFields[ name ] = field;
			return field;
		}

		/// <summary>
		///		Declares new private field.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		protected abstract FieldDefinition DeclarePrivateFieldCore( string name, TypeDefinition type );

		/// <summary>
		///		Defines the unpacking context type.
		/// </summary>
		/// <param name="fields">The fields must be declared.</param>
		/// <param name="type">
		///		The type definition of the unpacking context.
		///		Note that this type will be existing property bag or generated private type.
		/// </param>
		/// <param name="constructor">
		///		The constructor of the context.
		/// </param>
		public void DefineUnpackingContext(
			KeyValuePair<string, TypeDefinition>[] fields,
			out TypeDefinition type,
			out ConstructorDefinition constructor
		)
		{
			if ( this.UnpackingContextType != null )
			{
#if DEBUG
				Contract.Assert(
					this._lastUnpackingContextFields.Select( kv => kv.Key + ":" + kv.Value )
						.SequenceEqual( fields.Select( kv => kv.Key + ":" + kv.Value ) ),
					"Duplicated UnpackingContext registration."
				);
#endif // DEBUG
				type = this._unpackingContextDefinition.Key;
				constructor = this._unpackingContextDefinition.Value;
				return;
			}

#if DEBUG
			this._lastUnpackingContextFields = fields.ToArray();
#endif // DEBUG
			TConstruct parameterInUnpackValueMethods, parameterInSetValueMethods, parameterInCreateObjectFromContext;
			this.DefineUnpackingContextCore( fields, out type, out constructor, out parameterInUnpackValueMethods, out parameterInSetValueMethods, out parameterInCreateObjectFromContext );
			this.UnpackingContextInUnpackValueMethods = parameterInUnpackValueMethods;
			this.UnpackingContextInSetValueMethods = parameterInSetValueMethods;
			this.UnpackingContextInCreateObjectFromContext = parameterInCreateObjectFromContext;
			this._unpackingContextDefinition = new KeyValuePair<TypeDefinition, ConstructorDefinition>( type, constructor );
		}

		/// <summary>
		///		Defines the unpacking context type.
		/// </summary>
		/// <param name="fields">The fields must be declared.</param>
		/// <param name="type">
		///		The type definition of the unpacking context.
		///		Note that this type will be existing property bag or generated private type.
		/// </param>
		/// <param name="constructor">
		///		The constructor of the context.
		/// </param>
		/// <param name="parameterInUnpackValueMethods">The <paramref name="type"/> typed parameter for unpacking operations.</param>
		/// <param name="parameterInSetValueMethods">The <paramref name="type"/> typed parameter for unpacking operations.</param>
		/// <param name="parameterInCreateObjectFromContext">The <paramref name="type"/> typed parameter for CreateObjectFromContext method.</param>
		protected abstract void DefineUnpackingContextCore(
			IList<KeyValuePair<string, TypeDefinition>> fields,
			out TypeDefinition type,
			out ConstructorDefinition constructor,
			out TConstruct parameterInUnpackValueMethods,
			out TConstruct parameterInSetValueMethods,
			out TConstruct parameterInCreateObjectFromContext
		);

		/// <summary>
		///		Defines the unpacking context type with result object type.
		/// </summary>
		/// <returns>The unpacking context type.</returns>
		public TypeDefinition DefineUnpackingContextWithResultObject()
		{
			TypeDefinition type;
			TConstruct parameterInUnpackValueMethods, parameterInSetValueMethods, parameterInCreateObjectFromContext;
			this.DefineUnpackingContextWithResultObjectCore( out type, out parameterInUnpackValueMethods, out parameterInSetValueMethods, out parameterInCreateObjectFromContext );
			this.UnpackingContextInUnpackValueMethods = parameterInUnpackValueMethods;
			this.UnpackingContextInSetValueMethods = parameterInSetValueMethods;
			this.UnpackingContextInCreateObjectFromContext = parameterInCreateObjectFromContext;
			this._unpackingContextDefinition = new KeyValuePair<TypeDefinition, ConstructorDefinition>( type, null );
			return type;
		}

		/// <summary>
		///		Defines the unpacking context type with result object type.
		/// </summary>
		/// <param name="type">
		///		The type definition of the unpacking context.
		///		Note that this type will be existing property bag or generated private type.
		/// </param>
		/// <param name="parameterInUnpackValueMethods">The <paramref name="type"/> typed parameter for unpacking operations.</param>
		/// <param name="parameterInSetValueMethods">The <paramref name="type"/> typed parameter for unpacking operations.</param>
		/// <param name="parameterInCreateObjectFromContext">The <paramref name="type"/> typed parameter for CreateObjectFromContext method.</param>
		protected abstract void DefineUnpackingContextWithResultObjectCore(
			out TypeDefinition type,
			out TConstruct parameterInUnpackValueMethods,
			out TConstruct parameterInSetValueMethods,
			out TConstruct parameterInCreateObjectFromContext
		);

		/// <summary>
		///		Defines the unpacked item parameter in set value methods.
		/// </summary>
		/// <param name="itemType">Type of the value.</param>
		/// <returns>The parameter construct.</returns>
		public abstract TConstruct DefineUnpackedItemParameterInSetValueMethods( TypeDefinition itemType );
	}
}