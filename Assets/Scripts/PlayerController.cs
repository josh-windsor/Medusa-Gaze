using System.Collections;
using System.Collections.Generic;
using Tobii.Gaming;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

	private Vector2 _middlePoint;
	private Camera _mainCam;
	private int _hp = 50;
	private bool _takenDmg = false;
	private bool _stunned = false;
	private Renderer _myRenderer;
	private Text _hpText;
	private UiController _UIController;

	// Use this for initialization
	void Start ()
	{
		_middlePoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
		_mainCam = Camera.main;
		_myRenderer = GetComponent<Renderer>();
		_hpText = GameObject.Find("HpTxt").GetComponent<Text>();
		_hpText.text = "HP: " + _hp;
		_UIController = GameObject.Find("Canvas").GetComponent<UiController>();
	}

	// Update is called once per frame
	private void Update ()
	{
		Vector2 gazePoint = TobiiAPI.GetGazePoint().Screen;
		float xDif = (_middlePoint.x - gazePoint.x) * -1;
		float zDif = (_middlePoint.y - gazePoint.y) * -1;
		if (!_stunned || _takenDmg)
		{
			if (xDif < -200)
			{
				transform.position += new Vector3(xDif / 10000, 0, 0);
			}
			else if (xDif > 200)
			{
				transform.position += new Vector3(xDif / 10000, 0, 0);
			}
			if (zDif < -200)
			{
				transform.position += new Vector3(0, 0, zDif / 10000);
			}
			else if (zDif > 200)
			{
				transform.position += new Vector3(0, 0, zDif / 10000);
			}
		}
		Vector3 direction = new Vector3(xDif, 0, zDif);
		if (direction != Vector3.zero)
		{
			transform.rotation = Quaternion.Slerp(
				transform.rotation,
				Quaternion.LookRotation(direction),
				Time.deltaTime * 2
			);
		}
		_mainCam.transform.position = new Vector3(transform.position.x, 10, transform.position.z);
	}

	public void TakeDamage(int iDamage)
	{
		if (!_takenDmg)
		{
			_takenDmg = true;
			_hp -= iDamage;
			_hpText.text = "HP: " + _hp;
			StopAllCoroutines();
			_stunned = false;

			StartCoroutine(WaitForDmg());

			if (_hp <= 0)
			{
				_UIController.LoseGame();
			}
		}
	}

	private IEnumerator WaitForDmg()
	{
		_myRenderer.material.color = Color.red;
		yield return new WaitForSeconds(0.5f);
		_myRenderer.material.color = Color.yellow;
		yield return new WaitForSeconds(2.5f);
		_takenDmg = false;
		_myRenderer.material.color = Color.white;
	}

	public IEnumerator StunPlayer()
	{
		if (!_stunned)
		{
			_stunned = true;
			_myRenderer.material.color = Color.cyan;
			yield return new WaitForSeconds(3);
			_myRenderer.material.color = Color.white;
			_stunned = false;
		}
	}
}
