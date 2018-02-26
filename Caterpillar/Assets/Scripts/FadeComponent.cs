using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeComponent : MonoBehaviour {

    public List<FadeComponent> FadeChildren;
    public float fadeOutSpeed = 0.5f;
    public float fadeInSpeed = 0.5f;
    public float lifetime = 0.0f; // lifetime == 0 indicates to NOT use lifetime

    private bool isFadingOut;
    private bool isFadingIn;
    private float remainingLifetime;
    private Graphic renderable;
    private Color color;

    void Start()
    {
        remainingLifetime = lifetime;
    }

    void Update()
    {
        if(isFadingOut)
        {
            renderable.color = new Color(color.r, color.g, color.b, renderable.color.a - (fadeOutSpeed * Time.deltaTime));

            if (renderable.color.a <= 0.0f)
            {
                isFadingOut = false;
                gameObject.SetActive(false);
            }
        }
        else if(isFadingIn)
        {
            renderable.color = new Color(color.r, color.g, color.b, renderable.color.a + (fadeInSpeed * Time.deltaTime));

            if (renderable.color.a >= 1.0f)
            {
                isFadingIn = false;
            }
        }
        else if(lifetime > 0 && !isFadingOut && !isFadingIn)
        {
            remainingLifetime -= Time.deltaTime;
            if (remainingLifetime <= 0)
            {
                isFadingOut = true;
            }
        }
    }

    public void StartFadeOut()
    {
        Setup();

        isFadingOut = true;
        isFadingIn = false;

        foreach (FadeComponent comp in FadeChildren)
            comp.StartFadeOut();
    }

    public void StartFadeIn()
    {
        Setup();

        isFadingOut = false;
        isFadingIn = true;

        renderable.color = new Color(color.r, color.g, color.b, 0.0f);

        foreach (FadeComponent comp in FadeChildren)
            comp.StartFadeIn();
    }

    private void Setup()
    {
        renderable = GetComponent<Graphic>();
        color = renderable.color;
    }

}
