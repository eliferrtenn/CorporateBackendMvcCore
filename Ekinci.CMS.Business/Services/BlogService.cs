﻿using Ekinci.CMS.Business.Interfaces;
using Ekinci.CMS.Business.Models.Requests.BlogRequests;
using Ekinci.CMS.Business.Models.Responses.BlogResponses;
using Ekinci.Common.Business;
using Ekinci.Common.Extentions;
using Ekinci.Data.Context;
using Ekinci.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;

namespace Ekinci.CMS.Business.Services
{
    public class BlogService : BaseService, IBlogService
    {
        const string file = "Blog/";
        public BlogService(EkinciContext context, IConfiguration configuration, IHttpContextAccessor httpContext) : base(context, configuration, httpContext)
        {
        }
        public async Task<ServiceResult> AddBlog(AddBlogRequest request, IFormFile PhotoUrl)
        {
            var result = new ServiceResult();
            var exist = await _context.Blog.FirstOrDefaultAsync(x => x.Title == request.Title);
            if (exist != null)
            {
                result.SetError("Bu başlıkta blog zaten kayıtlıdır.");
                return result;
            }
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var Blog = new Blog();
            if (PhotoUrl != null)
            {
                if (PhotoUrl.Length > 0)
                {
                    var path = Path.GetExtension(PhotoUrl.FileName);
                    var type = file + guid.ToString() + path;
                    var filePath = "wwwroot/Dosya/" + type;
                    var filePathBunnyCdn = "/ekinci/" + type;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await PhotoUrl.CopyToAsync(stream);
                    }
                    await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                    Blog.PhotoUrl = type;
                }
            }
            Blog.Title = request.Title;
            Blog.BlogDate = request.BlogDate;
            Blog.InstagramUrl = request.InstagramUrl;
            _context.Blog.Add(Blog);
            await _context.SaveChangesAsync();

            result.SetSuccess("Blog başarıyla eklendi!");
            return result;
        }

        public async Task<ServiceResult> UpdateBlog(UpdateBlogRequest request, IFormFile PhotoUrl)
        {
            Guid guid = Guid.NewGuid();
            var filePaths = new List<string>();
            var result = new ServiceResult();
            var exist = await _context.Blog.AnyAsync(x => x.Title == request.Title && x.ID != request.ID);
            if (exist == false)
            {
                var blog = await _context.Blog.FirstOrDefaultAsync(x => x.ID == request.ID);
                if (blog == null)
                {
                    result.SetError("Blog bilgisi Bulunamadı!");
                    return result;
                }
                if (PhotoUrl != null)
                {
                    if (PhotoUrl.Length > 0)
                    {
                        await bunnyCDNStorage.DeleteObjectAsync("/ekinci/" + blog.PhotoUrl);
                        var path = Path.GetExtension(PhotoUrl.FileName);
                        var type = file + guid.ToString() + path;
                        var filePath = "wwwroot/Dosya/" + type;
                        var filePathBunnyCdn = "/ekinci/" + type;
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await PhotoUrl.CopyToAsync(stream);
                        }
                        await bunnyCDNStorage.UploadAsync(filePath, filePathBunnyCdn);
                        blog.PhotoUrl = type;
                    }
                }
                else
                {
                    blog.PhotoUrl = request.PhotoUrl;
                }
                blog.Title = request.Title;
                blog.BlogDate = request.BlogDate;
                blog.InstagramUrl = request.InstagramUrl;
                _context.Blog.Update(blog);
                await _context.SaveChangesAsync();
                result.SetSuccess("Blog  başarıyla güncellendi!");
            }
            else
            {
                result.SetError("Bu başlıkta tarihçe zaten kayıtlıdır.");
            }
            return result;
        }

        public async Task<ServiceResult> DeleteBlog(DeleteBlogRequest request)
        {
            var result = new ServiceResult();
            var blog = await _context.Blog.FirstOrDefaultAsync(x => x.ID == request.ID);
            if (blog == null)
            {
                result.SetError("Blog bilgisi Bulunamadı!");
                return result;
            }
            blog.IsEnabled = false;
            _context.Blog.Update(blog);
            await _context.SaveChangesAsync();

            result.SetSuccess("Blog silindi.");
            return result;
        }

        public async Task<ServiceResult<List<ListBlogResponse>>> GetAll()
        {
            var result = new ServiceResult<List<ListBlogResponse>>();
            if (_context.Blog.Count() == 0)
            {
                result.SetError("Tarihçe bulunamadı");
                return result;
            }
            var blogs = await(from blog in _context.Blog
                                  where blog.IsEnabled == true
                                  select new ListBlogResponse
                                  {
                                      ID = blog.ID,
                                      Title = blog.Title,
                                      BlogDate = blog.BlogDate.ToFormattedDate(),
                                      InstagramUrl = blog.InstagramUrl,
                                      PhotoUrl = ekinciUrl + blog.PhotoUrl,
                                  }).ToListAsync();
            result.Data = blogs;
            return result;
        }

        public async Task<ServiceResult<GetBlogResponse>> GetBlog(int BlogID)
        {
            var result = new ServiceResult<GetBlogResponse>();
            var histories = await(from blog in _context.Blog
                                  where blog.ID == BlogID
                                  select new GetBlogResponse
                                  {
                                      ID = blog.ID,
                                      Title = blog.Title,
                                      BlogDate = blog.BlogDate.ToFormattedDate(),
                                      InstagramUrl = blog.InstagramUrl,
                                      PhotoUrl = ekinciUrl + blog.PhotoUrl,
                                  }).FirstAsync();
            if (histories == null)
            {
                result.SetError("Blog bulunamadı");
                return result;
            }
            result.Data = histories;
            return result;
        }
    }
}