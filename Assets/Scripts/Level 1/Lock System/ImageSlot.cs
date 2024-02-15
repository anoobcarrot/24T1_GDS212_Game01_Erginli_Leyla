using UnityEngine;
using UnityEngine.UI;

public class ImageSlot : MonoBehaviour
{
    public Image image;
    public Sprite[] imageOptions; // Array of images to cycle through
    private int currentIndex = 0;

    public void CycleImageUp()
    {
        currentIndex = (currentIndex + 1) % imageOptions.Length;
        image.sprite = imageOptions[currentIndex];
    }

    public void CycleImageDown()
    {
        currentIndex = (currentIndex - 1 + imageOptions.Length) % imageOptions.Length;
        image.sprite = imageOptions[currentIndex];
    }
}

