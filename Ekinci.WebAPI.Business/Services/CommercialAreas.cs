﻿using Ekinci.Common.Business;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Responses.CommercialAreaResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ekinci.WebAPI.Business.Services
{
    public class CommercialAreas : BaseService, ICommercialAreaService
    {
        public CommercialAreas(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult<List<ListCommercialAreasResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListCommercialAreasResponse>>();
            var commercials = await (from commercial in _context.CommercialAreas
                                     select new ListCommercialAreasResponse
                                     {
                                         ID = commercial.ID,
                                         Title = commercial.Title,
                                         PhotoUrl = commercial.PhotoUrl,
                                         //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                                     }).ToListAsync();
            if (commercials == null)
            {
                result.SetError("Ticari Alan yoktur");
                return result;
            }
            result.Data = commercials;
            return result; ;
        }

        public async Task<ServiceResult<GetCommercialAreaResponse>> GetCommercialArea(int CommercialAreaID)
        {
            var result = new ServiceResult<GetCommercialAreaResponse>();

            var commercials = await (from commercial in _context.CommercialAreas
                                     where commercial.ID == CommercialAreaID
                                     select new GetCommercialAreaResponse
                                     {
                                         ID = commercial.ID,
                                         Title = commercial.Title,
                                         PhotoUrl = commercial.PhotoUrl,
                                         //TODO : resim kaydettiğin yere göre profilePhotoUrl i değiştir ve tam adres gönder.
                                     }).FirstAsync();
            if (commercials == null)
            {
                result.SetError("Ticari alan bulunamadı");
                return result;
            }
            result.Data = commercials;
            return result;
        }
    }
}