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
    public static class ScrollbarExtensions
    {
        public static void ScrollToEnd(this Scrollbar scrollbar)
            => scrollbar.value = 1;

        public static void ScrollToStart(this Scrollbar scrollbar)
            => scrollbar.value = 0;
    }
}