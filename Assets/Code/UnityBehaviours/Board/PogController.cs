using System.Collections;
using UnityEngine;

namespace Assets.Code.UnityBehaviours.Board
{
    [RequireComponent(typeof(PickUpBehaviour))]
    internal class PogController : InitializeRequiredBehaviour
    {
        /* REFERENCES */
        public MeshRenderer Mesh;
        public PickUpBehaviour PickUp;

        public void Initialize(string url)
        {
            //StartCoroutine(SetImageToUrl(url));

            MarkAsInitialized();
        }

        IEnumerator SetImageToUrl(string url)
        {
            var www = new WWW(url);
            
            yield return www;
            
            Mesh.material.mainTexture = www.texture;
        }

        public void OnMouseDown()
        {
            
        }
    }
}
