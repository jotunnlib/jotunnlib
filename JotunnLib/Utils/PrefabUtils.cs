using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JotunnLib.Utils
{
    public static class PrefabUtils
    {
        public static void NestChildrenUnderAttach(GameObject gameObject, bool force = false)
        {
            if (!gameObject)
            {
                Debug.LogError("Failed to NestChildrenUnderAttach for invalid GameObject");
                return;
            }

            if (gameObject.transform.Find("attach") && !force)
            {
                Debug.LogWarning($"GameObject ${gameObject.name} already contains an attach object, aborting NestChildrenUnderAttach");
                return;
            }

            GameObject attach = new GameObject("attach");

            foreach (Transform t in gameObject.transform)
            {
                Vector3 localPos = t.localPosition;
                t.parent = attach.transform;
                t.transform.localPosition = localPos;
            }

            attach.transform.parent = gameObject.transform;
            attach.transform.localPosition = Vector3.zero;
        }
    }
}
