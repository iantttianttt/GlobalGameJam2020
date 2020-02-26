using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ObjectPool : Singleton<ObjectPool>
{
	/// <summary>
	/// 每個Prefab專用之物件池
	/// </summary>
	class Pool
	{
		public GameObject GatPoolGameObject { get { return mPoolGameObject; } }

		private GameObject        mPoolGameObject;
		private int               mNextId = 1;
		private Stack<GameObject> mInactive;
		private GameObject        mPrefab;

		// Constructor
		public Pool(GameObject prefab, int initialQty)
		{
			this.mPrefab = prefab;
			mPoolGameObject = new GameObject("Pool_" + prefab.ToString());
			mPoolGameObject.transform.SetParent(ObjectPool.Instance.gameObject.transform);

			// If Stack uses a linked list internally, then this
			// whole initialQty thing is a placebo that we could
			// strip out for more minimal code. But it can't *hurt*.
			mInactive = new Stack<GameObject>(initialQty);
		}

		// Spawn an object from our pool
		public GameObject Spawn(Vector3 pos, Quaternion rot, Transform parent = null)
		{
			GameObject obj;
			if (mInactive.Count == 0)
			{
				// We don't have an object in our pool, so we
				// instantiate a whole new object.
				/*if (parent == null)
				{
					parent
				}*/
				obj = (GameObject)GameObject.Instantiate(mPrefab, pos, rot, parent);
				obj.name = mPrefab.name + " (" + (mNextId++) + ")";

				// Add a PoolMember component so we know what pool
				// we belong to.
				obj.AddComponent<PoolMember>().myPool = this;
			}
			else
			{
				// Grab the last object in the inactive array
				obj = mInactive.Pop();

				if (obj == null)
				{
					// The inactive object we expected to find no longer exists.
					// The most likely causes are:
					//   - Someone calling Destroy() on our object
					//   - A scene change (which will destroy all our objects).
					//     NOTE: This could be prevented with a DontDestroyOnLoad
					//	   if you really don't want this.
					// No worries -- we'll just try the next one in our sequence.

					return Spawn(pos, rot, parent);
				}
			}

			obj.transform.position = pos;
			obj.transform.rotation = rot;
			obj.transform.SetParent(parent);
			if (parent == null)
			{
				SceneManager.MoveGameObjectToScene(obj, SceneManager.GetActiveScene());
			}
			obj.SetActive(true);
			return obj;
		}

		// Return an object to the inactive pool.
		public void Despawn(GameObject obj)
		{
			obj.SetActive(false);
			mInactive.Push(obj);
			obj.transform.SetParent(mPoolGameObject.transform);
		}

	}


	/// <summary>
	/// Added to freshly instantiated objects, so we can link back
	/// to the correct pool on despawn.
	/// </summary>
	class PoolMember : MonoBehaviour
	{
		public Pool myPool;
	}

	// All of our pools
	private Dictionary<GameObject, Pool> pools;

	/// <summary>
	/// Initialize our dictionary.
	/// </summary>
	private void Init(GameObject prefab = null, int qty = DEFAULT_POOL_SIZE)
	{
		if (pools == null)
		{
			pools = new Dictionary<GameObject, Pool>();
		}
		if (prefab != null && pools.ContainsKey(prefab) == false)
		{
			pools[prefab] = new Pool(prefab, qty);
		}
	}

	/// <summary>
	/// If you want to preload a few copies of an object at the start
	/// of a scene, you can use this. Really not needed unless you're
	/// going to go from zero instances to 100+ very quickly.
	/// Could technically be optimized more, but in practice the
	/// Spawn/Despawn sequence is going to be pretty darn quick and
	/// this avoids code duplication.
	/// </summary>
	public void Preload(GameObject prefab, int qty = 1)
	{
		Init(prefab, qty);

		// Make an array to grab the objects we're about to pre-spawn.
		GameObject[] obs = new GameObject[qty];
		for (int i = 0; i < qty; i++)
		{
			obs[i] = Spawn(prefab, Vector3.zero, Quaternion.identity, pools[prefab].GatPoolGameObject.transform);
		}

		// Now despawn them all.
		for (int i = 0; i < qty; i++)
		{
			Despawn(obs[i]);
		}
	}

	/// <summary>
	/// Spawns a copy of the specified prefab (instantiating one if required).
	/// NOTE: Remember that Awake() or Start() will only run on the very first
	/// spawn and that member variables won't get reset.  OnEnable will run
	/// after spawning -- but remember that toggling IsActive will also
	/// call that function.
	/// </summary>
	public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot, Transform _Parent = null)
	{
		Init(prefab);

		return pools[prefab].Spawn(pos, rot, _Parent);
	}

	/// <summary>
	/// Despawn the specified gameobject back into its pool.
	/// </summary>
	public void Despawn(GameObject obj)
	{
		PoolMember pm = obj.GetComponent<PoolMember>();
		if (pm == null)
		{
			Debug.Log("Object '" + obj.name + "' wasn't spawned from a pool. Destroying it instead.");
			GameObject.Destroy(obj);
		}
		else
		{
			pm.myPool.Despawn(obj);
		}
	}

	//-----------------------------------------------------------------------
	// Const
	//-----------------------------------------------------------------------
	const int DEFAULT_POOL_SIZE = 3;
}
