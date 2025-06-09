using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnimation : MonoBehaviour
{
    public Animator Panel;
    public void DownAnim()
    {
        Panel.SetTrigger("Down");
    }

    public void UpAnim()
    {
        Panel.SetTrigger("Up");
    }
}
