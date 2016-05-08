#if !UNITY_METRO && !UNITY_4_5

using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

// This file is borrowed from UniRX (https://github.com/neuecc/UniRx/blob/master/Assets/ScheneItems/UnitTestScene.cs)

namespace MsgPack
{
	/// <summary>
	///		The presenter for whole unit testing tool.
	/// </summary>
	public class UnitTestScene : MonoBehaviour
	{
		/// <summary>
		///		The prefab for test starting buttons.
		/// </summary>
		public Button buttonPrefab;

		/// <summary>
		///		The vertical area which test starting buttons to be belonging.
		/// </summary>
		public GameObject buttonVertical;

		/// <summary>
		///		The prefab for test result indicators.
		/// </summary>
		public Result resultPrefab;

		/// <summary>
		///		The vertical area which test result indicators to be belonging.
		/// </summary>
		public GameObject resultVertical;


		void Start()
		{
			// UnitTest uses Wait, it can't run on MainThreadScheduler.
			Scheduler.DefaultSchedulers.SetDotNetCompatible();
			MainThreadDispatcher.Initialize();

			new UnityTestDriver().Run( this.buttonPrefab, this.buttonVertical, this.resultPrefab, this.resultVertical );
		}
	}
}

#endif