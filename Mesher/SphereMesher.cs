// Original source: https://github.com/alexisgea/sphere_mesher and post: https://www.alexisgiard.com/icosahedron-sphere/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexisGea {
	public enum SphereType {icosphere, uvsphere}

	/// <summary>
	/// Mesh generator script to be used as component on gameobjects.
	/// Resolution is how many subdivision should occur with 1 being the minimum.
	/// Drop on a game object, chose a sphere type, set resolution and radius then click generate.
	/// Poof! Magic happens.
	/// </summary>
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshFilter))]
	[DisallowMultipleComponent]
	public class SphereMesher : MonoBehaviour {
		[SerializeField] SphereType _sphereType = SphereType.icosphere;
        [SerializeField] float _radius = 0.5f;
        [SerializeField] int _resolution = 4;

        private bool _generating = false;
        private MeshData _sphereMesh = null;
		private MeshFilter _filter = null;
		private MeshRenderer _renderer = null;

		/// <summary>
		/// Starts the generation of a new mesh, erasing any previous mesh.
		/// </summary>
		public void GenerateMesh() {
			if (_generating) { return; }

			if(_filter == null) {
				_filter = GetComponent<MeshFilter>();
			}
			if(_renderer == null) {
				_renderer = GetComponent<MeshRenderer>();
			}

			_sphereMesh = null;
			_generating = true;

			if (Application.isPlaying) {
                System.Threading.ThreadPool.QueueUserWorkItem(GenerateSphereMeshThread);
            }
			else {
				GenerateSphereMeshThread(null);
				UpdateMesh();
			}
		}


		private void Reset() {
			Awake();
		}

        private void Awake() {
			_sphereMesh = null;
        	_generating = false;
            GenerateMesh();
        }

		private void Update() {
			if (_generating && _sphereMesh != null) {
				UpdateMesh();
			}
		}

		private void GenerateSphereMeshThread(object obj) {
			switch(_sphereType) {

				case SphereType.icosphere:
					_sphereMesh = IcoSphereBuilder.Generate(_radius, _resolution);
					break;
					
				case SphereType.uvsphere:
					_sphereMesh = UvSphereBuilder.Generate(_radius, _resolution);
					break;
			}
		}

		private void UpdateMesh() {
			if (!_filter.sharedMesh) {
				_filter.sharedMesh = new Mesh();
			}

			_filter.sharedMesh.Clear();
			_filter.sharedMesh.name = _sphereType.ToString();
			_filter.sharedMesh.vertices = _sphereMesh.Vertices;
			_filter.sharedMesh.triangles = _sphereMesh.Triangles;
			_filter.sharedMesh.uv = _sphereMesh.Uv;
			_filter.sharedMesh.normals = _sphereMesh.Normals;
			_filter.sharedMesh.tangents = _sphereMesh.Tangents;

			if(_renderer.sharedMaterial == null) {
				_renderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
			}

			_generating = false;
        }
    }	
}
