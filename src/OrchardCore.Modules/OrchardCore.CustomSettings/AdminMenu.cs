using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.CustomSettings.Services;
using OrchardCore.Navigation;

namespace OrchardCore.CustomSettings
{
    public class AdminMenu : INavigationProvider
    {
        private readonly CustomSettingsService _customSettingsService;

        public AdminMenu(
            IStringLocalizer<AdminMenu> localizer,
            CustomSettingsService customSettingsService)
        {
            T = localizer;
            _customSettingsService = customSettingsService;
        }

        public IStringLocalizer T { get; set; }

        public async Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            foreach (var type in await _customSettingsService.GetAllSettingsTypesAsync())
            {
                builder
                    .Add(T["Configuration"], configuration => configuration
                        .Add(T["Settings"], settings => settings
                            .Add(new LocalizedString(type.DisplayName, type.DisplayName), layers => layers
                                .Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = type.Name })
                                .Permission(Permissions.CreatePermissionForType(type))
                                .Resource(type.Name)
                                .LocalNav()
                            )));
            }
        }
    }
}
