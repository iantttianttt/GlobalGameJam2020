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
		public GameObject GatPoolGameObject { get { return mPoolGameObjectRoot; } }

		private GameObject        mPoolGameObjectRoot;
		private int               mNextId = 1;
		private Stack<GameObject> mInactive;
		private GameObject        mPrefab;
		private List<GameObject>  mSpawnedObject = new List<GameObject>();

		// Constructor
		public Pool(GameObject prefab, int initialQty)
		{
			this.mPrefab = prefab;
			mPoolGameObjectRoot = new GameObject("Pool_" + prefab.ToString());
			mPoolGameObjectRoot.transform.SetParent(ObjectPool.Instance.gameObject.transform);

			// If Stack uses a linked list internally, then this
			// whole initialQty thing is a placebo that we could
			// strip out for more minimal code. But it can't *hurt*.
			mInactive = new Stack<GameObject>(initialQty);
		}

		/// <summary>
		/// 生成新物件
		/// </summary>
		public GameObject Spawn(Vector3 pos, Quaternion rot, Transform parent = null)
		{
			GameObject obj;
			if (mInactive.Count == 0)
			{
				// We don't have an object in our pool, so we
				// instantiate a whole new object.
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
			mSpawnedObject.Add(obj);
			return obj;
		}
		/// <summary>
		/// 回收特定物件
		/// </summary>
		public void Despawn(GameObject obj)
		{
			obj.SetActive(false);
			mInactive.Push(obj);
			obj.transform.SetParent(mPoolGameObjectRoot.transform);
			mSpawnedObject.Remove(obj);
		}
		/// <summary>
		/// 回收所有使用中物件
		/// </summary>
		public void RecoverAll()
		{
			for (int i = mSpawnedObject.Count - 1; i >= 0; i--)
			{
				Despawn(mSpawnedObject[i]);
			}
		}
		/// <summary>
		/// 刪除物件池中未使用物件
		/// </summary>
		public void DestoryUnused()
		{
			for (int i = mInactive.Count - 1; i >= 0; i--)
			{
				Destroy(mInactive.Pop());
			}
		}
		/// <summary>
		/// 刪除物件池處理
		/// </summary>
		public void OnDestoryPool()
		{
			for (int i = mInactive.Count - 1; i >= 0; i--)
			{
				Destroy(mInactive.Pop());
			}
			for (int i = mSpawnedObject.Count - 1; i >= 0; i--)
			{
				Destroy(mSpawnedObject[i]);
			}
			Destroy(mPoolGameObjectRoot);
		}

	}


	/// <summary>
	/// 追加於生成物上，用來追蹤來源
	/// </summary>
	class PoolMember : MonoBehaviour
	{
		public Pool myPool;
	}

	/// <summary>
	/// 初始化該Prefab專用Pool
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
	/// 預載物件
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
	/// 生成物件   
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
	/// 回收物件
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

	/// <summary>
	/// 回收所有生成物件
	/// </summary>
	public void RecoverAllSpawnedObject()
	{
		if (pools == null) { return; }
		foreach (Pool pool in pools.Values)
		{
			pool.RecoverAll();
		}
	}

	/// <summary>
	/// 刪除所有未使用物件
	/// </summary>
	public void DestoryAllUnusedObject()
	{
		if (pools == null) { return; }
		foreach (Pool pool in pools.Values)
		{
			pool.DestoryUnused();
		}
	}
	/// <summary>
	/// 刪除特定物件池
	/// </summary>
	public void DestoryPool(GameObject _TargetPrefab)
	{
		if (pools == null) { return; }
		if (pools.ContainsKey(_TargetPrefab))
		{
			Pool targetPool = null;
			if (pools.TryGetValue(_TargetPrefab, out targetPool))
			{
				targetPool.OnDestoryPool();
				pools.Remove(_TargetPrefab);
			}
		}
	}
	/// <summary>
	/// 刪除所有物件池
	/// </summary>
	public void DestoryAllPool()
	{
		if (pools == null) { return; }
		foreach (Pool pool in pools.Values)
		{
			pool.OnDestoryPool();
		}
		pools.Clear();
	}

	/// <summary>
	/// 所有Prefab物件與其對應物件池
	/// </summary>
	private Dictionary<GameObject, Pool> pools;

	//-----------------------------------------------------------------------
	// Const
	//-----------------------------------------------------------------------
	const int DEFAULT_POOL_SIZE = 3;
}
