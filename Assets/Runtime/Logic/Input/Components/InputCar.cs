using Notteam.FastGameCore;

public class InputCar : GameUpdaterComponent<InputSystem>
{
    private float _acceleration;
    private float _brake;
    private int _handBrake;
    private float _steering;

    public float Acceleration => _acceleration;
    public float Brake => _brake;
    public int HandBrake => _handBrake;
    public float Steering => _steering;

    protected override void OnAwakeComponent()
    {
        System.BindToInputAction("Acceleration", (x) => { _acceleration = x; });
        System.BindToInputAction("Brake", (x) => { _brake = x; });
        System.BindToInputAction("HandBrake", (x) => { _handBrake = (int)x; });
        System.BindToInputAction("Steering", (x) => { _steering = x; });
    }
}