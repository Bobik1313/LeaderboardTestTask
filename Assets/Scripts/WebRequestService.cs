using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine;

namespace Assets.Scripts
{
    public class WebRequestService
    {
        private static Dictionary<string, Sprite> avatarCache = new Dictionary<string, Sprite>();

        public static IEnumerator LoadAvatar(string url, Action<Sprite> onAvatarLoaded)
        {
            //Check avatar in cache
            if (avatarCache.TryGetValue(url, out Sprite cachedAvatar))
            {
                onAvatarLoaded?.Invoke(cachedAvatar);
                yield break;
            }


            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error loading avatar: " + request.error);
                onAvatarLoaded?.Invoke(null);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                Sprite avatar = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

                // Save avatar in cache
                avatarCache[url] = avatar;

                onAvatarLoaded?.Invoke(avatar); 
            }
        }
    }
}
