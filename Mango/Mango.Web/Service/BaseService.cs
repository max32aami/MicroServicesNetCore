﻿using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;
        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }
        public async Task<ResponseDto> SendAsync(RequestDto requestDto, bool withBearer = true)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("Mango");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                //Token
                if (withBearer) 
                {
                    var token = _tokenProvider.GetToken();
                    message.Headers.Add("Authorization", $"Bearer {token}");
                }
                message.RequestUri = new Uri(requestDto.Url);
                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage? apiResponse = null;

                switch (requestDto.ApiType)
                {
                    case ApiType.POST:
                        {
                            message.Method = HttpMethod.Post;
                            break;
                        }
                    case ApiType.DELETE:
                        {
                            message.Method = HttpMethod.Delete;
                            break;
                        }
                    case ApiType.PUT:
                        {
                            message.Method = HttpMethod.Put;
                            break;
                        }
                    default:
                        {
                            message.Method = HttpMethod.Get;
                            break;
                        }
                }
                apiResponse = await client.SendAsync(message);

                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        {
                            return new() { IsSucess = false, Message = "Not Found" };
                        }
                    case HttpStatusCode.Forbidden:
                        {
                            return new() { IsSucess = false, Message = "Access Denied" };
                        }
                    case HttpStatusCode.Unauthorized:
                        {
                            return new() { IsSucess = false, Message = "Un Authorised" };
                        }
                    case HttpStatusCode.InternalServerError:
                        {
                            return new() { IsSucess = false, Message = "Internal Server Error" };
                        }
                    default:
                        {
                            var apiContent = await apiResponse.Content.ReadAsStringAsync();
                            var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                            //apiResponseDto.Message = apiResponseDto.Message;
                            //apiResponseDto.IsSucess = ;

                            return apiResponseDto;
                        }
                }
            }
            catch (Exception ex) 
            {
                var dto = new ResponseDto
                {
                    IsSucess = false,
                    Message = ex.Message
                };
                return dto;
            }


        }
    }
}
