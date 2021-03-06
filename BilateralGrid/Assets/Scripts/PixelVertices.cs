﻿using UnityEngine;
using System.Collections;

public class PixelVertices : MonoBehaviour {
	public const string PROP_TEXTURE_TYPE = "_TexType";
	public const string PROP_EFFECT = "_Effect";
	public const string PROP_GAMMA = "_Gamma";

	public Texture2D debugSrc;
	public GameObject viewer;
	public Material bilateralGridMat;
	public Material resultMat;
	public Material webcamMat;
	public bool gaussianOn = true;
	public Material gaussianX;
	public Material gaussianY;
	public Material gaussianZ;
	public Vector3 sigma = new Vector3(16f, 16f, 0.07f);

	public Rect uiArea;

	private BilateralGrid _bg;
	private float _effect = 1f;

	// Use this for initialization
	IEnumerator Start () {
		var gamma = 1.0f;
		Texture src = debugSrc;
		if (src == null) {
			yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
			if (!Application.HasUserAuthorization(UserAuthorization.WebCam)) {
				Debug.Log("Failed to obtain a web camera");
				yield break;
			}

			var webCamTex = new WebCamTexture();
			src = webCamTex;
			webCamTex.Play();
			while (src.width <= 16)
				yield return 0;
			Debug.Log (string.Format("Texture size : ({0}x{1})", src.width, src.height));

			bilateralGridMat.SetInt(PROP_TEXTURE_TYPE, 1);
			resultMat.SetInt(PROP_TEXTURE_TYPE, 1);
			gamma = UnityEditor.PlayerSettings.colorSpace == ColorSpace.Linear ? 2.2f : 1.0f;
		}

		webcamMat.mainTexture = src;
		Debug.Log("Gamma : " + gamma);
		bilateralGridMat.SetFloat(PROP_GAMMA , gamma);
		resultMat.SetFloat(PROP_GAMMA, gamma);

		_bg = new BilateralGrid(src, sigma, gaussianX, gaussianY, gaussianZ);
		_bg.BindCamera(camera);
		_bg.BindGridMat(bilateralGridMat);
		_bg.BindResultMat(resultMat);
		_bg.BindViewer(viewer);
		var meshGen = GetComponentInChildren<MeshGenerator>();
		meshGen.Generate();
		yield break;
	}
	
	void OnPostRender() {
		if (_bg != null)
			_bg.OnPostRender(gaussianOn);
	}

	void OnGUI() {
		GUILayout.BeginArea(uiArea);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Effect:", GUILayout.Width(50));
		var tmpEffect = GUILayout.HorizontalSlider(_effect, 0f, 1f);
		if (tmpEffect != _effect)
			resultMat.SetFloat(PROP_EFFECT, tmpEffect);
		_effect = tmpEffect;
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	void OnDestroy() {
		if (_bg != null)
			_bg.Dispose();
	}
}
