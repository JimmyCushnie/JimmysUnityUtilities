using UnityEngine;

namespace JimmysUnityUtilities
{
    internal static class MegaPoolParentHolder
    {
        public static Transform _MegaParent;
        public static Transform MegaParent
        {
            get
            {
                if (_MegaParent == null)
                {
                    _MegaParent = new GameObject("pooled objects parent").transform;
                    _MegaParent.gameObject.SetActive(false);
                }

                return _MegaParent;
            }
        }
    }
}
