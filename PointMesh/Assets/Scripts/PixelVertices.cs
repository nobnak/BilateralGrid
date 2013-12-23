using UnityEngine;
using System.Collections;

public class PixelVertices : MonoBehaviour {
	public const string PROP_TEXTURE_TYPE = "_TexType";

	public Texture2D debugSrc;
	public GameObject viewer;
	public Material bilateralGridMat;
	public Material resultMat;
	public bool gaussianOn = true;
	public Material gaussianX;
	public Material gaussianY;
	public Material gaussianZ;
	public Vector3 sigma = new Vector3(16f, 16f, 0.07f);

	private BilateralGrid _bg;

	// Use this for initialization
	IEnumerator Start () {
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
			yield return 0;

			bilateralGridMat.SetInt(PROP_TEXTURE_TYPE, 1);
			resultMat.SetInt(PROP_TEXTURE_TYPE, 1);
		}

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

	void OnDestroy() {
		if (_bg != null)
			_bg.Dispose();
	}
}
