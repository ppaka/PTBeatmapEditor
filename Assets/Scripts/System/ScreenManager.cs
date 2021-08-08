using UnityEngine;

public class ScreenManager : MonoBehaviour
{
	static bool _landscapeModeOnly = true;
	static float _wantedAspectRatio;
	static Camera _cam;
	public float wantedAspectRatio = 9 / 16;
	public bool landscapeModeOnly = true;

	public static int ScreenHeight => (int) (Screen.height * _cam.rect.height);

	public static int ScreenWidth => (int) (Screen.width * _cam.rect.width);

	public static int XOffset => (int) (Screen.width * _cam.rect.x);

	public static int YOffset => (int) (Screen.height * _cam.rect.y);

	public static Rect ScreenRect => new Rect(_cam.rect.x * Screen.width, _cam.rect.y * Screen.height,
		_cam.rect.width * Screen.width, _cam.rect.height * Screen.height);

	public static Vector3 MousePosition
	{
		get
		{
			Vector3 mousePos = Input.mousePosition;
			Rect rect = _cam.rect;
			mousePos.y -= (int) (rect.y * Screen.height);
			mousePos.x -= (int) (rect.x * Screen.width);
			return mousePos;
		}
	}

	public static Vector2 GUIMousePosition
	{
		get
		{
			Vector2 mousePos = Event.current.mousePosition;
			Rect rect = _cam.rect;
			mousePos.y = Mathf.Clamp(mousePos.y, rect.y * Screen.height,
				rect.y * Screen.height + rect.height * Screen.height);
			Rect rect1 = _cam.rect;
			mousePos.x = Mathf.Clamp(mousePos.x, rect1.x * Screen.width,
				rect1.x * Screen.width + rect1.width * Screen.width);
			return mousePos;
		}
	}

	void Awake()
	{
		_landscapeModeOnly = landscapeModeOnly;
		_cam = GetComponent<Camera>();
		if (!_cam) _cam = Camera.main;

		_wantedAspectRatio = wantedAspectRatio;
		SetCamera();
	}

	static void SetCamera()
	{
		float currentAspectRatio;
		if (Screen.orientation == ScreenOrientation.LandscapeRight ||
		    Screen.orientation == ScreenOrientation.LandscapeLeft)
		{
			currentAspectRatio = (float) Screen.width / Screen.height;
		}
		else
		{
			if (Screen.height > Screen.width && _landscapeModeOnly)
				currentAspectRatio = (float) Screen.height / Screen.width;
			else
				currentAspectRatio = (float) Screen.width / Screen.height;
		}
		// If the current aspect ratio is already approximately equal to the desired aspect ratio,
		// use a full-screen Rect (in case it was set to something else previously)

		//Debug.Log ("currentAspectRatio = " + currentAspectRatio + ", wantedAspectRatio = " + wantedAspectRatio);

		if ((int) (currentAspectRatio * 100) / 100.0f == (int) (_wantedAspectRatio * 100) / 100.0f)
		{
			_cam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
			/*if (_backgroundCam)
			{
			    Destroy(_backgroundCam.gameObject);
			}*/

			return;
		}

		// 필러박스
		if (currentAspectRatio > _wantedAspectRatio)
		{
			float inset = 1.0f - _wantedAspectRatio / currentAspectRatio;
			Debug.Log("wanted: " + _wantedAspectRatio);
			Debug.Log("current: " + currentAspectRatio);
			Debug.Log("inset: " + inset);
			// _cam.rect = new Rect(inset / 2, 0.0f, 1.0f - inset, 1.0f);
			// _cam.rect = new Rect(inset / 2, 0.0f, (1.0f - inset) * 0.8f, 1.0f * 0.8f);
			_cam.rect = new Rect(inset / 2 * 0.8f, 0, (1.0f - inset) * 0.8f, 0.8f);
		}
		// 레터박스
		else
		{
			float inset = 1.0f - currentAspectRatio / _wantedAspectRatio;
			_cam.rect = new Rect(0.0f, inset / 2, 1.0f, 1.0f - inset);
		}

		/*if (_backgroundCam) return;
		// Make a new camera behind the normal camera which displays black; otherwise the unused space is undefined
		_backgroundCam = new GameObject("BackgroundCam", typeof(Camera)).GetComponent<Camera>();
		_backgroundCam.transform.Translate(new Vector3(0, 0, -10));
		_backgroundCam.orthographic = true;
		_backgroundCam.orthographicSize = 3;
		_backgroundCam.depth = int.MinValue;
		_backgroundCam.clearFlags = CameraClearFlags.SolidColor;
		_backgroundCam.backgroundColor = _cam.backgroundColor;
		var layer = new string[] {"UI"};
		_backgroundCam.cullingMask = LayerMask.GetMask(layer);*/
	}

	/*private void Update()
	{
	    if (_backgroundCam != null)
	    {
	        SetColor();
	    }
	}*/

	/*private static void SetColor()
	{
	    _backgroundCam.backgroundColor = _cam.backgroundColor;
	}*/
}