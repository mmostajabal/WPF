using System;

namespace Test
{
  public class SmartObjectProperty
  {
    public SmartObjectProperty()
    {
    }

    public string Name { get; set; }
    public object Value { get; set; }
    public Type Type { get; set; }
    public override string ToString()
    {
            return Name?.ToString() + "," + Value?.ToString() + "," + Type?.ToString();
    }
  }
}