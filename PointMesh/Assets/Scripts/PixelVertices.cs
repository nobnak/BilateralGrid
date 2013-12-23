using UnityEngine;
using System.Collections;

public class PixelVertices : MonoBehaviour {
	public const string PROP_GRID_SIZE = "_GridSize";
	public const string PROP_RCP_SIGMA = "_RcpSigma";
	public const string PROP_RCP_TILE = "_RcpTile";
	public const string PROP_BILATERAL_TEX = "_BilateralGridTex";

	public GameObject viewer;
	public Material bilateralGridMat;
	public Material resultMat;
	public bool gaussianOn = true;
	public Material gaussianX;
	public Material gaussianY;
	public Material gaussianZ;
	public Vector3 sigma = new Vector3(16f, 16f, 0.07f);

	private Mesh _mesh;
	private RenderTexture _bgTex;
	private RenderTexture _viewerTex;

	// Use this for initialization
	void Start () {
		var src = (Texture2D)bilateralGridMat.mainTexture;

		var imageWidth = src.width;
		var imageHeight = src.height;

		var gridWidth = (int)Mathf.Ceil(imageWidth / sigma.x);
		var gridHeight = (int)Mathf.Ceil(imageHeight / sigma.y);
		var gridDepth = (int)Mathf.Ceil(1f / sigma.z);
		
		var tileWidth = 3 + gridWidth;
		var tileDepth = 3 + gridDepth;
		var tileHeight = tileWidth * tileDepth;

		sigma = new Vector3(imageWidth / gridWidth, imageHeight / gridHeight, 1f / gridDepth);
		//var rcpSigma = new Vector4(1f / sigma.x, 1f / sigma.y, 1f / sigma.z, 0f);
		var gridSize = new Vector4(gridWidth, gridHeight, gridDepth, 0f);
		var rcpTile = new Vector4(1f / tileWidth, 1f / tileHeight, 1f / tileDepth, 0f);

		Debug.Log(string.Format("Grid {0}x{1}x{2}", gridWidth, gridHeight, gridDepth));
		Debug.Log(string.Format("Sigma {0}x{1}x{2}", sigma.x, sigma.y, sigma.z));
		Debug.Log(string.Format("Tile {0}x{1}x{2}", tileWidth, tileHeight, tileDepth));

		_bgTex = new RenderTexture(tileWidth, tileHeight, 0, RenderTextureFormat.ARGBFloat);
		_bgTex.filterMode = FilterMode.Point;
		_bgTex.useMipMap = false;
		camera.targetTexture = _bgTex;

		_viewerTex = new RenderTexture(_bgTex.width, _bgTex.height, _bgTex.depth, _bgTex.format);
		_viewerTex.filterMode = FilterMode.Bilinear;
		_viewerTex.useMipMap = _bgTex.useMipMap;

		viewer.renderer.sharedMaterial.mainTexture = _viewerTex;
		viewer.transform.localScale = new Vector3(tileWidth, tileHeight, 1f);

		bilateralGridMat.SetVector(PROP_GRID_SIZE, gridSize);
		//bilateralGridMat.SetVector(PROP_RCP_SIGMA, rcpSigma);
		bilateralGridMat.SetVector(PROP_RCP_TILE, rcpTile);
		gaussianZ.SetVector(PROP_RCP_TILE, rcpTile);
		resultMat.SetVector(PROP_GRID_SIZE, gridSize);
		//resultMat.SetVector(PROP_RCP_SIGMA, rcpSigma);
		resultMat.SetVector(PROP_RCP_TILE, rcpTile);
        resultMat.SetTexture(PROP_BILATERAL_TEX, _viewerTex);
	}
	
	void OnPostRender() {
		if (!gaussianOn) {
			Graphics.Blit(_bgTex, _viewerTex);
			return;
		}

		var tmp0 = RenderTexture.GetTemporary(_bgTex.width, _bgTex.height, 0, _bgTex.format);
		var tmp1 = RenderTexture.GetTemporary(_bgTex.width, _bgTex.height, 0, _bgTex.format);
		try {
#if true
			Graphics.Blit(_bgTex, tmp0, gaussianX);
			Graphics.Blit(tmp0, tmp1, gaussianY);
			Graphics.Blit(tmp1, _viewerTex, gaussianZ);
#else
			Graphics.Blit(_bgTex, _viewerTex, gaussianZ);
#endif
		} finally {
			RenderTexture.ReleaseTemporary(tmp0);
			RenderTexture.ReleaseTemporary(tmp1);
		}
	}

	void OnDestroy() {
		Destroy(_bgTex);
		Destroy(_viewerTex);
	}
}
