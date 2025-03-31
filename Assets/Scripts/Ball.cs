using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// click and drag to aim
// interacts with an Aimer to help the player
public class Ball : MonoBehaviour
{
    [SerializeField] float sensitivity;         // how much the power scales per unit the mouse is dragged
    [SerializeField] List<Material> materials;  // stores all valid materials
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
