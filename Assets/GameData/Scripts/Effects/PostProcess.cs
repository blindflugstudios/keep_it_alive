using KeepItAlive.World;
using UnityEngine;

namespace KeepItAlive.Effects
{
    public sealed class PostProcess : MonoBehaviour
    {
        [SerializeField] private Material _worldEndPost;
        private static readonly int WorldSize = Shader.PropertyToID("_WorldSize");

        private UnityEngine.Camera _cam;
        private static readonly int CameraPos = Shader.PropertyToID("_CameraPos");
        private static readonly int CameraSize = Shader.PropertyToID("_CameraSize");

        public void Awake()
        {
            _cam = GetComponent<UnityEngine.Camera>();
        }
        
        private void Start()
        {
            _worldEndPost.SetFloat(WorldSize, WorldGenerator.Instance.Settings.WorldSize);
        }

        public void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            _worldEndPost.SetVector(CameraPos, new Vector4(_cam.transform.position.x, _cam.transform.position.y, .0f, .0f));
            var camHeight = _cam.orthographicSize * 2.0f;
            _worldEndPost.SetVector(CameraSize, new Vector4(camHeight * _cam.aspect, camHeight, .0f, .0f));
            Graphics.Blit(src, dest, _worldEndPost);
        }
    }
}