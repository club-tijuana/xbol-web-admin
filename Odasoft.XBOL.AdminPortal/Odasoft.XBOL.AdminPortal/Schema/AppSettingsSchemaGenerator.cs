using Odasoft.XBOL.AdminPortal.Services;
using Odasoft.XBOL.Common.Options;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;

namespace Odasoft.XBOL.AdminPortal.Schema;

public static class AppSettingsSchemaGenerator
{
    /// <summary>
    /// Generates a JSON Schema for appsettings.json, merging custom sections
    /// with SchemaStore's built-in ASP.NET schema via allOf.
    /// </summary>
    public static JsonObject Generate()
    {
        var exporterOptions = new JsonSchemaExporterOptions
        {
            TreatNullObliviousAsNonNullable = true,
            TransformSchemaNode = (context, node) =>
            {
                var description = context.PropertyInfo?.AttributeProvider
                    ?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .OfType<DescriptionAttribute>()
                    .FirstOrDefault()?.Description;

                if (description is not null)
                {
                    node["description"] = description;
                }

                var defaultValue = context.PropertyInfo?.AttributeProvider
                    ?.GetCustomAttributes(typeof(DefaultValueAttribute), false)
                    .OfType<DefaultValueAttribute>()
                    .FirstOrDefault()?.Value;

                if (defaultValue is not null)
                {
                    node["default"] = JsonSerializer.SerializeToNode(defaultValue);
                }

                return node;
            }
        };

        var customSchema = JsonSchemaExporter.GetJsonSchemaAsNode(
            JsonSerializerOptions.Default, typeof(AppSettingsSchema), exporterOptions);

        return new JsonObject
        {
            ["allOf"] = new JsonArray
            {
                new JsonObject
                {
                    ["$ref"] = "https://json.schemastore.org/appsettings.json"
                }
            },
            ["properties"] = customSchema["properties"]?.DeepClone(),
            ["type"] = "object"
        };
    }

    /// <summary>
    /// Generates the schema and writes it to the specified file path.
    /// </summary>
    public static void GenerateAndWrite(string outputPath)
    {
        var schema = Generate();
        var json = schema.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(outputPath, json);
    }

    /// <summary>
    /// Composite type representing app-specific appsettings.json sections.
    /// Add a property here for each new Options class.
    /// </summary>
    public sealed class AppSettingsSchema
    {
        [Description("Admin API client settings")]
        public AdminApiClientOptions? AdminApiClient { get; set; }

        [Description("Firebase Web SDK authentication settings for admin users")]
        public FirebaseAuthOptions? FirebaseAuth { get; set; }

        [Description("Firebase Admin SDK authentication settings")]
        public GcipAuthOptions? GcipAuth { get; set; }

        [Description("Admin API Firebase session cookie settings")]
        public AdminSessionCookieOptions? AdminSession { get; set; }

        [Description("Seats.io integration settings")]
        public SeatsIoOptions? SeatsIo { get; set; }

        [Description("Hosting / reverse proxy settings")]
        public HostingOptions? Hosting { get; set; }
    }
}
