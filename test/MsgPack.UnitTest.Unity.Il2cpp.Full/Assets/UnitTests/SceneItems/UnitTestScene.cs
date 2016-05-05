#if !UNITY_METRO && !UNITY_4_5

using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

// This file is borrowed from UniRX (https://github.com/neuecc/UniRx/blob/master/Assets/ScheneItems/UnitTestScene.cs)

namespace MsgPack
{
	public class UnitTestScene : MonoBehaviour
	{
		public Button buttonPrefab;
		public GameObject buttonVertical;

		public Result resultPrefab;
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