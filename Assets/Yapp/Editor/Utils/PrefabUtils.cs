using System.Linq;
using UnityEngine;

namespace Yapp
{
    public class PrefabUtils
    {
        public static Transform[] GetContainerChildren( GameObject container)
        {
            if (container == null)
                return new Transform[0];

            Transform[] children = container.transform.Cast<Transform>().ToArray();

            return children;
        }
    }
}