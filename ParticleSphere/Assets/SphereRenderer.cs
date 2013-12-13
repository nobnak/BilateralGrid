using UnityEngine;
using System.Collections;

public class SphereRenderer : MonoBehaviour {
	public const float DEG2RAD = Mathf.PI / 180f;

	public Material normalFromDepth;

	private RenderTexture _depthTex;
	private RenderTexture _normalTex;

	void Start() {
		_depthTex = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Depth);
		_normalTex = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
		camera.targetTexture = _depthTex;

		UpdateFOV();
	}

	void Update() {
	}

	void OnRenderImage(RenderTexture src, RenderTexture dst) {
		Graphics.Blit(_depthTex, _normalTex, normalFromDepth);
		Graphics.Blit(_normalTex, dst);
	}

	void OnDestroy() {
		Destroy(_depthTex);
		Destroy(_normalTex);
	}

	void UpdateFOV () {
		var fovY = Mathf.Tan (camera.fieldOfView * DEG2RAD);
		var fovX = (float)Screen.width / Screen.height * fovY;
		normalFromDepth.SetVector ("_Fov", new Vector4 (fovX, fovY, 0, 0));
	}
}
