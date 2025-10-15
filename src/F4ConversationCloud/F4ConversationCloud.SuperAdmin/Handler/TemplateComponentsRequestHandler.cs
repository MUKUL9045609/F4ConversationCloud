using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates;
using F4ConversationCloud.SuperAdmin.Models;

namespace F4ConversationCloud.SuperAdmin.Handler
{
    public static class TemplateComponentsRequestHandler
    {
        
        public static Task<List<CreateTemplateComponent>> ComponetRequest(CreateTemplateViewModel request)
        {
            var components = (request.Components ?? new List<ComponentViewModel>())
                .Where(c => !string.IsNullOrWhiteSpace(c.Text)
                    || (c.Buttons?.Any(b => !string.IsNullOrWhiteSpace(b.Text)) == true))
                .Select(c => new CreateTemplateComponent
                {
                    Type = c.Type,
                    Text = c.Text,
                    Format = c.Format,
                    Buttons = c.Buttons?
                        .Where(b => !string.IsNullOrWhiteSpace(b.Text))
                        .Select(b => new TemplateButton
                        {
                            Text = b.Text,
                            Type = b.Type,
                            Url = b.Url
                        }).ToList(),
                    Example = c.Example != null
                        ? new Example
                        {
                            HeaderText = (c.Example.HeaderText?.Any() == true)
                                ? c.Example.HeaderText
                                : new List<string>(),
                            BodyText = c.Example.BodyText?.ToList()
                                ?? new List<List<string>>()
                        }
                        : null
                }).ToList();

            return Task.FromResult(components);
        }
    }
}
