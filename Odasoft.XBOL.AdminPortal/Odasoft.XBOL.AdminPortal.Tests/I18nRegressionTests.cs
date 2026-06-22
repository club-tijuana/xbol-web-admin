using System.Xml.Linq;
using Xunit;

namespace Odasoft.XBOL.AdminPortal.Tests;

public sealed class I18nRegressionTests
{
    [Fact]
    public void Bundle_menu_labels_have_shared_english_and_spanish_resources()
    {
        string[] keys = ["NewBundle", "NewPackage", "NewSeasonPass"];

        foreach (var key in keys)
        {
            Assert.False(string.IsNullOrWhiteSpace(ResourceValue("Resources/SharedResource.resx", key)));
            Assert.False(string.IsNullOrWhiteSpace(ResourceValue("Resources/SharedResource.en.resx", key)));
            Assert.False(string.IsNullOrWhiteSpace(ResourceValue("Resources/SharedResource.es.resx", key)));
        }
    }

    [Fact]
    public void Bundle_create_editor_titles_are_localized()
    {
        string[] keys =
        [
            "EditorUndo",
            "EditorRedo",
            "EditorBold",
            "EditorItalic",
            "EditorUnderline",
            "EditorUnorderedList",
            "EditorOrderedList",
            "EditorLink",
            "EditorUnlink",
            "EditorRemoveFormat"
        ];

        foreach (var key in keys)
        {
            Assert.False(string.IsNullOrWhiteSpace(ResourceValue("Resources/Components/Pages/BundleCreate.en.resx", key)));
            Assert.False(string.IsNullOrWhiteSpace(ResourceValue("Resources/Components/Pages/BundleCreate.es.resx", key)));
        }

        var source = ReadAppSource("Components/Pages/BundleCreate.razor");
        Assert.DoesNotContain("Title=\"Deshacer\"", source, StringComparison.Ordinal);
        Assert.DoesNotContain("Title=\"Negrita\"", source, StringComparison.Ordinal);
        Assert.DoesNotContain("Title=\"Lista de vinetas\"", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Bundle_create_media_labels_match_event_caratula_and_gallery()
    {
        string[] keys =
        [
            "EventBanner",
            "EventBannerDimensions",
            "EventBannerDescription",
            "WebsiteGallery",
            "GalleryUploadHint",
            "GalleryUploadHintDetails",
            "GalleryPreview",
            "Clear",
            "ImageTooSmall",
            "ErrorSavingMedia"
        ];

        foreach (var key in keys)
        {
            Assert.False(string.IsNullOrWhiteSpace(ResourceValue("Resources/Components/Pages/BundleCreate.en.resx", key)));
            Assert.False(string.IsNullOrWhiteSpace(ResourceValue("Resources/Components/Pages/BundleCreate.es.resx", key)));
        }

        Assert.Equal("Car\u00e1tula del evento", ResourceValue("Resources/Components/Pages/BundleCreate.es.resx", "EventBanner"));
    }

    [Fact]
    public void Bundle_create_events_builder_copy_is_localized()
    {
        string[] keys =
        [
            "BundleLineup",
            "BundleLineupHelp",
            "AvailableEventsToAdd",
            "AvailableEventsToAddHelp",
            "NoEventsSelected",
            "NoEventsSelectedHelp"
        ];

        foreach (var key in keys)
        {
            Assert.False(string.IsNullOrWhiteSpace(ResourceValue("Resources/Components/Pages/BundleCreate.en.resx", key)));
            Assert.False(string.IsNullOrWhiteSpace(ResourceValue("Resources/Components/Pages/BundleCreate.es.resx", key)));
        }
    }

    [Fact]
    public void New_catalog_grids_use_localized_date_placeholders()
    {
        Assert.Equal("MM/DD/YYYY", ResourceValue("Resources/Components/EventsDataGrid.en.resx", "DatePlaceholder"));
        Assert.Equal("DD/MM/AAAA", ResourceValue("Resources/Components/EventsDataGrid.es.resx", "DatePlaceholder"));

        var eventCatalogGrid = ReadAppSource("Components/EventCatalogDataGrid.razor");
        var bundleSchedulesGrid = ReadAppSource("Components/BundleSchedulesDataGrid.razor");

        Assert.DoesNotContain("PlaceholderStart=\"DD/MM/AAAA\"", eventCatalogGrid, StringComparison.Ordinal);
        Assert.DoesNotContain("PlaceholderEnd=\"DD/MM/AAAA\"", eventCatalogGrid, StringComparison.Ordinal);
        Assert.DoesNotContain("PlaceholderStart=\"DD/MM/AAAA\"", bundleSchedulesGrid, StringComparison.Ordinal);
        Assert.DoesNotContain("PlaceholderEnd=\"DD/MM/AAAA\"", bundleSchedulesGrid, StringComparison.Ordinal);
    }

    [Fact]
    public void Additional_charge_validator_uses_existing_localized_type_label()
    {
        Assert.False(string.IsNullOrWhiteSpace(ResourceValue("Resources/SharedResource.resx", "Type")));
        Assert.False(string.IsNullOrWhiteSpace(ResourceValue("Resources/SharedResource.en.resx", "Type")));
        Assert.False(string.IsNullOrWhiteSpace(ResourceValue("Resources/SharedResource.es.resx", "Type")));

        var source = ReadAppSource("Validators/AdditionalChargeInfoValidator.cs");
        Assert.Contains(".WithName(L[\"Type\"])", source, StringComparison.Ordinal);
        Assert.DoesNotContain("L[\"FeeType\"]", source, StringComparison.Ordinal);
    }

    private static string? ResourceValue(string relativePath, string key)
    {
        var source = ReadAppSource(relativePath);
        var document = XDocument.Parse(source);
        return document
            .Root?
            .Elements("data")
            .FirstOrDefault(element => (string?)element.Attribute("name") == key)?
            .Element("value")?
            .Value;
    }

    private static string ReadAppSource(string relativePath)
    {
        var path = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "../../../..",
            "Odasoft.XBOL.AdminPortal",
            relativePath));

        return File.ReadAllText(path);
    }
}
