namespace Momento.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum Formatting
    {
        [Display(Name = "Select Formatting")]
        Select_Formatting,
        None,
        CSharp,
        SQL, 
    }
}
