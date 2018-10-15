// Original source: https://github.com/alexisgea/sphere_generator and post: https://www.alexisgiard.com/icosahedron-sphere-remastered/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexisGea {
	public enum SphereType {tetrasphere, cubesphere, octasphere, dodecasphere, icosphere, uvsphere}

	/// <summary>
	/// Mesh generator script to be used as component on gameobjects.
	/// Resolution is how many subdivision should occur with 1 being the minimum.
	/// Drop on a game object, chose a sphere type, set resolution and radius then click generate.
	/// Poof! Magic happens.
	/// </summary>
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshFilter))]
	[DisallowMultipleComponent]
	public class SphereGenerator : MonoBehaviour {

		[HideInInspector] public SphereType SphereType = SphereType.icosphere;
		[HideInInspector] public float Radius = 0.5f;
		[HideInInspector] public int Resolution = 4;
		[HideInInspector] public bool Smooth = true;
		[HideInInspector] public bool RemapVertices = false;


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
			if(SphereType == SphereType.uvsphere) {
				_sphereMesh = UvSphereBuilder.Generate(Radius, Resolution);
			}
			else {
				IPlatonicSolid baseSolid = GetBaseSolid(SphereType);
				_sphereMesh = SphereBuilder.Build(baseSolid, Radius, Resolution, Smooth, RemapVertices);
			}
			Debug.Log(SphereType.ToString() + " generated: " + _sphereMesh.Triangles.Length + " tris and " + _sphereMesh.Vertices.Length + " verts.");
		}

		private IPlatonicSolid GetBaseSolid(SphereType type) {
			switch(type) {
				case SphereType.tetrasphere:
					return new Tetrahedron();

				case SphereType.cubesphere:
					return new Cube();

				case SphereType.octasphere:
					return new Octahedron();

				case SphereType.dodecasphere:
					return new Dodecahedron();

				case SphereType.icosphere:
					return new Icosahedron();

				case SphereType.uvsphere:
					return new UvPolyhedron();
					
				default:
					return new Icosahedron();
			}
		}

		private void UpdateMesh() {
			if (!_filter.sharedMesh) {
				_filter.sharedMesh = new Mesh();
			}

			_filter.sharedMesh.Clear();
			_filter.sharedMesh.name = SphereType.ToString();
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
