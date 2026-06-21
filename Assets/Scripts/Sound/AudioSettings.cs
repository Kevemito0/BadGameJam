// Assets/Scripts/Canvas/MusicVolumeSlider.cs
// Settings panelindeki Slider objesine bu scripti ekle.
// Slider'ın Min: 0, Max: 1, WholeNumbers: false olduğundan emin ol.

using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        // Kaydedilmiş değeri slider'a yansıt
        slider.value = SoundManager.GetMusicVolume();

        // Kullanıcı slider'ı hareket ettirince çağrılır
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnSliderChanged(float value)
    {
        SoundManager.SetMusicVolume(value);
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnSliderChanged);
    }
}