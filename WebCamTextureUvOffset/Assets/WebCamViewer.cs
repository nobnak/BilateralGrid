using UnityEngine;
using System.Collections;

public class WebCamViewer : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
		var src = new WebCamTexture();
		src.Play();
		yield return 0;
		renderer.sharedMaterial.mainTexture = src;
		var screenAspect = (float)Screen.width / Screen.height;
		transform.localScale = new Vector3(screenAspect, screenAspect * src.height / src.width, 1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
