using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	private Camera mainCamera;
	// Start is called before the first frame update
	void Start()
	{
		mainCamera = Camera.main;
		gameObject.GetComponent<Canvas>().worldCamera = mainCamera;
	}

	// Update is called once per frame
	void LateUpdate()
	{
		transform.LookAt(mainCamera.transform);
        transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
	}
}
