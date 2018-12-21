using System.Collections;
using UnityEngine;
using ICODES.STUDIO.WWebView;
using sh_akira.VROverlay;

namespace VROverlayWebView {
    [RequireComponent(typeof(WWebView))]
    public class VRWebView : MonoBehaviour {
        bool IsDrag = false;
        bool IsKeyUp = false;

        IEnumerator Start() {
            while (!AreYouReady) yield return null;

            // webView.Alpha = 0.005f;
            webView.Show();
            webView.SetTexture(texture);

            var renderer = GetComponent<MeshRenderer>();
            if (renderer != null) renderer.material.mainTexture = texture;

            var overray = GetComponent<VROverlay>();
            if (overray != null) overray.texture = texture;
        }

        void Update() {

        }

        WWebView webView {
            get {
                return _webView ?? (_webView = GetComponent<WWebView>());
            }
        }

        WWebView _webView;

        Texture2D texture {
            get {
                if (_texture != null) return _texture;
                _texture = new Texture2D(webView.GetActualWidth(), webView.GetActualHeight(), TextureFormat.ARGB32, false);
                _texture.filterMode = FilterMode.Trilinear;
                _texture.Apply();
                return _texture;
            }
        }

        Texture2D _texture;

        bool AreYouReady {
            get {
                return (webView.GetActualWidth() > 0);
            }
        }
    }
}
