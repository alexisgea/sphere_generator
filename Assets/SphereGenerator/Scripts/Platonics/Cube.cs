// Original source: https://github.com/alexisgea/sphere_generator and post: https://www.alexisgiard.com/icosahedron-sphere-remastered/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexisGea {
    public class Cube : IPlatonicSolid {
        public List<Vector3> Vertices { private set; get; }
        public List<TriangleFace> Faces { private set; get; }
		public Vector3 NorthPole { private set; get; }

        public Cube() {
			Vertices = CreateStartingVertices();
			Faces = CreateStartingFaces();
        }

		public List<Vector3> RemapVertices(List<Vector3> vertices, List<TriangleFace> faces) {
			for(int i = 0; i < vertices.Count; i++) {

				Vector3 v = vertices[i];
				float x2 = v.x * v.x;
				float y2 = v.y * v.y;
				float z2 = v.z * v.z;
				Vector3 s;
				s.x = v.x * Mathf.Sqrt(1f - y2 / 2f - z2 / 2f + y2 * z2 / 3f);
				s.y = v.y * Mathf.Sqrt(1f - x2 / 2f - z2 / 2f + x2 * z2 / 3f);
				s.z = v.z * Mathf.Sqrt(1f - x2 / 2f - y2 / 2f + x2 * y2 / 3f);

				vertices[i] = s;
			}
			return vertices;
		}

		private List<Vector3> CreateStartingVertices() {
			List<Vector3> startingVert = new List<Vector3>();

			NorthPole = Vector3.up;

			startingVert.Add(new Vector3(-1f, 1f, -1f));
			startingVert.Add(new Vector3(-1f, 1f, 1f));
			startingVert.Add(new Vector3(1f, 1f, 1f));
			startingVert.Add(new Vector3(1f, 1f, -1f));

			startingVert.Add(new Vector3(-1f, -1f, -1f));
			startingVert.Add(new Vector3(-1f, -1f, 1f));
			startingVert.Add(new Vector3(1f, -1f, 1f));
			startingVert.Add(new Vector3(1f, -1f, -1f));

			return startingVert;
		}

		private List<TriangleFace> CreateStartingFaces() {
			List<TriangleFace> startingFaces = new List<TriangleFace>();

			// Y pos face (top)
			startingFaces.Add(new TriangleFace(3, 0, 1));
			startingFaces.Add(new TriangleFace(3, 1, 2));
			//Y neg face (bottom)
			startingFaces.Add(new TriangleFace(7, 5, 4));
			startingFaces.Add(new TriangleFace(7, 6, 5));

			// Z neg face (front)
			startingFaces.Add(new TriangleFace(3, 4, 0));
			startingFaces.Add(new TriangleFace(3, 7, 4));
			// Z pos face (back)
			startingFaces.Add(new TriangleFace(1, 5, 6));
			startingFaces.Add(new TriangleFace(1, 6, 2));

			// X pos face (right)
			startingFaces.Add(new TriangleFace(2, 7, 3));
			startingFaces.Add(new TriangleFace(2, 6, 7));
			// X neg face (left)
			startingFaces.Add(new TriangleFace(0, 5, 1));
			startingFaces.Add(new TriangleFace(0, 4, 5));

			return startingFaces;
		}
    }
}
