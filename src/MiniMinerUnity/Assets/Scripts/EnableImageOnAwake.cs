using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class EnableImageOnAwake : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.enabled = true;
    }
}
