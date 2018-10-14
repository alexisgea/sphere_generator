using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexisGea {
	public interface IPlatonicSolid {
			List<Vector3> Vertices { get; }
			List<TriangleFace> Faces { get; }
			Vector3 NorthPole { get; }

			List<Vector3> RemapVertices(List<Vector3> vertices, List<TriangleFace> faces);
	}
}
