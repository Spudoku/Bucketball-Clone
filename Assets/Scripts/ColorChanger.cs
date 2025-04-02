using UnityEngine;

// changes the color of the ball when it enters
// the trigger
public class ColorChanger : MonoBehaviour
{
    // change color of gameobject to color of this object
    void OnTriggerEnter2D(Collider2D collision)
    {
        Renderer myR = GetComponent<Renderer>();
        Renderer cR = collision.gameObject.GetComponent<Renderer>();
        cR.sharedMaterial = myR.sharedMaterial;
    }
}
