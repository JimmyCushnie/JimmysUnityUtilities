using System;
using System.Collections.Generic;
using Lidgren.Network;
using UnityEngine;
using System.ComponentModel;
using System.Linq.Expressions;

using UnityEngine.UI;
using Object = UnityEngine.Object;
using Component = UnityEngine.Component;
using Random = UnityEngine.Random;

namespace JimmysUnityUtilities
{
    public static class ScrollRectExtensions
    {
        public static void ScrollToBottom(this ScrollRect scrollRect)
            => scrollRect.verticalNormalizedPosition = 0;

        public static void ScrollToTop(this ScrollRect scrollRect)
            => scrollRect.verticalNormalizedPosition = 1;
    }
}