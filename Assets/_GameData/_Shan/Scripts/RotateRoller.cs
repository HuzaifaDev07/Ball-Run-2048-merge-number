using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRoller : MonoBehaviour
{
    public Material metalChain;
    public int direction = 1;
    [Range(0, 10)] public float speed = 0.1f;
    [Range(0, 10)] public float speedMultiplier = 1;

    int texID = Shader.PropertyToID("_MainTex");
    Vector2 offset = Vector2.zero;

    void Update()
    {
        offset.y += Time.deltaTime * (speed * direction) * (speedMultiplier / 50);
        metalChain.SetTextureOffset(texID, offset);
    }
}
