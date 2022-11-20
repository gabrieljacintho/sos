using UnityEngine;
using UnityEngine.UI;

public class ParallaxScript : MonoBehaviour
{
    [SerializeField]
    private RawImage[] rawImages = null;

    private float cameraWidth;

    private void Start()
    {
        cameraWidth = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, 0)), new Vector2(Camera.main.transform.position.x, 0)) * 2;
    }

    private void Update()
    {
        //if (!GameManager.instance.loadingScene)
        //{
        float cameraPositionNormalized = Camera.main.transform.position.x;// - GameManager.instance.initialPosition.x;

            for (int i = 0; i < rawImages.Length; i++)
            {
                rawImages[i].uvRect = new Rect(cameraPositionNormalized * ((i + 2) * 0.1f) / cameraWidth, 0, rawImages[i].uvRect.width, rawImages[i].uvRect.height);
            }
        //}
    }
}
