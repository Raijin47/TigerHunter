using UnityEngine;

public class BushShadow : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Game.Locator.Player.IsHide = true;
        //Game.Audio.PlayClip(2);
    }

    private void OnTriggerExit(Collider other)
    {
        //Game.Locator.Player.IsHide = false;
    }
}