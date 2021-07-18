using UnityEngine;

public class CameraOnPlayer : MonoBehaviour
{
    public Camera cam;
    public float cameraDepthOffset;
    public float speed;

    // Start 'Borders' :

    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;

    // End 'Borders'

    private void Start()
    {
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
    }

    private void LateUpdate()
    {
        cam.transform.position = Vector2.Lerp(new Vector2(cam.transform.position.x, cam.transform.position.y), transform.position, speed * Time.deltaTime);
        cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, -10f);
    }
}