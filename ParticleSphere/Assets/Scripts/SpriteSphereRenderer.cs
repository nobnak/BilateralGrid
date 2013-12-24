using UnityEngine;
using System.Collections;

public class SpriteSphereRenderer : MonoBehaviour {
	public const float DEG2RAD = Mathf.PI / 180f;

	public Camera original;
	public Material normalMat;

	private RenderTexture _depthTex;
	private RenderTexture _normalTex;

	void Awake() {
		var width = Screen.width;
		var height = Screen.height;
		_depthTex = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.RFloat);
		_normalTex = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGBFloat);
	}

	void OnPreRender() {
		CloneCameraParam();
	}

	void OnPostRender() {
		RenderNormal();
	}

	void OnDestroy() {
		ClearTexture ();
	}

	void CloneCameraParam() {
		var clearFlag = camera.clearFlags;
		var backgroundColor = camera.backgroundColor;
		var cullingMask = camera.cullingMask;
		camera.CopyFrom(original);
		camera.clearFlags = clearFlag;
		camera.backgroundColor = backgroundColor;
		camera.cullingMask = cullingMask;
		camera.targetTexture = _depthTex;
	}

	void RenderNormal() {
		UpdateFOV();
		Graphics.Blit(_depthTex, _normalTex, normalMat);
	}

	void ClearTexture () {
		if (_depthTex != null)
			RenderTexture.ReleaseTemporary (_depthTex);
		_depthTex = null;
		if (_normalTex != null)
			RenderTexture.ReleaseTemporary(_normalTex);
		_normalTex = null;
	}

	void UpdateFOV () {
		var fovY = Mathf.Tan (0.5f * camera.fieldOfView * DEG2RAD);
		var fovX = (float)Screen.width / Screen.height * fovY;
		normalMat.SetVector ("_Fov", new Vector4 (fovX, fovY, 0, 0));
	}

	public Result GetResult() { return new Result(_depthTex, _normalTex); }

	public struct Result {
		public RenderTexture depth;
		public RenderTexture normal;

		public Result(RenderTexture depth, RenderTexture normal) {
			this.depth = depth;
			this.normal = normal;
		}
	}
}
