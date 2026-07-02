using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Odasoft.XBOL.AdminPortal.Helpers;
using Odasoft.XBOL.AdminPortal.Services;
using Odasoft.XBOL.AdminPortal.States;
using Odasoft.XBOL.Business;
using Xunit;

namespace Odasoft.XBOL.AdminPortal.Tests;

public sealed class AdminCheckoutBookingSeatPayloadTests
{
    [Fact]
    public async Task BoxOfficeCheckout_BookEventSeatsAsync_sends_booking_seats_as_json_array()
    {
        var handler = new CapturingHandler();
        var client = CreateAdminClient(handler);
        var request = BuildEventBookingRequest();

#pragma warning disable CS0612
        await client.BookEventSeatsAsync(request);
#pragma warning restore CS0612

        AssertBookingSeatsArray(handler, "/api/bookings/event/book-seats");
    }

    [Fact]
    public async Task BoxOfficeSeasonCheckout_BookSeasonSeatsAsync_sends_booking_seats_as_json_array()
    {
        var handler = new CapturingHandler();
        var client = CreateAdminClient(handler);
        var request = BuildSeasonBookingRequest();

#pragma warning disable CS0612
        await client.BookSeasonSeatsAsync(request);
#pragma warning restore CS0612

        AssertBookingSeatsArray(handler, "/api/bookings/season/book-season");
    }

    [Fact]
    public async Task SeasonPassCheckout_service_sends_booking_seats_as_json_array()
    {
        var handler = new CapturingHandler();
        var service = new SeasonPassService(CreateAdminClient(handler));
        var request = BuildSeasonBookingRequest();

        await service.BookSeasonAsync(request);

        AssertBookingSeatsArray(handler, "/api/bookings/season/book-season");
    }

    [Theory]
    [InlineData("Components/Shared/CheckoutPanel.razor")]
    [InlineData("Components/Pages/BoxOfficeSeasonCheckout.razor")]
    public void Checkout_credit_controls_require_credit_transaction_permission(string componentPath)
    {
        var source = File.ReadAllText(GetSourcePath(componentPath));

        Assert.Contains("AdminPermissions.CreditTransactions.Create", source);
        Assert.Contains("AdminPermissions.CreditSettings.Read", source);
        Assert.Contains("AdminPermissionScopes.HasPermission", source);
        Assert.Contains("!_canSellOnCredit || !_canReadCreditSettings", source);
        Assert.DoesNotContain("GetQrCodeSettingsAsync", source);
        Assert.DoesNotContain("AdminPermissions.Orders.OverrideSaleWindow", source);
    }

    [Fact]
    public void AdminApi_contract_does_not_expose_qr_secret_key()
    {
        var contract = JObject.Parse(File.ReadAllText(GetBusinessPath("OpenAPIs/admin-api.json")));
        var qrEndpoint = contract["paths"]?["/api/settings/qr-code"]?["get"];
        var schemaRef = (string?)qrEndpoint?["responses"]?["200"]?["content"]?["application/json"]?["schema"]?["$ref"];

        Assert.Equal("#/components/schemas/QrCodeSettingsResponse", schemaRef);
        Assert.Null(contract["components"]?["schemas"]?["QrCodeSettingsResponse"]?["properties"]?["secretKey"]);
    }

    private static AdminClient CreateAdminClient(CapturingHandler handler)
    {
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://admin-api.test")
        };

        return new AdminClient(httpClient);
    }

    private static EventBookingRequest BuildEventBookingRequest() => new()
    {
        EventKey = "event-key",
        HoldToken = "hold-token",
        TicketType = ItemType.Ticket,
        Seats = BookingSeatMapper.ToBookingSeats(SelectedSeats()),
        ClientContact = ClientContact(),
        PaymentInfoRequest = PaymentInfo(),
        ChangeInfoRequest = ChangeInfo()
    };

    private static SeasonBookingRequest BuildSeasonBookingRequest() => new()
    {
        SeasonKey = "season-key",
        HoldToken = "hold-token",
        TicketType = ItemType.SeasonPass,
        Seats = BookingSeatMapper.ToBookingSeats(SelectedSeats()),
        ClientContact = ClientContact(),
        PaymentInfoRequest = PaymentInfo(),
        ChangeInfoRequest = ChangeInfo()
    };

    private static List<SeatInfo> SelectedSeats() =>
    [
        new()
        {
            SeatId = "B-1-1",
            Price = 375m,
            PriceListItemId = 501,
            IsSelected = true
        },
        new()
        {
            SeatId = "B-1-2",
            Price = 425m,
            PriceListItemId = 502,
            IsSelected = true
        }
    ];

    private static ClientInfoRequest ClientContact() => new()
    {
        PhoneRegionCodeId = 1,
        PhoneNumber = "5550000100",
        Email = "checkout-payload@example.com",
        FullName = "Checkout Payload",
        FirstName = "Checkout",
        LastName = "Payload"
    };

    private static PaymentInfoRequest PaymentInfo() => new()
    {
        IsCourtesy = false,
        CardAmount = 0,
        CashAmount = 800m,
        DolarAmount = 0,
        CreditAmount = 0,
        OtherAmount = 0
    };

    private static ChangeInfoRequest ChangeInfo() => new()
    {
        AmountMXN = 0,
        AmountUSD = 0
    };

    private static void AssertBookingSeatsArray(
        CapturingHandler handler,
        string expectedPath)
    {
        Assert.NotNull(handler.Request);
        Assert.Equal(expectedPath, handler.Request!.RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Post, handler.Request.Method);

        var payload = JObject.Parse(handler.RequestBody);
        var seats = Assert.IsType<JArray>(payload["seats"]);

        Assert.Equal(2, seats.Count);
        Assert.Equal("B-1-1", (string?)seats[0]?["seatKey"]);
        Assert.Equal(375m, (decimal?)seats[0]?["seatPrice"]);
        Assert.Equal(501, (long?)seats[0]?["priceListItemId"]);
        Assert.Equal("B-1-2", (string?)seats[1]?["seatKey"]);
        Assert.Equal(425m, (decimal?)seats[1]?["seatPrice"]);
        Assert.Equal(502, (long?)seats[1]?["priceListItemId"]);
    }

    private static string GetSourcePath(string relativePath)
    {
        var baseDirectory = AppContext.BaseDirectory;
        var sourceRoot = Path.GetFullPath(Path.Combine(
            baseDirectory,
            "..",
            "..",
            "..",
            "..",
            "Odasoft.XBOL.AdminPortal"));

        return Path.Combine(sourceRoot, relativePath);
    }

    private static string GetBusinessPath(string relativePath)
    {
        var baseDirectory = AppContext.BaseDirectory;
        var businessRoot = Path.GetFullPath(Path.Combine(
            baseDirectory,
            "..",
            "..",
            "..",
            "..",
            "Odasoft.XBOL.Business"));

        return Path.Combine(businessRoot, relativePath);
    }

    private sealed class CapturingHandler : HttpMessageHandler
    {
        public HttpRequestMessage? Request { get; private set; }
        public string RequestBody { get; private set; } = string.Empty;

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Request = request;
            RequestBody = request.Content is null
                ? string.Empty
                : await request.Content.ReadAsStringAsync(cancellationToken);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        orderId = 123,
                        reference = "checkout-payload",
                        bookedSeatKeys = new[] { "B-1-1", "B-1-2" },
                        ticketIds = Array.Empty<long>(),
                        bundlePassIds = Array.Empty<long>(),
                        clientId = 456,
                        total = 800m
                    }),
                    Encoding.UTF8,
                    "application/json")
            };
        }
    }
}
