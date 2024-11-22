using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSpriteManager : MonoBehaviour
{
     public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private int frame;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Invoke(nameof(Animate), 0f); // value of 0f will get called in the next frame
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Animate()
    {
        frame++;

        if (frame >= sprites.Length) {
            frame = 0;
        }

        if (frame >= 0 && frame < sprites.Length) {
            spriteRenderer.sprite = sprites[frame];
        }

        Invoke(nameof(Animate), 1f / ClassicModeManager.Instance.gameSpeed);
    }

}
