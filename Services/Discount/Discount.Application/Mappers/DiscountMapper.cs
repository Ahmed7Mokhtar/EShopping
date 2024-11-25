using Discount.Core.Entities;
using Discount.Grpc.Protos;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Mappers
{
    [Mapper]
    public partial class DiscountMapper
    {
        [MapProperty(nameof(Coupon.Id), nameof(CouponModel.Id))]
        [MapProperty(nameof(Coupon.ProductId), nameof(CouponModel.ProductId))]
        [MapProperty(nameof(Coupon.Description), nameof(CouponModel.Description))]
        [MapProperty(nameof(Coupon.Amount), nameof(CouponModel.Amount))]
        public partial CouponModel Map(Coupon coupon);

        [MapProperty(nameof(CouponModel.Id), nameof(Coupon.Id))]
        [MapProperty(nameof(CouponModel.ProductId), nameof(Coupon.ProductId))]
        [MapProperty(nameof(CouponModel.Description), nameof(Coupon.Description))]
        [MapProperty(nameof(CouponModel.Amount), nameof(Coupon.Amount))]
        public partial Coupon Map(CouponModel coupon);

        private long AmountDecimalToLong(decimal amount) => (long)amount * 100;

        private decimal AmountLongToDecimal(long amount) => amount / 100M;
    }
}
