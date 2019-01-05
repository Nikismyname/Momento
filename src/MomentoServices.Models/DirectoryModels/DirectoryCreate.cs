namespace Momento.Services.Models.DirectoryModels
{
    using Momento.Services.Models.CustomAttributes;
    using System.ComponentModel.DataAnnotations;

    public class DirectoryCreate
    {
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Folder Name must be between 3 and 50 characters long")]
        [ShouldNotBeValidation("boot")]
        public string DirectoryName { get; set; }
        [Range(1, int.MaxValue)]
        public int ParentDirId { get; set; }
    }
}
