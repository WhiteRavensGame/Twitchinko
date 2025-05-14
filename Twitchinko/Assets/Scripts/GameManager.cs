using TwitchIntegration;
using UnityEngine;

public class GameManager : TwitchMonoBehaviour
{
    [SerializeField] private GameObject _testObject;

    [SerializeField] private GameObject _foodObject;

    private Vector3 _targetPosition;

    [TwitchCommand("move_object", "move", "m")]
    public void MovePillpet(float x, float y)
    {
        Pillpet p = _testObject.GetComponent<Pillpet>();
        p.Move(x, -3.5f);
    }

    [TwitchCommand("drop_food", "food")]
    public void DropFood()
    {
        float spawnPoint = Random.Range(-8f, 8f);
        _foodObject.transform.position = new Vector3(spawnPoint, 0, 0);
        _foodObject.SetActive(true);
    }

    [TwitchCommand("revive", "revive")]
    public void Revive()
    {
        Pillpet p = _testObject.GetComponent<Pillpet>();
        p.Revive();
    }

    // Update is called once per frame
    void Update()
    {
        //_testObject.transform.position = Vector3.Lerp(_testObject.transform.position, _targetPosition, Time.deltaTime * 3);
    }
}
