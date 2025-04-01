using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Renderer myR = GetComponent<Renderer>();
        Renderer cR = collision.gameObject.GetComponent<Renderer>();
        cR.sharedMaterial = myR.sharedMaterial;
    }
}
