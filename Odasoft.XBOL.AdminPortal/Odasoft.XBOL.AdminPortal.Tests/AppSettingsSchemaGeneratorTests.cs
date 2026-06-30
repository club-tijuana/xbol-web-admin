using System.Text.Json.Nodes;
using Odasoft.XBOL.AdminPortal.Schema;
using Xunit;

namespace Odasoft.XBOL.AdminPortal.Tests;

public sealed class AppSettingsSchemaGeneratorTests
{
    [Fact]
    public void Generate_includes_payment_link_defaults_and_localization_timezone()
    {
        var schema = AppSettingsSchemaGenerator.Generate();

        var paymentLink = schema["properties"]?["PaymentLink"] as JsonObject;
        Assert.NotNull(paymentLink);
        Assert.Equal(
            48,
            paymentLink!["properties"]?["DefaultExpirationHours"]?["default"]?.GetValue<int>());
        Assert.Null(paymentLink["properties"]?["TimeZoneId"]);

        var localization = schema["properties"]?["Localization"] as JsonObject;
        Assert.NotNull(localization);
        Assert.Equal(
            "America/Tijuana",
            localization!["properties"]?["TimeZoneId"]?["default"]?.GetValue<string>());

        var required = localization["required"] as JsonArray;
        Assert.NotNull(required);
        Assert.Contains(required!, item => item!.GetValue<string>() == "TimeZoneId");
    }
}
