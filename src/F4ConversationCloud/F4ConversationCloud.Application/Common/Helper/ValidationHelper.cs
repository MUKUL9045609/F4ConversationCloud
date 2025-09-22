using F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel;
using System.ComponentModel.DataAnnotations;

namespace F4ConversationCloud.Application.Common.Helper
{
    public class ValidationHelper
    {
        public async Task<IEnumerable<ValidationResult>> Validate(RegisterUserModel model)
        {
            var errors = new List<ValidationResult>();

            if (!model.TermsCondition)
            {
                errors.Add(new ValidationResult("This Field is Required.", new[] { nameof(model.TermsCondition) }));
            }

            return errors;
        }
    }
}
