using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreditsManager : MonoBehaviour {

    public Image creditsImage;
    public Sprite[] images;
    public GameObject leftButton;
    public GameObject rightButton;

    private int currentScreen = 0;

    public virtual void NextCredits()
    {
        if (++currentScreen >= 2)
        {
            rightButton.SetActive(false);
            currentScreen = 2;
        }

        if (currentScreen > 0)
        {
            leftButton.SetActive(true);
        }

        creditsImage.sprite = images[currentScreen];
    }

    public virtual void PreviousCredits()
    {
        if (--currentScreen <= 0)
        {
            leftButton.SetActive(false);
            currentScreen = 0;
        }

        if (currentScreen < 2)
        {
            rightButton.SetActive(true);
        }

        creditsImage.sprite = images[currentScreen];
    }
}
