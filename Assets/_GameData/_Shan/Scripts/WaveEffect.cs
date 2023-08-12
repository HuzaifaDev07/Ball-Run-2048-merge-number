using UnityEngine;

public class WaveEffect : MonoBehaviour
{
    public float speed = 1.0f; // Controls how fast the wave moves
    public float amplitude = 0.1f; // Controls the height of the wave

    private SpriteRenderer spriteRenderer;
    private Vector2[] baseVertices;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseVertices = spriteRenderer.sprite.vertices;
    }

    private void Update()
    {
        Vector2[] vertices = spriteRenderer.sprite.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 vertex = vertices[i];
            vertex.y = baseVertices[i].y + amplitude * Mathf.Sin(Time.time * speed + vertex.x);
            vertices[i] = vertex;

            // Clamp the modified vertex y-coordinate to ensure it stays within sprite bounds
            float minY = Mathf.Min(baseVertices[i].y, vertex.y);
            float maxY = Mathf.Max(baseVertices[i].y, vertex.y);
            vertices[i].y = Mathf.Clamp(vertices[i].y, minY, maxY);
        }

        spriteRenderer.sprite.OverrideGeometry(vertices, spriteRenderer.sprite.triangles);
    }
}
