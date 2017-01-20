using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour {


	public Image testButton;

	// Use this for initialization
	void Start () {
		UUIEventListener.Get (testButton.gameObject).onClick = jumpToScene1;
	}

	void jumpToScene1(PointerEventData eventData){
		SceneManager.LoadScene ("scene1");
	}
		
}
