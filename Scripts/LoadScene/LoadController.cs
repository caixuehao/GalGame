using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadController : MonoBehaviour {
	public Sprite[] loadingSpriteArr;
	public Image loadingImage;
	public float animationTime = 1.0f;//动画播放时间

	private int currentSpriteIndex = 0;
	// Use this for initialization
	void Start () {
		InvokeRepeating ("changeImage", 0, animationTime / loadingSpriteArr.Length);
		Invoke("jumpToStart",3);
	}
	

	void jumpToStart(){
		SceneManager.LoadScene ("start");
	}

	void changeImage(){
		if (++currentSpriteIndex >= loadingSpriteArr.Length)
			currentSpriteIndex = 0;

		loadingImage.sprite = loadingSpriteArr [currentSpriteIndex];
	}
}
