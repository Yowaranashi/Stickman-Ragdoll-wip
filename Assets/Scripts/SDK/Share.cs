using UnityEngine;

namespace SDK
{
    public class Share : MonoBehaviour
    {
        [Header("All about share")] 
        [SerializeField] private string shareLink = "https://example.com";

        public void ShareLink()
        {
            Application.OpenURL(shareLink);
        }
    }
}
