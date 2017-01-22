using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour {

	public Sprite bg_start_Sprite;
	public Sprite bg_start_ipad_Sprite;


	public Image startImage;
	public Image backgroundImage;

	private float screenScale = 0;
	// Use this for initialization
	void Start () {
		UUIEventListener.Get (startImage.gameObject).onClick = jumpToScene1;
	}

	void Update(){
		if (screenScale != (float)Screen.height / (float)Screen.width) {
			screenScale = (float)Screen.height / (float)Screen.width;
			if (screenScale > 1.5f) {
				backgroundImage.sprite = bg_start_Sprite;
			} else {
				backgroundImage.sprite = bg_start_ipad_Sprite;
			}
		}
	}


	void jumpToScene1(PointerEventData eventData){
		SceneManager.LoadScene ("game");
//		Screen.SetResolution(750,1333,false);
	}
		
}
