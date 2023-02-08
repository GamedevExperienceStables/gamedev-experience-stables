using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Yapp
{
    /// <summary>
    /// Coroutines in Unity Editor. This would also be possible via plain Unity, but it would require importing a package.
    /// This solution works without having to include a package. Full credit to MogulTech.
    /// Source: https://forum.unity.com/threads/editor-coroutines.589504/
    /// </summary>
    public static class EditorCoroutines
    {

        public class Coroutine
        {
            public IEnumerator enumerator;
            public System.Action<bool> OnUpdate;
            public List<IEnumerator> history = new List<IEnumerator>();
        }

        static readonly List<Coroutine> coroutines = new List<Coroutine>();

        public static void Execute(IEnumerator enumerator, System.Action<bool> OnUpdate = null)
        {
            if (coroutines.Count == 0)
            {
                EditorApplication.update += Update;
            }
            var coroutine = new Coroutine { enumerator = enumerator, OnUpdate = OnUpdate };
            coroutines.Add(coroutine);
        }

        static void Update()
        {
            for (int i = 0; i < coroutines.Count; i++)
            {
                var coroutine = coroutines[i];
                bool done = !coroutine.enumerator.MoveNext();
                if (done)
                {
                    if (coroutine.history.Count == 0)
                    {
                        coroutines.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        done = false;
                        coroutine.enumerator = coroutine.history[coroutine.history.Count - 1];
                        coroutine.history.RemoveAt(coroutine.history.Count - 1);
                    }
                }
                else
                {
                    if (coroutine.enumerator.Current is IEnumerator)
                    {
                        coroutine.history.Add(coroutine.enumerator);
                        coroutine.enumerator = (IEnumerator)coroutine.enumerator.Current;
                    }
                }
                if (coroutine.OnUpdate != null) coroutine.OnUpdate(done);
            }
            if (coroutines.Count == 0) EditorApplication.update -= Update;
        }

        internal static void StopAll()
        {
            coroutines.Clear();
            EditorApplication.update -= Update;
        }

    }
}

