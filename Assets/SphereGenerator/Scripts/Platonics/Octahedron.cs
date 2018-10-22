// Original source: https://github.com/alexisgea/sphere_generator and post: https://www.alexisgiard.com/icosahedron-sphere-remastered/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexisGea {
    public class Octahedron : IPlatonicSolid {
        public List<Vector3> Vertices { private set; get; }
        public List<TriangleFace> Faces { private set; get; }
		public Vector3 NorthPole { private set; get; }

        public Octahedron() {
			Vertices = CreateStartingVertices();
			Faces = CreateStartingFaces();
        }

		public List<Vector3> RemapVertices(List<Vector3> vertices, List<TriangleFace> faces) {
			return vertices;
		}

		private List<Vector3> CreateStartingVertices() {
			List<Vector3> startingVert = new List<Vector3>();

			NorthPole = Vector3.up;

			startingVert.Add(new Vector3(1f, 0f, 0f));
			startingVert.Add(new Vector3(-1f, 0f, 0f));
			startingVert.Add(new Vector3(0f, 1f, 0f));
			startingVert.Add(new Vector3(0f, -1f, 0f));
			startingVert.Add(new Vector3(0f, 0f, 1f));
			startingVert.Add(new Vector3(0f, 0f, -1f));

			return startingVert;
		}

		private List<TriangleFace> CreateStartingFaces() {
			List<TriangleFace> startingFaces = new List<TriangleFace>();

			startingFaces.Add(new TriangleFace(2, 0, 5));
			startingFaces.Add(new TriangleFace(2, 5, 1));
			startingFaces.Add(new TriangleFace(2, 1, 4));
			startingFaces.Add(new TriangleFace(2, 4, 0));

			startingFaces.Add(new TriangleFace(3, 5, 0));
			startingFaces.Add(new TriangleFace(3, 1, 5));
			startingFaces.Add(new TriangleFace(3, 4, 1));
			startingFaces.Add(new TriangleFace(3, 0, 4));

			return startingFaces;
		}
    }
}
