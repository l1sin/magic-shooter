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
    }

    public void SelectMap(int i)
    {
        _camera.transform.position = Presets[i]._CameraTransform.position;
        _camera.transform.rotation = Presets[i]._CameraTransform.rotation;
        _terrain.terrainData = Presets[i]._TerrainData;
        _volume.profile = Presets[i]._VolumeProfile;
        RenderSettings.skybox = Presets[i]._Skybox;
        SaveManager.Instance.CurrentProgress.SelectedMap = i;
    }

    [ContextMenu("SelectMeadow")]
    public void SelectMeadow()
    {
        _camera.transform.position = Presets[0]._CameraTransform.position;
        _camera.transform.rotation = Presets[0]._CameraTransform.rotation;
        _terrain.terrainData = Presets[0]._TerrainData;
        _volume.profile = Presets[0]._VolumeProfile;
        RenderSettings.skybox = Presets[0]._Skybox;
        SaveManager.Instance.CurrentProgress.SelectedMap = 0;
    }
    [ContextMenu("SelectDesert")]
    public void SelectDesert()
    {
        _camera.transform.position = Presets[1]._CameraTransform.position;
        _camera.transform.rotation = Presets[1]._CameraTransform.rotation;
        _terrain.terrainData = Presets[1]._TerrainData;
        _volume.profile = Presets[1]._VolumeProfile;
        RenderSettings.skybox = Presets[1]._Skybox;
        SaveManager.Instance.CurrentProgress.SelectedMap = 1;
    }
    [ContextMenu("SelectArctic")]
    public void SelectArctic()
    {
        _camera.transform.position = Presets[2]._CameraTransform.position;
        _camera.transform.rotation = Presets[2]._CameraTransform.rotation;
        _terrain.terrainData = Presets[2]._TerrainData;
        _volume.profile = Presets[2]._VolumeProfile;
        RenderSettings.skybox = Presets[2]._Skybox;
        SaveManager.Instance.CurrentProgress.SelectedMap = 2;
    }
    [ContextMenu("SelectMoon")]
    public void SelectMoon()
    {
        _camera.transform.position = Presets[3]._CameraTransform.position;
        _camera.transform.rotation = Presets[3]._CameraTransform.rotation;
        _terrain.terrainData = Presets[3]._TerrainData;
        _volume.profile = Presets[3]._VolumeProfile;
        RenderSettings.skybox = Presets[3]._Skybox;
        SaveManager.Instance.CurrentProgress.SelectedMap = 3;
    }
}
