using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndController : MonoBehaviour
{

    private UiController _controller;

    private void Awake()
    {
        _controller = GameObject.Find("Canvas").GetComponent<UiController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            _controller.EndGame();
        }
    }
}
