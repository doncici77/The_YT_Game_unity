using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyController : MonoBehaviour
{
    public Material transparentMaterial;
    public float transparency = 0.5f; // 0: 완전히 투명, 1: 완전히 불투명

    void Start()
    {
        SetTransparency(transparency);
    }

    void SetTransparency(float alpha)
    {
        if (transparentMaterial != null)
        {
            Color color = transparentMaterial.color;
            color.a = alpha;
            transparentMaterial.color = color;
        }
    }
}

