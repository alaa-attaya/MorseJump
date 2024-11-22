using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
public class GroundManager : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        float speed = ClassicModeManager.Instance.gameSpeed / transform.localScale.x;
        meshRenderer.material.mainTextureOffset += speed * Time.deltaTime * Vector2.right;
    }
}
