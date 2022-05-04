using System.Collections ;
using UnityEngine ;

public class PlayerController : MonoBehaviour
{
    public float speed = 0.0025f ; // seconds
    private float phi ;
    private float theta ;

    void Start()
    {
		
    }

    public void onAxis2DEvent(Vector2 values)
    {
        phi = values.y ;
        theta = values.x ;
            transform.Translate(speed * new Vector3(phi * Mathf.Sin(theta), 0.0f, phi * Mathf.Cos(theta))) ;
        transform.Rotate(0.0f, theta, 0.0f, Space.Self);
    }
}