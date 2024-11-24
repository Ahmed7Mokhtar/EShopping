using Discount.Grpc.Protos;

namespace Basket.Application.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
        {
            _discountProtoServiceClient = discountProtoServiceClient;
        }

        public async Task<CouponModel> GetDiscount(string productId)
        {
            var discount = new GetDiscountRequest { ProductId = productId };
            return await _discountProtoServiceClient.GetDiscountAsync(discount);
        }
    }
}
