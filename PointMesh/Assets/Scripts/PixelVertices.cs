using UnityEngine;
using System.Collections;

public class PixelVertices : MonoBehaviour {
	public Texture2D src;

	private Mesh _mesh;

	// Use this for initialization
	void Start () {
		var width = src.width;
		var height = src.height;

		var vertices = new Vector3[width * height];
		var indices = new int[width * height];
		var colors = src.GetPixels();

		for (var y = 0; y < height; y++) {
			for (var x = 0; x < width; x++) {
				var index = x + y * width;
				vertices[index] = new Vector3(x, y, 0);
				indices[index] = index;
			}
		}

		_mesh = new Mesh();
		_mesh.vertices = vertices;
		_mesh.colors = colors;
		_mesh.SetIndices(indices, MeshTopology.Points, 0);
		_mesh.bounds = new Bounds(new Vector3(width * 0.5f, height * 0.5f, 0f), new Vector3(width, height, 0));

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
