﻿using UnityEngine;
using System.Collections;

public class PixelVertices : MonoBehaviour {
	public const string PROP_RPC_SIGMA = "_RcpSigma";
	public const string PROP_RPC_TILE = "_RcpTile";
	public const string PROP_GRID = "_GridSize";

	public GameObject target;
	public Vector3 sigma = new Vector3(16f, 16f, 0.07f);

	private Mesh _mesh;

	// Use this for initialization
	void Start () {
		var mat = target.renderer.sharedMaterial;

		var src = (Texture2D)mat.mainTexture;

		var imageWidth = src.width;
		var imageHeight = src.height;

		var gridWidth = (int)Mathf.Ceil(imageWidth / sigma.x);
		var gridHeight = (int)Mathf.Ceil(imageHeight / sigma.y);
		var gridDepth = (int)Mathf.Ceil(1f / sigma.z);
		
		sigma = new Vector3(imageWidth / gridWidth, imageHeight / gridHeight, 1f / gridDepth);

		var tileWidth = 2 + gridWidth;
		var tileDepth = 2 + gridDepth;
		var tileHeight = tileWidth * tileDepth;

		Debug.Log(string.Format("Grid {0}x{1}x{2}", gridWidth, gridHeight, gridDepth));
		Debug.Log(string.Format("Sigma {0}x{1}x{2}", sigma.x, sigma.y, sigma.z));
		Debug.Log(string.Format("Tile {0}x{1}x{2}", tileWidth, tileHeight, tileDepth));

		mat.SetVector(PROP_RPC_SIGMA, new Vector4(1f / sigma.x, 1f / sigma.y, 1f / sigma.z, 0f));
		mat.SetVector(PROP_RPC_TILE, new Vector4(1f / tileWidth, 1f / tileHeight, 1f / tileDepth, 0f));
		
		var vertices = new Vector3[imageWidth * imageHeight];
		var indices = new int[imageWidth * imageHeight];
		var uvs = new Vector2[imageWidth * imageHeight];
		var colors = src.GetPixels();

		var texelSize = new Vector2(1f / imageWidth, 1f / imageHeight);
		var texelOffset = 0.5f * texelSize;

		for (var y = 0; y < imageHeight; y++) {
			for (var x = 0; x < imageWidth; x++) {
				var index = x + y * imageWidth;
				vertices[index] = new Vector3(x, y, 0f);
				indices[index] = index;
				uvs[index] = new Vector2(Mathf.Clamp01(x * texelSize.x), Mathf.Clamp01(y * texelSize.y)) + texelOffset;
			}
		}

		_mesh = new Mesh();
		_mesh.vertices = vertices;
		_mesh.colors = colors;
		_mesh.uv = uvs;
		_mesh.SetIndices(indices, MeshTopology.Points, 0);
		_mesh.bounds = new Bounds(Vector3.zero, Mathf.Infinity * Vector3.one);

		var mf = target.GetComponent<MeshFilter>();
		mf.sharedMesh = _mesh;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy() {
		Destroy(_mesh);
	}


}
