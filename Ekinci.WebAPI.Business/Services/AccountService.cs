﻿using Ekinci.Common.Business;
using Ekinci.Common.Helpers;
using Ekinci.Common.JwtModels;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Ekinci.WebAPI.Business.Interfaces;
using Ekinci.WebAPI.Business.Models.Requests.AccountRequests;
using Ekinci.WebAPI.Business.Models.Responses.AccountResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Ekinci.WebAPI.Business.Services
{
    public class AccountService : BaseService, IAccountService
    {

        public AccountService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }

        public async Task<ServiceResult<LoginResponse>> Login(LoginRequest request)
        {
            var result = new ServiceResult<LoginResponse>();
            var member = await _context.Members.FirstOrDefaultAsync(x => x.MobilePhone == request.MobilePhone);
            if (member == null)
            {
                member = new Member
                {
                    MobilePhone = request.MobilePhone,
                    IsDeleted = false,
                    IsEnabled = false,
                    CreatedDate = DateTime.Now
                };
                await _context.Members.AddAsync(member);
                await _context.SaveChangesAsync();
                result.Data.IsNewUser = true;
            }
            else
            {
                result.Data.IsNewUser = false;
            }
            var smscode = KeyGenerator.CreateRandomNumber(1000, 9999);

            //TODO : sms gönderme işlemi.

            member.SmsCode = smscode.ToString();
            member.SmsCodeExpireDate = DateTime.Now.AddMinutes(3);
            _context.Members.Update(member);
            await _context.SaveChangesAsync();

            result.SetSuccess("Sms gönderildi.");
            return result;
        }

        public async Task<ServiceResult<Token>> LoginSmsVerification(LoginSmsVerificationRequest request)
        {
            var result = new ServiceResult<Token>();
            var member = await _context.Members.FirstOrDefaultAsync(x => x.MobilePhone == request.MobilePhone);
            if (member == null)
            {
                result.SetError("Kullanıcı bulunamadı.");
                return result;
            }

            if (member.SmsCode != request.SmsCode)
            {
                result.SetError("Sms kodu yanlış.");
                return result;
            }
            if (DateTime.Now > member.SmsCodeExpireDate)
            {
                result.SetError("Süre geçti..");
                return result;
            }

            CustomTokenHandler tokenHandler = new(_configuration);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, member.ID.ToString()),
                new Claim(ClaimTypes.Name, member.FullName),
                new Claim(ClaimTypes.MobilePhone, member.MobilePhone)
             };
            var token = tokenHandler.CreateAccessToken(claims);
            result.Data = token;
            return result;
        }
    }
}