using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class MeshGene {
	[MenuItem("Assets/Custom/GenerateMesh")]
	public static void Gen() {
		var width = 512;
		var height = 512;

		var vertices = new Vector3[width * height];
		var indices = new int[width * height];
		var uvs = new Vector2[width * height];
		var texelSize = new Vector2 (1f / width, 1f / height);
		var texelOffset = 0.5f * texelSize;

		for (var y = 0; y < height; y++) {
			for (var x = 0; x < width; x++) {
				var index = x + y * width;
				vertices [index] = new Vector3 (x, y, 0f);
				indices [index] = index;
				uvs [index] = new Vector2 (Mathf.Clamp01 (x * texelSize.x), Mathf.Clamp01 (y * texelSize.y)) + texelOffset;
			}
		}

		var file = string.Format("Assets/Models/Generated/{0:d4}x{1:d4}.obj", width, height);
		using(var istream = new StreamWriter(File.OpenWrite(file))) {
			istream.WriteLine("g point vertex");

			foreach (var v in vertices)
				istream.WriteLine("v {0:e} {1:e} {2:e}", v.x, v.y, v.z);
			istream.WriteLine();

			foreach (var u in uvs)
				istream.WriteLine("vt {0:e} {1:e}", u.x, u.y);
			istream.WriteLine();

			for (var i = 0; i < vertices.Length; i++)
				istream.WriteLine("f {0:d}/{0:d}", i+1);
			istream.WriteLine();
		}
	}
}
