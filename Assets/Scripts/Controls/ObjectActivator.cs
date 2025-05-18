using UnityEngine;

public static class ObjectActivator
{
    public static void Activator(bool _state, GameObject _object)
    {
        _object.SetActive(_state);
    }
}