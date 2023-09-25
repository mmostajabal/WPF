using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Test
{
  public class SmartObjectInfo
  {
    public SmartObjectInfo()
    {
    }

    public string Name { get; set; }

    public string TemplateName { get; set; }

    public ObservableCollection<SmartObjectProperty> Properties { get; }
      = new ObservableCollection<SmartObjectProperty>();

    public override string ToString()
    {
        string prop = "";

        foreach (SmartObjectProperty property in Properties)
        {
            prop += (string.IsNullOrEmpty(prop) ? ""  : ",") + property.ToString();
        }

        return Name + "," + TemplateName +  "," + (prop != "" ? prop : ",,"); 
    }
  }

}
