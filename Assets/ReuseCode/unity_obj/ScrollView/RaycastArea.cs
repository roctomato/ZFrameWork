using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace LR
{
    [ExecuteInEditMode]
    public class RaycastArea : UnityEngine.UI.MaskableGraphic
    {
        protected override void Awake()
        {
            useLegacyMeshGeneration = false;
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }
    }
}
