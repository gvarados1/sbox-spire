namespace Spire;

public struct Range<T> where T : unmanaged
{
    public T Min;
    public T Max;

    public Range( T min, T max )
    {
        Min = min;
        Max = max;
    }
}
