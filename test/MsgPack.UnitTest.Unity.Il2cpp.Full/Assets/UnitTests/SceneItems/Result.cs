#if !UNITY_METRO && !UNITY_4_5

using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

// This file is borrowed from UniRX (https://github.com/neuecc/UniRx/blob/master/Assets/ScheneItems/Result.cs)

namespace MsgPack
{
    public class Result : PresenterBase
    {
        public UnityEngine.UI.Text text;

        public ReactiveProperty<string> Message { get; private set; }
        public ReactiveProperty<Color> Color { get; private set; }

        protected override IPresenter[] Children
        {
            get
            {
                return EmptyChildren;
            }
        }

        protected override void BeforeInitialize()
        {
        }

        protected override void Initialize()
        {
            var image = this.GetComponent<UnityEngine.UI.Image>();

            Message = new ReactiveProperty<string>("");
            Message.SubscribeToText(text);

            Color = new ReactiveProperty<UnityEngine.Color>();
            Color.Subscribe(x => image.color = x);
        }
    }
}

#endif