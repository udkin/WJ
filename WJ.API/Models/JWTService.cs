using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WJ.API.Models
{
    public class JWTService
    {
        #region 单列模式
        private static JWTService _instance = null;

        private JWTService() { }

        public static JWTService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("TokenService")
                        if (_instance == null)
                            _instance = new JWTService();

                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// 创建token
        /// </summary>
        /// <param name="authInfo"></param>
        /// <returns></returns>
        public string CreateToken(AuthInfo authInfo)
        {
            string secret = System.Configuration.ConfigurationManager.AppSettings[""];

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(authInfo, secret);
            //Console.WriteLine(token);
            return token;
        }

        /// <summary>
        /// 解析token提取用户授权信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public AuthInfo DecodeToken(string token)
        {
            string secret = System.Configuration.ConfigurationManager.AppSettings[""];
            AuthInfo authInfo = null;

            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

            try
            {
                authInfo = decoder.DecodeToObject<AuthInfo>(token, secret, verify: true);
            }
            catch { }
            return authInfo;
        }
    }
}