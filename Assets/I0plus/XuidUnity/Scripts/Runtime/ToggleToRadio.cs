using UnityEngine;
using UnityEngine.UI;

namespace I0plus.XduiUnity
{
    /// <summary>
    /// Create an Gameobject with a ToggleGroup in the Scene.
    /// Share it with a Toggle with the same Group name.
    /// </summary>
    public class ToggleToRadio : MonoBehaviour
    {
        private const string MANAGER_OBJECT_NAME = "[XuidUnity] ToggleGroups";

        [SerializeField] private string groupName;

        public string GroupName
        {
            get => groupName;
            set => groupName = value;
        }
        
        private void OnEnable()
        {
            SetToggleGroup();
        }

        public void SetToggleGroup()
        {
            if (groupName != null)
            {
                var toggle = GetComponent<Toggle>();
                if (toggle != null)
                {
                    var toggleGroup = GetOrCreateToggleGroup(groupName);
                    toggle.group = toggleGroup;
                }
            }
        }

        /// <summary>
        /// Create or share a ToggleGroup.
        /// Identify it by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ToggleGroup GetOrCreateToggleGroup(string name)
        {
            // まだその名前でToggleGroupがつくられていない
            var groupsObject = GameObject.Find(MANAGER_OBJECT_NAME);
            if (groupsObject == null) groupsObject = new GameObject(MANAGER_OBJECT_NAME);

            var toggleGroupObject = groupsObject.transform.Find(name)?.gameObject;
            if (toggleGroupObject == null)
            {
                toggleGroupObject = new GameObject(name);
                toggleGroupObject.AddComponent<ToggleGroup>();
                toggleGroupObject.transform.SetParent(groupsObject.transform);
            }

            var toggleGroup = toggleGroupObject.GetComponent<ToggleGroup>();

            return toggleGroup;
        }
    }
}