using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


[System.Serializable]
public class GameController : MonoBehaviour {

	public Image roleImage;

	public Image backgroundImage;
	public Image textBackgroundImage;

	public ScrollRect scrollView;
	public Text text;

	public Text speedUpText;
	public Image	nextImage;

	private float screenScale = 0;
	private float gameScale = 1200.0f / 750.0f;
	private float currentSpeed = 1.0f;

	private string currentText;
	//游戏事件数据
	public TextAsset GameJSON;
	public int startTag;
	public GameEventEntity[] eventList;

	public int currentEventTag;

	void Start () {
		//Screen.SetResolution(750,1333,false);
		//PlayerSettings.SetAspectRatio(AspectRatio.Aspect16by9,true);没用
		UUIEventListener.Get (speedUpText.gameObject).onClick = speedUp;
		//setText (text.text);
		JsonUtility.FromJsonOverwrite (GameJSON.text, this);

	}

	void Update () {
		if (screenScale != (float)Screen.height / (float)Screen.width) {
			screenScale = (float)Screen.height / (float)Screen.width;


			textBackgroundImage.rectTransform.sizeDelta = new Vector2 (0, (float)Screen.height / 3.0f);

			if (screenScale > gameScale) {
				backgroundImage.rectTransform.sizeDelta = new Vector2 ((float)Screen.width,(float)Screen.width*gameScale); 
			} else {
				backgroundImage.rectTransform.sizeDelta = new Vector2 ((float)Screen.height/gameScale,(float)Screen.height); 
			}

			float rolewidth = backgroundImage.rectTransform.sizeDelta.x * 0.6f;
			roleImage.rectTransform.sizeDelta = new Vector2 (rolewidth,rolewidth*2.5f);
		}
	}


	void speedUp(PointerEventData eventData){
		if (currentSpeed == 1.0f) {
			currentSpeed = 2.0f;
			speedUpText.text = "加速×1";
		}
		else{
			currentSpeed = 1.0f;
			speedUpText.text = "加速";
		}
	}

	void next(PointerEventData eventData){
	
	}





	public void setCurrentTag(int tag){
		currentEventTag = tag;
		foreach(GameEventEntity gameEvent in eventList){
			if (gameEvent.tag == currentEventTag) {
				backgroundImage.sprite = Resources.Load ("Images/GameScene/background/" + gameEvent.background) as Sprite;
				return;
			}
		}
		Debug.Log ("没有事件" + tag);
	}

	public void setText(string str){
		currentText = str;
		text.text = "";
		changeText ();
	}

	void changeText(){
		if (text.text.Length == currentText.Length)return;
		text.text = currentText.Substring (0, text.text.Length + 1);

		Invoke ("changeText", 0.1f/currentSpeed);
	}







}
