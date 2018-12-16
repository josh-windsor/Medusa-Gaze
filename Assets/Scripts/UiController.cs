using Tobii.Gaming;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
	private GameObject _pointerSprite;
	
	[SerializeField]
	private GameObject _level;

	private static GameObject _currentLevel;

	private static GameObject _panel;

	private static Image _startBtn;

	private static bool _gameRunning = false;
	// Use this for initialization
	private void Start ()
	{
		_pointerSprite = GameObject.Find("Pointer");
		_currentLevel = Instantiate(_level);
		_currentLevel.SetActive(false);
		_panel = GameObject.Find("Panel");
		_startBtn = GameObject.Find("StartBtn").GetComponent<Image>();
	}

	// Update is called once per frame
	private void Update ()
	{
		Vector2 gazePoint = TobiiAPI.GetGazePoint().Screen;

		#if UNITY_EDITOR
			_pointerSprite.transform.position = gazePoint;
		#endif

		if (!_gameRunning)
		{
			if (Vector2.Distance(gazePoint, _startBtn.transform.position) < 250f)
			{
				Color c = _startBtn.color;
				if (c.a < 0.01f)
				{
					_gameRunning = true;
					_currentLevel.SetActive(true);
					_panel.SetActive(false);

				}
				c.a = c.a - 0.01f;
				_startBtn.color = c;
			}
			else
			{
				Color c = _startBtn.color;
				c.a = 1;
				_startBtn.color = c;
			}
		}
	}

	public void EndGame()
	{
		_gameRunning = false;
		Destroy(_currentLevel);
		_currentLevel = Instantiate(_level);
		_currentLevel.SetActive(false);
		_panel.SetActive(true);
		Color c = _startBtn.material.color;
		c.a = 1;
		_startBtn.material.color = c;

	}
}
