using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour 
{
	[SerializeField]
	protected Vector2 cameraSizeRange = new Vector2(3, 7);

	[SerializeField]
	protected float scrollScale = 0.5f,
		panScale = 0.3f;

	private new Camera camera;

	private void Start()
	{
		camera = GetComponent<Camera>();
	}
	
	private void Update ()
	{
		var scroll = Input.mouseScrollDelta.y * scrollScale;
		camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - scroll, cameraSizeRange.x,
			cameraSizeRange.y);
		
		camera.transform.position += new Vector3(Input.GetAxis("Horizontal") * panScale, 
			Input.GetAxis("Vertical") * panScale);
	}
}
