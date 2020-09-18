
using System.ComponentModel.DataAnnotations.Schema;

[NotMapped]
public class clsString
{
    public clsString()
    {
    }

    public clsString(string name)
    {
        Name = name;
    }

    public string Name {get; set;}
}