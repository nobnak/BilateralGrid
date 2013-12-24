using UnityEngine;
using System.Collections;

public class BilateralGrid : System.IDisposable {
	public const string PROP_GRID_SIZE = "_GridSize";
	public const string PROP_RCP_SIGMA = "_RcpSigma";
	public const string PROP_RCP_TILE = "_RcpTile";
	public const string PROP_BILATERAL_TEX = "_BilateralGridTex";

	private Material _gaussianX;
	private Material _gaussianY;
	private Material _gaussianZ;

	private Texture _src;
	private RenderTexture _bgTex;
	private RenderTexture _gaussTex;

	private int _tileWidth;
	private int _tileDepth;
	private int _tileHeight;
	private Vector3 _gridSize;
	private Vector3 _rcpTile;

	private bool _disposed = false;

	public BilateralGrid(Texture src, Vector3 sigma, Material gaussianX, Material gaussianY, Material gaussianZ) {
		this._src = src;
		this._gaussianX = gaussianX;
		this._gaussianY = gaussianY;
		this._gaussianZ = gaussianZ;

		var imageWidth = src.width;
		var imageHeight = src.height;

		var gridWidth = (int)Mathf.Ceil(imageWidth / sigma.x);
		var gridHeight = (int)Mathf.Ceil(imageHeight / sigma.y);
		var gridDepth = (int)Mathf.Ceil(1f / sigma.z);
		
		_tileWidth = 3 + gridWidth;
		_tileDepth = 3 + gridDepth;
		_tileHeight = _tileWidth * _tileDepth;

		sigma = new Vector3(imageWidth / gridWidth, imageHeight / gridHeight, 1f / gridDepth);
		_gridSize = new Vector4(gridWidth, gridHeight, gridDepth, 0f);
		_rcpTile = new Vector4(1f / _tileWidth, 1f / _tileHeight, 1f / _tileDepth, 0f);

		Debug.Log(string.Format("Grid {0}x{1}x{2}", gridWidth, gridHeight, gridDepth));
		Debug.Log(string.Format("Sigma {0}x{1}x{2}", sigma.x, sigma.y, sigma.z));
		Debug.Log(string.Format("Tile {0}x{1}x{2}", _tileWidth, _tileHeight, _tileDepth));

		_bgTex = new RenderTexture(_tileWidth, _tileHeight, 0, RenderTextureFormat.ARGBFloat);
		_bgTex.filterMode = FilterMode.Point;
		_bgTex.useMipMap = false;

		_gaussTex = new RenderTexture(_bgTex.width, _bgTex.height, _bgTex.depth, _bgTex.format);
		_gaussTex.filterMode = FilterMode.Bilinear;
		_gaussTex.useMipMap = _bgTex.useMipMap;

		gaussianZ.SetVector(PROP_RCP_TILE, _rcpTile);
	}

	public void BindCamera(Camera camera) {
		camera.targetTexture = _bgTex;
	}
	public void BindViewer(GameObject viewer) {
		viewer.renderer.sharedMaterial.mainTexture = _gaussTex;
		viewer.transform.localScale = new Vector3(_tileWidth, _tileHeight, 1f);
	}
	public void BindGridMat(Material bilateralGridMat) {
		bilateralGridMat.mainTexture = _src;
		bilateralGridMat.SetVector(PROP_GRID_SIZE, _gridSize);
		bilateralGridMat.SetVector(PROP_RCP_TILE, _rcpTile);
	}
	public void BindResultMat(Material resultMat) {
		resultMat.mainTexture = _src;
		resultMat.SetVector(PROP_GRID_SIZE, _gridSize);
		resultMat.SetVector(PROP_RCP_TILE, _rcpTile);
        resultMat.SetTexture(PROP_BILATERAL_TEX, _gaussTex);
	}
	
	public void OnPostRender(bool gaussianOn) {
		if (!gaussianOn) {
			Graphics.Blit(_bgTex, _gaussTex);
			return;
		}

		var tmp0 = RenderTexture.GetTemporary(_bgTex.width, _bgTex.height, 0, _bgTex.format);
		var tmp1 = RenderTexture.GetTemporary(_bgTex.width, _bgTex.height, 0, _bgTex.format);
		try {
#if true
			Graphics.Blit(_bgTex, tmp0, _gaussianX);
			Graphics.Blit(tmp0, tmp1, _gaussianY);
			Graphics.Blit(tmp1, _gaussTex, _gaussianZ);
#else
			Graphics.Blit(_bgTex, _viewerTex, gaussianZ);
#endif
		} finally {
			RenderTexture.ReleaseTemporary(tmp0);
			RenderTexture.ReleaseTemporary(tmp1);
		}
	}

	public void Dispose() {
		if (_disposed)
			return;
		_disposed = true;

		GameObject.Destroy(_bgTex);
		GameObject.Destroy(_gaussTex);
	}
}
