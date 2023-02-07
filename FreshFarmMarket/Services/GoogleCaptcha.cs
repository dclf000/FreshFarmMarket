﻿using Microsoft.Extensions.Options;
using System.Net;
using FreshFarmMarket.ViewModels;
using Newtonsoft.Json;

namespace FreshFarmMarket.Services
{
    public class GoogleCaptcha
    {
        private readonly IOptionsMonitor<GoogleCaptchaConfig> _config;
        public GoogleCaptcha(IOptionsMonitor<GoogleCaptchaConfig> config)
        {
            _config = config;

        }


        public async Task<bool> VerifyToken(string token)
        {
            try
            {
                var url = $"https://www.google.com/recaptcha/api/siteverify?secret={_config.CurrentValue.SecretKey}&response={token}";

                using (var client = new HttpClient())
                {
                    var httpResult = await client.GetAsync(url);


                    if (httpResult.StatusCode != HttpStatusCode.OK)
                    {
                        return false;
                    }
                    var responseString = await httpResult.Content.ReadAsStringAsync();
                    var googleResult = JsonConvert.DeserializeObject<GoogleCaptchaResponse>(responseString);

                    return googleResult.success && googleResult.score >= 0.5;
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
