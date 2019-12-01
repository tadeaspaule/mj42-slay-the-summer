using UnityEngine;
using UnityEngine.UI;

public class TitleScreenLoop : MonoBehaviour
{
    public Image img; 
    Sprite[] sprites;
    int i = 0;
    public float frameGap = 0.1f;
    float timer = 0f;

    void Start()
    {
        sprites = Resources.LoadAll<Sprite>("animations/playerattack");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= frameGap) {
            timer = 0f;
            i = (i + 1) % sprites.Length;
            img.sprite = sprites[i];
        }
    }
}
