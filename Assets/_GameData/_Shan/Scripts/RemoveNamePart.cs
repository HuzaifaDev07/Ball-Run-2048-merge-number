using UnityEngine;
using NaughtyAttributes;


namespace BaranovskyStudio
{
    [ExecuteInEditMode]
    public class RemoveNamePart : MonoBehaviour
    {
        public string partToRemove;
        private string modifiedName;
        public void RemovePart()
        {
            string objectName = gameObject.name;

            if (objectName.Contains(partToRemove))
            {
                modifiedName = objectName.Replace(partToRemove, string.Empty).Trim();
                gameObject.name = modifiedName  ;
            }
        }

        private void OnEnable()
        {
            modifiedName = gameObject.name;
            RemovePart();
        }
        [Button]
        public void ShowElement()
        {
            RemovePart();
        }

    }
}