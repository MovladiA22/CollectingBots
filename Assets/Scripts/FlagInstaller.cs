using UnityEngine;

public class FlagInstaller : MonoBehaviour
{
    [SerializeField] private Land _land;

    private BaseFlag _flag;
    private bool _isFlagSelected = false;

    private void OnEnable()
    {
        _land.Clicked += SetFlag;
    }

    private void OnDisable()
    {
        _land.Clicked -= SetFlag;
    }

    public void ToggleFlagActivation(bool isSelected, BaseFlag flag)
    {
        _isFlagSelected = isSelected;

        if (_isFlagSelected)
            _flag = flag;
        else
            _flag = null;
    }

    private void SetFlag(Vector3 flagPosition)
    {
        if (_isFlagSelected)
        {
            _flag.gameObject.SetActive(true);
            _flag.transform.position = flagPosition;
            _flag.Install();
        }
    }
}
