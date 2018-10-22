// Original source: https://github.com/alexisgea/sphere_generator and post: https://www.alexisgiard.com/icosahedron-sphere-remastered/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AlexisGea {
	[CustomEditor(typeof(SphereGenerator))]
	public class SphereGenerator_Editor : Editor {
		
		private SphereGenerator sphere;
		private bool _resolutionSafety = true;


		public override void OnInspectorGUI() {
			sphere.SphereType = (SphereType)EditorGUILayout.EnumPopup(new GUIContent("Sphere Type",
			"The base solid to use for generating the sphere."
			+"The UV Sphere works differently and only the Radius and Resolution are used."), sphere.SphereType);

			sphere.Radius = EditorGUILayout.FloatField(new GUIContent("Radius",
			"How big is the sphere. 0.5 gives the same size as a Unity Sphere."), sphere.Radius);

			sphere.Resolution = EditorGUILayout.IntField(new GUIContent("Resolution", "How many subdivision should be done."
			+ "Warning, this is exponential and can be very CPU intensive."), sphere.Resolution);

			sphere.Smooth = EditorGUILayout.Toggle(new GUIContent("Smooth", "Smoothness of the sphere."
			+"Smooth takes much less vertices than otherwise and is faster to generate."), sphere.Smooth);

			sphere.RemapVertices = EditorGUILayout.Toggle(new GUIContent("Remap Vertices",
			"In some case the vertices are not evenly spread. This forces a recompute of the vertices' position."
			+ "At the moment I only found an algorythm for the cube."), sphere.RemapVertices);

			if (GUILayout.Button("Update Mesh")) {
				((SphereGenerator)target).GenerateMesh();
			}

			// safety checks
			_resolutionSafety = EditorGUILayout.Toggle(new GUIContent("Resolution Safety",
			"Above resolution 6, it can take a while to generate a sphere and even crash your computer. Be warned."
			+ "The resolution is maxed at 5 in case on non-smooth object."), _resolutionSafety);

			if(sphere.Radius < 0) {
				sphere.Radius = 0.5f;
			}

			if(sphere.Resolution < 1) {
				sphere.Resolution = 1;
			}

			if(_resolutionSafety && !sphere.Smooth && sphere.Resolution > 5) {
				sphere.Resolution = 5;
			}
			else if(_resolutionSafety && sphere.Smooth && sphere.Resolution > 6) {
				sphere.Resolution = 6;
			}
    	}

		private void OnEnable() {
			sphere = (SphereGenerator)target;
		}
	}
}
