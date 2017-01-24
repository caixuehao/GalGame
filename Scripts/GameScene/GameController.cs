﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameController : MonoBehaviour {

	public Image roleImage;

	public Image backgroundImage;
	public Image textBackgroundImage;

	public ScrollRect scrollView;
	public Text text;

	public Text speedUpText;
	public Image	nextImage;


	public Image optionBackgroundImage;
	public GameObject optionGameObject;

	private float screenScale = 0;
	private float gameScale = 1200.0f / 750.0f;
	private float currentSpeed = 1.0f;
	private float textSpacetime = 0.1f;

	private string currentText;
	//游戏事件数据
	public TextAsset GameJSON;
	public int startTag;
	public GameEventEntity[] eventList;

	public int currentEventTag;
	public GameEventEntity currentGameEvent;
	public int currentTextIndex;


	void Start () {
		//Screen.SetResolution(750,1333,false);
		//PlayerSettings.SetAspectRatio(AspectRatio.Aspect16by9,true);没用

		UUIEventListener.Get (speedUpText.gameObject).onClick = speedUp;
		UUIEventListener.Get (nextImage.gameObject).onClick = next;
		UUIEventListener.Get (backgroundImage.gameObject).onClick = next;
		UUIEventListener.Get (textBackgroundImage.gameObject).onClick = next;
		UUIEventListener.Get (roleImage.gameObject).onClick = next;

		UUIEventListener.Get (speedUpText.gameObject).onClick = speedUp;
		JsonUtility.FromJsonOverwrite (GameJSON.text, this);
		setCurrentTag (startTag);

		optionBackgroundImage.gameObject.SetActive (false);
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
		CancelInvoke ("changeText");
		text.text = currentText;
		nextEvent ();
	}

	void optionClick(int tag_){
		Debug.Log ("选择了选项" + tag_);
		setCurrentTag (tag_);
		for (int i = 0; i < optionBackgroundImage.transform.childCount; i++) {  
			Destroy (optionBackgroundImage.transform.GetChild (i).gameObject);  
		}  
		optionBackgroundImage.gameObject.SetActive (false);
	}

	//下一个事件
	void nextEvent(){
		if (optionBackgroundImage.IsActive())return;

		if (++currentTextIndex >= currentGameEvent.textList.Length) {
			if (currentGameEvent.type == GameEventEntity.EventType.End) {
				Debug.Log ("显示结局");
				PlayerPrefs.SetInt ("endTag", currentGameEvent.endTag);
				SceneManager.LoadScene ("end");

			} else if(currentGameEvent.type == GameEventEntity.EventType.Option){
				Debug.Log ("显示选项");
				optionBackgroundImage.gameObject.SetActive (true);
				addOptions ();

			} else if(currentGameEvent.type == GameEventEntity.EventType.NotOption){
				Debug.Log ("下一个事件");
				setCurrentTag (currentGameEvent.nextTag);
			}
		}else{
			setText (currentGameEvent.textList [currentTextIndex]);
		}
	}

	//设置当前的事件
	public void setCurrentTag(int tag){

		currentEventTag = tag;
		foreach(GameEventEntity gameEvent in eventList){
			if (gameEvent.tag == currentEventTag) {
				currentGameEvent = gameEvent;
				//背景
				Texture2D texture = Resources.Load( ("Images/GameScene/background/" + gameEvent.background)) as Texture2D;
				backgroundImage.sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), Vector2.zero);
				//人物
				if(gameEvent.characters == null || gameEvent.characters.Length == 0){
					roleImage.sprite = null;
					roleImage.color = new Color (255, 255, 255, 0);
				}else if(gameEvent.characters.Length==1){
					roleImage.color = new Color (255, 255, 255, 255);
					texture = Resources.Load( ("Images/GameScene/role/" + gameEvent.characters[0])) as Texture2D;
					roleImage.sprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), Vector2.zero);
				}
				//对话
				currentTextIndex = 0;
				if(gameEvent.textList.Length>currentTextIndex)setText(gameEvent.textList[0]);

				return;
			}
		}
		Debug.Log ("没有事件" + tag);
	}


	//添加选项
	void addOptions(){
		for (int i = 0; i < currentGameEvent.options.Length; i++) {
			GameObject option = MonoBehaviour.Instantiate (optionGameObject);
			option.name = string.Format ("option{0}", i);
			option.transform.parent = optionBackgroundImage.gameObject.transform;
		
			RectTransform rectTransform = option.GetComponent<RectTransform> ();
			rectTransform.anchorMin =  Vector2.one * 0.5f;
			rectTransform.anchorMax =  Vector2.one * 0.5f;
			float width = backgroundImage.rectTransform.sizeDelta.x *0.75f;
			float height = width / 5.0f;
			rectTransform.sizeDelta = new Vector2 (width, height);
			rectTransform.pivot = Vector2.one * 0.5f;
			rectTransform.localScale = Vector3.one;
			float posY = (currentGameEvent.options.Length - 1) * height / 2.0f - (height * (float)i);
			Debug.Log (width+","+height+","+posY);
			rectTransform.anchoredPosition3D = new Vector3 ( 0,posY, 0);

			Text optionText =  rectTransform.FindChild ("text").gameObject.GetComponent<Text>();
			if (optionText != null) {
				optionText.text = currentGameEvent.options [i].text;
				UUIEventListener.Get (option,currentGameEvent.options [i].tag).onClickTag = optionClick;
			} 
			
		}
	}

	//对话框
	public void setText(string str){
		CancelInvoke ("changeText");
		currentText = str;
		text.text = "";
		changeText ();
	}

	void changeText(){
		if (text.text.Length == currentText.Length) {
			return;
		}
		text.text = currentText.Substring (0, text.text.Length + 1);

		Invoke ("changeText", textSpacetime/currentSpeed);
	}







}
