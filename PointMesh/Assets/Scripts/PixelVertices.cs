using UnityEngine;
using System.Collections;

public class PixelVertices : MonoBehaviour {
	public const string PROP_RPC_SIGMA = "_RcpSigma";
	public const string PROP_RPC_GRID = "_RcpGrid";
	public const string PROP_GRID = "_GridSize";

	public Vector3 sigma = new Vector3(16f, 16f, 0.07f);

	private Mesh _mesh;

	// Use this for initialization
	void Start () {
		var mat = renderer.sharedMaterial;

		var src = (Texture2D)mat.mainTexture;

		var width = src.width;
		var height = src.height;

		mat.SetVector(PROP_RPC_SIGMA, new Vector4(1f / sigma.x, 1f / sigma.y, 1f / sigma.z, 0f));
		var grid = new Vector4(width / sigma.x, height / sigma.y, Mathf.Ceil(1f / sigma.z), 0f);
		mat.SetVector(PROP_GRID, grid);
		mat.SetVector(PROP_RPC_GRID, new Vector4(1f / grid.x, 1f / grid.y, 1f / grid.z, 0f));
		
		var vertices = new Vector3[width * height];
		var indices = new int[width * height];
		var uvs = new Vector2[width * height];
		var colors = src.GetPixels();

		var texelSize = new Vector2(1f / width, 1f / height);
		var texelOffset = 0.5f * texelSize;

		for (var y = 0; y < height; y++) {
			for (var x = 0; x < width; x++) {
				var index = x + y * width;
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

		var mf = GetComponent<MeshFilter>();
		mf.sharedMesh = _mesh;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy() {
		Destroy(_mesh);
	}


}
