using System;
using UnityEngine;
using UnityEngine.Rendering;

public class MapSelector : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Terrain _terrain;
    [SerializeField] private Volume _volume;

    public BGPreset[] Presets;
    [Serializable]
    public struct BGPreset
    {
        public Transform _CameraTransform;
        public TerrainData _TerrainData;
        public VolumeProfile _VolumeProfile;
        public Material _Skybox;
        public Color _AmbientColor;
    }

    public void SelectMap(int i)
    {
        _camera.transform.position = Presets[i]._CameraTransform.position;
        _camera.transform.rotation = Presets[i]._CameraTransform.rotation;
        _terrain.terrainData = Presets[i]._TerrainData;
        _volume.profile = Presets[i]._VolumeProfile;
        RenderSettings.skybox = Presets[i]._Skybox;
        RenderSettings.ambientLight = Presets[i]._AmbientColor;
        SaveManager.Instance.CurrentProgress.SelectedMap = i;
    }
}
