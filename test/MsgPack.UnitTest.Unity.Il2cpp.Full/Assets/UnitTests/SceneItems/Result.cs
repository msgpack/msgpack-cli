#if !UNITY_METRO && !UNITY_4_5

using System;
using UnityEngine;
using UniRx;

// This file is borrowed from UniRX (https://github.com/neuecc/UniRx/blob/master/Assets/ScheneItems/Result.cs)

namespace MsgPack
{
	/// <summary>
	///		Implements presentation logic for text test result.
	/// </summary>
	public class Result : PresenterBase
    {
		/// <summary>
		///		The Text Widget to be updated from this presenter.
		/// </summary>
		public UnityEngine.UI.Text text;

		/// <summary>
		///		Gets the reactive property for result message text.
		/// </summary>
		/// <value>
		///		The reactive property for result message text.
		/// </value>
		public ReactiveProperty<string> Message { get; private set; }

		/// <summary>
		///		Gets the reactive property for result coloring.
		/// </summary>
		/// <value>
		///		The reactive property for result coloring.
		/// </value>
		public ReactiveProperty<Color> Color { get; private set; }

		protected override IPresenter[] Children
		{
			get { return EmptyChildren; }
		}

		protected override void BeforeInitialize() {}

		protected override void Initialize()
		{
			var image = this.GetComponent<UnityEngine.UI.Image>();

			Message = new ReactiveProperty<string>( "" );
			Message.SubscribeToText( text );

			Color = new ReactiveProperty<UnityEngine.Color>();
			Color.Subscribe( x => image.color = x );
		}
    }
}

#endif // !UNITY_METRO && !UNITY_4_5