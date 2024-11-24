using AutoMapper;
using Discount.Core.Entities;
using Discount.Grpc.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Mappers
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            // Map Coupon to gRPC CouponModel
            CreateMap<Coupon, CouponModel>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => (long)(src.Amount * 100))); // Convert decimal to cents

            // Map gRPC CouponModel to Coupon
            CreateMap<CouponModel, Coupon>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount / 100M)); // Convert cents to decimal


            // Map CreateDiscountRequest to Coupon
            CreateMap<CreateDiscountRequest, Coupon>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Id is often generated, so ignore it
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Coupon.ProductId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Coupon.Description))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Coupon.Amount / 100M)); // cents -> decimal

            // Map UpdateDiscountRequest to Coupon
            CreateMap<UpdateDiscountRequest, Coupon>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Coupon.Id))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Coupon.ProductId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Coupon.Description))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Coupon.Amount / 100M)); // cents -> decimal
        }
    }
}
