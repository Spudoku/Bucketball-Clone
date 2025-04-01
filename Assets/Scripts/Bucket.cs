using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bucket : MonoBehaviour
{

    [SerializeField] List<Material> materials;  // stores all valid materials
    private Renderer r;
    // Start is called before the first frame update
    void Start()
    {
        RefreshChildren();
        r = GetComponent<Renderer>();
    }

    // update child objects to have the same material as the main object
    private void RefreshChildren()
    {
        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.sharedMaterial;
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.sharedMaterial = mat;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Something triggered me!");
        if (collision.gameObject.TryGetComponent<Ball>(out var ball))
        {
            Renderer cR = collision.gameObject.GetComponent<Renderer>();
            if (r.sharedMaterial.name != cR.sharedMaterial.name)
            {
                Debug.Log($"Losing! {r.sharedMaterial.name} vs {cR.sharedMaterial.name}");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                Debug.Log("Winning!");
                // call some method in ball?
                StartCoroutine(ball.Yay());
                StartCoroutine(WinLevel());
            }

        }
    }

    private IEnumerator WinLevel()
    {

        // let the player process what just happened
        yield return new WaitForSeconds(2f);
        // advance to next level, or back to level 1 if the last level is reached
        int count = SceneManager.sceneCountInBuildSettings;
        int cur = SceneManager.GetActiveScene().buildIndex;
        if (cur >= count - 1)
        {
            cur = 0;
        }
        else
        {
            cur++;
        }
        SceneManager.LoadScene(cur);
    }
}
