using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DepthNormal : MonoBehaviour {
	public const float DEG2RAG = Mathf.PI / 180f;

	public Material normalFromDepth;

	private RenderTexture _depthTex;
	private GameObject _depthCameraObj;

	void OnPreRender() {
		if (!enabled || !gameObject.activeSelf) 
			return;

		if (_depthTex == null) {
			_depthTex = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.RFloat);
		}
		if (_depthCameraObj == null) {
			_depthCameraObj = new GameObject("DepthCamera");
			_depthCameraObj.hideFlags = HideFlags.HideAndDontSave;
			_depthCameraObj.AddComponent<Camera>();
			_depthCameraObj.camera.enabled = false;
		}

		var dcam = _depthCameraObj.camera;
		dcam.CopyFrom(camera);
		dcam.backgroundColor = Color.black;
		dcam.clearFlags = CameraClearFlags.SolidColor;
		dcam.targetTexture = _depthTex;
		dcam.Render();
	}

	void OnRenderImage(RenderTexture src, RenderTexture dst) {
		UpdateFov ();
		Graphics.Blit(_depthTex, dst, normalFromDepth);
	}

	void OnDisable() {
		DestroyImmediate(_depthCameraObj);
		DestroyImmediate(_depthTex);
	}

	void UpdateFov () {
		var fovy = Mathf.Tan (camera.fieldOfView * DEG2RAG);
		var fovx = fovy * (float)Screen.width / Screen.height;
		var fov = new Vector4 (fovx, fovy, 0f, 0f);
		normalFromDepth.SetVector ("_Fov", fov);
	}
}
