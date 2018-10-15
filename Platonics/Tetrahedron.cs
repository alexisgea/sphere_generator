// Original source: https://github.com/alexisgea/sphere_generator and post: https://www.alexisgiard.com/icosahedron-sphere-remastered/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexisGea {
    public class Tetrahedron : IPlatonicSolid {
        public List<Vector3> Vertices { private set; get; }
        public List<TriangleFace> Faces { private set; get; }
		public Vector3 NorthPole { private set; get; }

        public Tetrahedron() {
			Vertices = CreateStartingVertices();
			Faces = CreateStartingFaces();
        }

		public List<Vector3> RemapVertices(List<Vector3> vertices, List<TriangleFace> faces) {
			return vertices;
		}

		private List<Vector3> CreateStartingVertices() {
			List<Vector3> startingVert = new List<Vector3>();

			NorthPole = Vector3.up;

			startingVert.Add(new Vector3(1f, 1f, 1f));
			startingVert.Add(new Vector3(1f, -1f, -1f));
			startingVert.Add(new Vector3(-1f, 1f, -1f));
			startingVert.Add(new Vector3(-1f, -1f, 1f));

			return startingVert;
		}

		private List<TriangleFace> CreateStartingFaces() {
			List<TriangleFace> startingFaces = new List<TriangleFace>();

			// top 1
			startingFaces.Add(new TriangleFace(0, 1, 2));
			// top 2
			startingFaces.Add(new TriangleFace(0, 2, 3));

			// bottom 1
			startingFaces.Add(new TriangleFace(1, 3, 2));
			// bottom 2
			startingFaces.Add(new TriangleFace(0, 3, 1));

			return startingFaces;
		}
    }
}
