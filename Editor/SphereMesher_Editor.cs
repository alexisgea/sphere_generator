// Original source: https://github.com/alexisgea/sphere_mesher and post: https://www.alexisgiard.com/icosahedron-sphere/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AlexisGea {
	[CustomEditor(typeof(SphereMesher))]
	public class SphereMesher_Editor : Editor {
		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			if (GUILayout.Button("Update Mesh")) {
				((SphereMesher)target).GenerateMesh();
			}
    	}
	}
}
