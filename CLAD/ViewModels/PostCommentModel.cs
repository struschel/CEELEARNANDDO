using System.ComponentModel.DataAnnotations;

namespace CLAD.ViewModels
{
    public class PostCommentModel
    {
        [Required]
        [MinLength(20)]
        public string Message { get; set; }
    }
}