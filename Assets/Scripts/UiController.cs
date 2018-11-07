using System.Collections;
using System.Collections.Generic;
using Tobii.GameIntegration.Net;
using Tobii.Gaming;
using UnityEngine;

public class UiController : MonoBehaviour
{
	private GameObject _pointerSprite;
	// Use this for initialization
	private void Start ()
	{
		_pointerSprite = GameObject.Find("Pointer");
	}
	
	// Update is called once per frame
	private void Update ()
	{
		_pointerSprite.transform.position = TobiiAPI.GetGazePoint().Screen;

	}
}
