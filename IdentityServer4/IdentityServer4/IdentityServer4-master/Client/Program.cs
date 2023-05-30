// 利用IdentityModel访问，里面包含了访问Identity Server的方法，主要用于发现端点，只需要Identity的根地址
using System;
using IdentityModel.Client;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Client
{
    class Program
    {
        // model异步， viod => asyns
        static async Task Main(string[] args)
        {
            // 创建客户端对象
            var client = new HttpClient();
            // 获取发现文档 https://localhost:5001/.well-known/openid-configuration
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            // 判断是否获取成功
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            Console.WriteLine("Input Client or User:");
            string ch = Console.ReadLine();

            if (ch == "Clinet")
            {
                // 使用发现文档响应对象（disco）的信息去IDS申请令牌,参数是请求对象
                var tokenResponse = await client.RequestClientCredentialsTokenAsync(
                    // 请求对象
                    new ClientCredentialsTokenRequest
                    {
                        Address = disco.TokenEndpoint,// 令牌的端点地址
                        ClientId = "Client_1",//客户端ID
                        ClientSecret = "Secret_1"//客户端密钥
                    }
                );

                // 判断请求是否有错误发生
                if (tokenResponse.IsError)
                {
                    Console.WriteLine(disco.Error);
                    return;
                }
                // 打印正确的令牌
                Console.WriteLine(tokenResponse.Json);

                // 利用令牌访问Api，使用Http授权头
                var apiClient = new HttpClient();
                apiClient.SetBearerToken(tokenResponse.AccessToken);
                //请求Api
                var response = await apiClient.GetAsync("https://localhost:6001/Identity");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);//Unauthorized
                    return;
                }
                //成功，读取响应字符
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
            else if(ch =="User")
            {
                //请求 pwd
                var tokenResponse = await client.RequestPasswordTokenAsync(
                    // 请求对象
                    new PasswordTokenRequest
                    {
                        Address = disco.TokenEndpoint,// 令牌的端点地址
                       
                        ClientId = "Client_role",//客户端ID
                        ClientSecret = "Secret_rote",//客户端密钥
                        UserName = "louhoin",
                        Password = "plouhoin"
                    }
                );

                // 判断请求是否有错误发生
                if (tokenResponse.IsError)
                {
                    Console.WriteLine(disco.Error);
                    return;
                }
                // 打印正确的令牌
                Console.WriteLine(tokenResponse.Json);

                // 利用令牌访问Api，使用Http授权头
                var apiClient = new HttpClient();
                apiClient.SetBearerToken(tokenResponse.AccessToken);
                //请求Api
                var response = await apiClient.GetAsync("https://localhost:6001/Identity");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);//Unauthorized
                    return;
                }
                //成功，读取响应字符
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }   

        }
    }
}
