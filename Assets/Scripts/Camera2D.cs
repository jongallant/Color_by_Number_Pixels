using UnityEngine;


public class Camera2D : MonoBehaviour
{
	public Transform Area;

    float Top;
    private Vector2 Size;
    private Vector2 Offset;
    private float PixelUnits;

    float Distance = 20;
    bool LockZoom;
    public float ZoomVelocity;
    float ZoomSpeed = 50.0f;
    float ZoomMin = 10f, ZoomMax = 25f;

    Camera Camera;

    private void Awake()
    {
        Camera = transform.gameObject.GetComponent<Camera>();
    }

    private void Start()
    {
        Set();
    }

    public void FixedUpdate()
    {
        Camera.orthographicSize = Distance;
    }

    public float GetDistance()
    {
        return Distance;

    }
    public void SetDistance(float distance)
    {
        Distance = distance;
        Camera.orthographicSize = Distance;
    }

    public void Update()
    {
        if (!LockZoom)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll < 0 && ZoomVelocity > 0 || scroll > 0 && ZoomVelocity < 0)
                ZoomVelocity = 0;

            ZoomVelocity += Time.deltaTime * scroll * ZoomSpeed;
            Distance -= ZoomVelocity;
            Distance = Mathf.Clamp(Distance, ZoomMin, ZoomMax);

            ZoomVelocity = Mathf.Lerp(ZoomVelocity, 0, 0.1f);
        }
    }

    public void Set()
	{
        if (Camera == null)
            Camera = transform.gameObject.GetComponent<Camera>();

        float height = Area.localScale.y * 100;
		float width = Area.localScale.x * 100;

		float w = Screen.width / width;
		float h = Screen.height / height;

		float ratio = w / h;
		float size = (height / 2) / 100f;

		if (w < h)
			size /= ratio;

        Camera.orthographicSize = size;

		Vector2 position = Area.transform.position;

		Vector3 camPosition = position;
		Vector3 point = Camera.WorldToViewportPoint(camPosition);
		Vector3 delta = camPosition - Camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
		Vector3 destination = transform.position + delta;

		transform.position = destination;

        RefreshBounds();   
	}

	private void RefreshBounds()
	{
		Sprite sprite = Area.gameObject.GetComponent<SpriteRenderer> ().sprite;
		PixelUnits = sprite.rect.width / sprite.bounds.size.x;

		Size = new Vector2(Area.transform.localScale.x * sprite.texture.width / PixelUnits,
			Area.transform.localScale.y * sprite.texture.height / PixelUnits);

		Offset = Area.transform.position;

		var vertExtent = Camera.orthographicSize;

		Top = Size.y / 2.0f - vertExtent + Offset.y;

        Vector3 v3 = transform.position;
        v3.y = Top;
        transform.position = v3;
    }

}





