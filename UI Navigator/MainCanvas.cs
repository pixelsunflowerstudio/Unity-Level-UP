using Game.Defines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class MainCanvas : Singleton<MainCanvas>
    {
        public RectTransform RectTransform;

        private List<UITagObject> _activeTaggedObjects = new();

        public void AddActiveTag(UITagObject taggedObject)
        {
            if (!_activeTaggedObjects.Contains(taggedObject))
                _activeTaggedObjects.Add(taggedObject);
        }

        public void RemoveActiveTag(UITagObject taggedObject)
        {
            if (_activeTaggedObjects.Contains(taggedObject))
                _activeTaggedObjects.Remove(taggedObject);
            else
                Debug.LogWarning($"{taggedObject.gameObject.name} is not an active UI element");
        }

        public UITagObject GetTaggedObject(UIElementTag tag)
        {
            foreach (var taggedObject in _activeTaggedObjects)
            {
                if (taggedObject.Tag == tag)
                    return taggedObject;
            }
            Debug.LogWarning($"{tag} is not an active UI element");
            return null;
        }
    }
}
