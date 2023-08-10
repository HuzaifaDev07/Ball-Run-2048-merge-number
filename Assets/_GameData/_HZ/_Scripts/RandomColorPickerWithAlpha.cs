using UnityEngine;

public class RandomColorPickerWithAlpha : MonoBehaviour
{
    public Color[] colors; // Assign your desired colors here
    public float transitionSpeed = 1.0f;
    public Renderer targetRenderer; // Assign the Renderer component of the object here

    private Material material;
    private int currentIndex = 0;

    private void Start()
    {
        material = targetRenderer.material;
        material.color = colors[0]; // Set the initial color
        StartCoroutine(TransitionColor());
    }

    private System.Collections.IEnumerator TransitionColor()
    {
        while (true)
        {
            float startTime = Time.time;
            float elapsedTime = 0f;

            while (elapsedTime < transitionSpeed)
            {
                float t = elapsedTime / transitionSpeed;
                Color currentColor = Color.Lerp(colors[currentIndex], colors[(currentIndex + 1) % colors.Length], t);
                material.color = currentColor;

                elapsedTime = Time.time - startTime;
                yield return null;
            }

            currentIndex = (currentIndex + 1) % colors.Length;
        }
    }
}
