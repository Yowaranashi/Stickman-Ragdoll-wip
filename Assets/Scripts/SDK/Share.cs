using InstantGamesBridge;
using InstantGamesBridge.Modules.Social;
using UnityEngine;

namespace SDK
{
    public class Share : MonoBehaviour
    {
        [Header("All about share")] 
        [SerializeField] private string vkShareLink = "https://vk.com/mewton.games";

        public void ShareLink()
        {
            Bridge.social.Share(
                success => { 
                    if (success)
                    {
                        // Success
                        Debug.LogAssertion($"Share is {success}");
                    }
                    else
                    {
                        // Error
                        Debug.LogError($"Share is {success}");
                    }
                },
                new ShareVkOptions(vkShareLink));
        }
    }
}
