using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class GlPointSize : MonoBehaviour {
	public const System.UInt32 GL_VERTEX_PROGRAM_POINT_SIZE = 0x8642;
	public const string libGL = "/System/Library/Frameworks/OpenGL.framework/OpenGL";

	public bool pointSizeEnabled = true;

	void Start() {
		if (!pointSizeEnabled)
			Destroy(this);
	}

	void OnPreRender() {
		glEnable(GL_VERTEX_PROGRAM_POINT_SIZE);
	}

	[DllImport(libGL)]
	public static extern void glEnable(System.UInt32 flag);
}
