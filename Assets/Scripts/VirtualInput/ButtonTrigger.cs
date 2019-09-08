using UnityEngine;
using UnityGB;

namespace VirtualInput
{
    [RequireComponent(typeof(BoxCollider))]
    public class ButtonTrigger : MonoBehaviour
    {
        [SerializeField]
        private AvatarHandsInputHub handsInputHub;
        [SerializeField]
        private string tagFilter = "Hands";
        [SerializeField]
        private EmulatorBase.Button key;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Enter " + other.gameObject.name);

            if (other.CompareTag(tagFilter))
            {
                handsInputHub.SetButtonState(key, true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("Exit " + other.gameObject.name);

            if (other.CompareTag(tagFilter))
            {
                handsInputHub.SetButtonState(key, false);
            }
        }
    }
}
