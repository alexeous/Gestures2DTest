namespace Util;

public class RestrictType : PropertyAttribute
{
    public Type Type;

    public RestrictType(Type type)
    {
        Type = type;
    }
}