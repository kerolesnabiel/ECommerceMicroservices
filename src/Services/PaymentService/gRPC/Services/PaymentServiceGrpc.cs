using Grpc.Core;
using Stripe;

namespace PaymentService.gRPC.Services;

public class PaymentServiceGrpc : PaymentServiceProto.PaymentServiceProtoBase
{
    public override async Task<ChargeResponse> Charge(ChargeRequest request, ServerCallContext context)
    {
        try
        {
            if (string.IsNullOrEmpty(request.UserId))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "User ID cannot be empty."));

            if (request.Amount <= 0)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Amount must be greater than zero."));

            if (string.IsNullOrEmpty(request.CardToken))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Card token cannot be empty."));

            var options = new ChargeCreateOptions()
            {
                Amount = (long)request.Amount,
                Currency = request.Currency,
                Source = request.CardToken,
                Description = $"Charge for user {request.UserId}"
            };

            var charge = await CreateChargeAsync(options);

            if (charge.Status == "succeeded")
                return new ChargeResponse() { TransactionId = charge.Id };
            else
                throw new RpcException(new Status(StatusCode.Internal, charge.FailureMessage ?? "Payment failed for an unknown reason."));
        }
        catch (StripeException ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, "Stripe error: " + ex.Message));
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, "An error occurred: " + ex.Message));
        }
    }

    private async Task<Charge> CreateChargeAsync(ChargeCreateOptions options)
    {
        var service = new ChargeService();
        return await service.CreateAsync(options).ConfigureAwait(false);
    }
}
