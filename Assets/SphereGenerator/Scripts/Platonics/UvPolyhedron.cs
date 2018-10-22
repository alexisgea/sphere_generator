// Original source: https://github.com/alexisgea/sphere_generator and post: https://www.alexisgiard.com/icosahedron-sphere-remastered/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexisGea {
    public class UvPolyhedron : IPlatonicSolid {
        public List<Vector3> Vertices { private set; get; }
        public List<TriangleFace> Faces { private set; get; }
		public Vector3 NorthPole { private set; get; }

        public UvPolyhedron() {
			Vertices = CreateStartingVertices();
			Faces = CreateStartingFaces();
        }

		public List<Vector3> RemapVertices(List<Vector3> vertices, List<TriangleFace> faces) {
			return vertices;
		}

		private List<Vector3> CreateStartingVertices() {
			List<Vector3> startingVert = new List<Vector3>();

			// a vertical slice is an hexagon
			float phi = Mathf.Sqrt(3f)/2f;

			NorthPole = Vector3.up;

			startingVert.Add(new Vector3(0f, 1f, 0));
			startingVert.Add(new Vector3(0f, -1f, 0f));

			startingVert.Add(new Vector3(phi, 0.5f, 0f));
			startingVert.Add(new Vector3(0f, 0.5f, -phi));
			startingVert.Add(new Vector3(-phi, 0.5f, 0f));
			startingVert.Add(new Vector3(0f, 0.5f, phi));

			startingVert.Add(new Vector3(phi, -0.5f, 0f));
			startingVert.Add(new Vector3(0f, -0.5f, -phi));
			startingVert.Add(new Vector3(-phi, -0.5f, 0f));
			startingVert.Add(new Vector3(0f, -0.5f, phi));

			return startingVert;
		}

		private List<TriangleFace> CreateStartingFaces() {
			List<TriangleFace> startingFaces = new List<TriangleFace>();

			// top
			startingFaces.Add(new TriangleFace(0, 2, 3));
			startingFaces.Add(new TriangleFace(0, 3, 4));
			startingFaces.Add(new TriangleFace(0, 4, 5));
			startingFaces.Add(new TriangleFace(0, 5, 2));

			// sides
			startingFaces.Add(new TriangleFace(3, 2, 6));
			startingFaces.Add(new TriangleFace(3, 6, 7));
			startingFaces.Add(new TriangleFace(4, 3, 7));
			startingFaces.Add(new TriangleFace(4, 7, 8));
			startingFaces.Add(new TriangleFace(5, 4, 8));
			startingFaces.Add(new TriangleFace(5, 8, 9));
			startingFaces.Add(new TriangleFace(2, 5, 9));
			startingFaces.Add(new TriangleFace(2, 9, 6));

			// bottom
			startingFaces.Add(new TriangleFace(1, 7, 6));
			startingFaces.Add(new TriangleFace(1, 8, 7));
			startingFaces.Add(new TriangleFace(1, 9, 8));
			startingFaces.Add(new TriangleFace(1, 6, 9));


			return startingFaces;
		}
    }
}
