using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatKing.Base {

	public class Child : MonoBehaviour {
		[SerializeField] Transform root = null;
		public Transform Root { get { return root; } }

#if UNITY_EDITOR
		void OnValidate() {
			if (root == null) { root = transform.parent; }
		}
#endif

		//

		public T GetRootComponent<T>() where T : Component {
			return (root != null ? root : transform).GetComponent<T>();
		}

		public T[] GetRootComponents<T>() where T : Component {
			return (root != null ? root : transform).GetComponents<T>();
		}

		public T GetRootComponentInChildren<T>(bool includeInactive = false) where T : Component {
			return (root != null ? root : transform).GetComponentInChildren<T>(includeInactive);
		}

		public T[] GetRootComponentsInChildren<T>( bool includeInactive = false) where T : Component {
			return (root != null ? root : transform).GetComponentsInChildren<T>(includeInactive);
		}
	}

	public static class Root {
		public static Transform Get(Component t) {
			if (t == null) { return null; }
			if (t.TryGetComponent(out Child c)) { return c.Root; }
			return t.transform;
		}

		public static Transform Get(Transform t) {
			if (t == null) { return null; }
			if (t.TryGetComponent(out Child c)) { return c.Root; }
			return t;
		}

		public static GameObject Get(GameObject go) {
			if (go == null) { return null; }
			if (go.TryGetComponent(out Child c)) { return c.Root.gameObject; }
			return go;
		}

		public static T GetComponent<T>(Component t) where T : class {
			if (t == null) { return null; }
			if (t.TryGetComponent(out Child c)) { return c.Root.GetComponent<T>(); }
			return t.GetComponent<T>();
		}

		public static T GetComponent<T>(Transform t) where T : class {
			if (t == null) { return null; }
			if (t.TryGetComponent(out Child c)) { return c.Root.GetComponent<T>(); }
			return t.GetComponent<T>();
		}

		public static T GetComponent<T>(GameObject go) where T : class {
			if (go == null) { return null; }
			if (go.TryGetComponent(out Child c)) { return c.Root.GetComponent<T>(); }
			return go.GetComponent<T>();
		}

		public static bool TryGetComponent<T>(Component t, out T result) where T : class {
			if (t == null) { result = null; return false; }
			if (t.TryGetComponent(out Child c)) { return c.Root.TryGetComponent(out result); }
			return t.TryGetComponent<T>(out result);
		}

		public static bool TryGetComponent<T>(Transform t, out T result) where T : class {
			if (t == null) { result = null; return false; }
			if (t.TryGetComponent(out Child c)) { return c.Root.TryGetComponent(out result); }
			return t.TryGetComponent<T>(out result);
		}

		public static bool TryGetComponent<T>(GameObject go, out T result) where T : class {
			if (go == null) { result = null; return false; }
			if (go.TryGetComponent(out Child c)) { return c.Root.TryGetComponent(out result); }
			return go.TryGetComponent<T>(out result);
		}

		public static T[] GetComponents<T>(Component t) where T : class {
			if (t == null) { return null; }
			if (t.TryGetComponent(out Child c)) { return c.Root.GetComponents<T>(); }
			return t.GetComponents<T>();
		}

		public static T[] GetComponents<T>(Transform t) where T : class {
			if (t == null) { return null; }
			if (t.TryGetComponent(out Child c)) { return c.Root.GetComponents<T>(); }
			return t.GetComponents<T>();
		}

		public static T[] GetComponents<T>(GameObject go) where T : class {
			if (go == null) { return null; }
			if (go.TryGetComponent(out Child c)) { return c.Root.GetComponents<T>(); }
			return go.GetComponents<T>();
		}

		public static T GetComponentInChildren<T>(Component t, bool includeInactive = false) where T : class {
			if (t == null) { return null; }
			if (t.TryGetComponent(out Child c)) { return c.Root.GetComponentInChildren<T>(includeInactive); }
			return t.GetComponentInChildren<T>(includeInactive);
		}

		public static T GetComponentInChildren<T>(Transform t, bool includeInactive = false) where T : class {
			if (t == null) { return null; }
			if (t.TryGetComponent(out Child c)) { return c.Root.GetComponentInChildren<T>(includeInactive); }
			return t.GetComponentInChildren<T>(includeInactive);
		}

		public static T GetComponentInChildren<T>(GameObject go, bool includeInactive = false) where T : class {
			if (go == null) { return null; }
			if (go.TryGetComponent(out Child c)) { return c.Root.GetComponentInChildren<T>(includeInactive); }
			return go.GetComponentInChildren<T>(includeInactive);
		}

		public static bool TryGetComponentInChildren<T>(Component t, out T result, bool includeInactive = false) where T : class {
			if (t == null) { result = null; return false; }
			if (t.TryGetComponent(out Child c)) { result = c.Root.GetComponentInChildren<T>(includeInactive); return result != null; }
			result = t.GetComponentInChildren<T>(includeInactive); return result != null;
		}

		public static bool TryGetComponentInChildren<T>(Transform t, out T result, bool includeInactive = false) where T : class {
			if (t == null) { result = null; return false; }
			if (t.TryGetComponent(out Child c)) { result = c.Root.GetComponentInChildren<T>(includeInactive); return result != null; }
			result = t.GetComponentInChildren<T>(includeInactive); return result != null;
		}

		public static bool TryGetComponentInChildren<T>(GameObject go, out T result, bool includeInactive = false) where T : class {
			if (go == null) { result = null; return false; }
			if (go.TryGetComponent(out Child c)) { result = c.Root.GetComponentInChildren<T>(includeInactive); return result != null; }
			result = go.GetComponentInChildren<T>(includeInactive); return result != null;
		}

		public static T[] GetComponentsInChildren<T>(Component t, bool includeInactive = false) where T : class {
			if (t == null) { return null; }
			if (t.TryGetComponent(out Child c)) { return c.Root.GetComponentsInChildren<T>(includeInactive); }
			return t.GetComponentsInChildren<T>(includeInactive);
		}

		public static T[] GetComponentsInChildren<T>(Transform t, bool includeInactive = false) where T : class {
			if (t == null) { return null; }
			if (t.TryGetComponent(out Child c)) { return c.Root.GetComponentsInChildren<T>(includeInactive); }
			return t.GetComponentsInChildren<T>(includeInactive);
		}

		public static T[] GetComponentsInChildren<T>(GameObject go, bool includeInactive = false) where T : class {
			if (go == null) { return null; }
			if (go.TryGetComponent(out Child c)) { return c.Root.GetComponentsInChildren<T>(includeInactive); }
			return go.GetComponentsInChildren<T>(includeInactive);
		}
	}

}