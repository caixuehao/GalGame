using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EndController : MonoBehaviour {

	public Image CGImage;
	public Image downloadCGImage;
	public Image replayImage;
	public Image shareImage;


	private float screenScale = 0;
	private float gameScale = 1200.0f / 750.0f;
	// Use this for initialization
	void Start () {
		Debug.Log(PlayerPrefs.GetInt("endTag"));
		UUIEventListener.Get (replayImage.gameObject).onClick = rePlay;
	}

	void rePlay(PointerEventData eventData){
		SceneManager.LoadScene ("start");
	}
	// Update is called once per frame
	void Update () {
		if (screenScale != (float)Screen.height / (float)Screen.width) {
			screenScale = (float)Screen.height / (float)Screen.width;

			if (screenScale > gameScale) {
				CGImage.rectTransform.sizeDelta = new Vector2 ((float)Screen.width,(float)Screen.width*gameScale); 
			} else {
				CGImage.rectTransform.sizeDelta = new Vector2 ((float)Screen.height/gameScale,(float)Screen.height); 
			}
			float width = CGImage.rectTransform.sizeDelta.x * 0.2f;
			downloadCGImage.rectTransform.sizeDelta = new Vector2 (width, width * 1.5f);
			replayImage.rectTransform.sizeDelta = new Vector2 (width, width * 1.5f);
			shareImage.rectTransform.sizeDelta = new Vector2 (width, width * 1.5f);
		}
	}
}
