using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    private float leftEdge;

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
    }

    private void Update()
    {
        transform.position += ClassicModeManager.Instance.gameSpeed * Time.deltaTime * Vector3.left;

        if (transform.position.x < leftEdge) {
            Destroy(gameObject);
        }
    }
}
