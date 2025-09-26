using System.ComponentModel.DataAnnotations;
namespace F4ConversationCloud.Application.Common.Models.OnBoardingModel
{
    

    public class ResetPasswordResponseViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password  { get; set; }
        public string Stage { get; set; }
    }
   
}
