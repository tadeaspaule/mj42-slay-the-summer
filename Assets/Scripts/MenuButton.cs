using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public GameObject outline;
    
    public void MouseEntered()
    {
        outline.SetActive(true);
    }

    public void MouseLeft()
    {
        outline.SetActive(false);

    }
}
