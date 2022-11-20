using UnityEngine;

public class SingleObjectScript : MonoBehaviour
{
    private void Start()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(gameObject.tag);
        if (gameObjects.Length > 1) Destroy(gameObject);
    }
}
