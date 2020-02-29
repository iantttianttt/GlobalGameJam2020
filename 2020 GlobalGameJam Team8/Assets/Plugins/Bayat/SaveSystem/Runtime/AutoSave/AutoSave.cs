using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bayat.Json;

namespace Bayat.SaveSystem
{

#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    [AddComponentMenu("Bayat/Save System/Auto Save")]
    public class AutoSave : GameObjectSerializationHandler
    {

        protected virtual void OnEnable()
        {
            AutoSaveManager.Current.AddAutoSave(this);
        }

        protected virtual void OnDisable()
        {
            AutoSaveManager.Current.RemoveAutoSave(this);
        }

        protected virtual void OnDestroy()
        {
            AutoSaveManager.Current.RemoveAutoSave(this);
        }

    }

}