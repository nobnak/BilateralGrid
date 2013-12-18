using UnityEngine;
using System.Collections;

public class PixelVertices : MonoBehaviour {
	private Mesh _mesh;

	// Use this for initialization
	void Start () {
		var src = (Texture2D)renderer.sharedMaterial.mainTexture;

		var width = src.width;
		var height = src.height;

		var vertices = new Vector3[width * height];
		var indices = new int[width * height];
		var uvs = new Vector2[width * height];
		var colors = src.GetPixels();

		var texelSize = new Vector2(1f / width, 1f / height);
		var texelOffset = 0.5f * texelSize;

		for (var y = 0; y < height; y++) {
			for (var x = 0; x < width; x++) {
				var index = x + y * width;
				vertices[index] = new Vector3(x * texelSize.x, y * texelSize.y, 0f);
				indices[index] = index;
				uvs[index] = new Vector2(x * texelSize.x, y * texelSize.y) + texelOffset;
			}
		}

		_mesh = new Mesh();
		_mesh.vertices = vertices;
		_mesh.colors = colors;
		_mesh.uv = uvs;
		_mesh.SetIndices(indices, MeshTopology.Points, 0);
		_mesh.bounds = new Bounds(Vector3.zero, new Vector3(2f, 2f, 0f));

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
