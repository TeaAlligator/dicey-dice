using System.Collections;
using UnityEngine;

namespace Assets.Code.UnityBehaviours.Board
{
    public class PogBehaviour : MonoBehaviour
    {
        public MeshRenderer Mesh;

        public void Start()
        {
            var targetUrl = "https://upload.wikimedia.org/wikipedia/commons/b/b0/PSM_V37_D105_English_tabby_cat.jpg";

            StartCoroutine(SetImageToUrl(targetUrl));
        }

        IEnumerator SetImageToUrl(string url)
        {
            var www = new WWW(url);
            
            yield return www;
            
            Mesh.material.mainTexture = www.texture;
        }
    }
}
