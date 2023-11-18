using System.Text.Json;
using VentaControlEstampillas.Domain.Dto;
using VentaControlEstampillas.Domain.Entities;
using VentaControlEstampillas.Infrastructure.Ports;
using VentaControlEstampillas.Application.Voters;
using VentaControlEstampillas.Domain.Ports;
using Microsoft.Extensions.DependencyInjection;


namespace VentaControlEstampillas.Api.Tests;

public class VoterApiTest
{
    [Fact]
        public async Task GetSingleClientsSuccess()
        {            
            await using var webApp = new ApiApp();
            var serviceCollection = webApp.GetServiceCollection();
            using var scope = serviceCollection.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<Voter>>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var voter = new Voter("1234567890", DateTime.Now.AddYears(-18), "Colombia");                
            var voterCreated = await repository.AddAsync( new Voter("1234567890", DateTime.Now.AddYears(-18), "Colombia"){ Id = webApp.UserId});
            await unitOfWork.SaveAsync(new CancellationTokenSource().Token);
            var client = webApp.CreateClient();
            var singleVoter = await client.GetFromJsonAsync<VoterDto>($"/api/voter/{voterCreated.Id}");
            Assert.True(singleVoter is not null && singleVoter is VoterDto);
            Assert.Equal(singleVoter.Id,webApp.UserId);                            
        }


        [Fact]
        public async Task PostClientsSuccess()
        {            
            await using var webApp = new ApiApp();                                
            VoterRegisterCommand voter = new("123456789", "Colombia", DateTime.Now.AddYears(-20));                                              
            var client = webApp.CreateClient();
            var request = await client.PostAsJsonAsync<VoterRegisterCommand>("/api/voter/",voter);
            request.EnsureSuccessStatusCode();   
            var deserializeOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };  
            var responseData =  System.Text.Json.JsonSerializer.Deserialize<VoterDto>(await request.Content.ReadAsStringAsync(), deserializeOptions);
            Assert.True(responseData is not null);
            Assert.IsType<VoterDto>(responseData);          
        }

        [Fact]
        public async Task PostClientsFailureByAge()
        {
            HttpResponseMessage request = default!;
            try
            {
                await using var webApp = new ApiApp();                                
                VoterRegisterCommand voter = new("123456789", "Colombia", DateTime.Now.AddYears(-16));                                              
                var client = webApp.CreateClient();
                request = await client.PostAsJsonAsync<VoterRegisterCommand>("/api/voter/",voter);                
                request.EnsureSuccessStatusCode();                
                Assert.Fail("There's no way to get here if voter is underage");
            }
            catch (Exception)
            {
                var responseMessage = await request.Content.ReadAsStringAsync();
                Assert.True(request.StatusCode is HttpStatusCode.BadRequest && responseMessage.Contains("Voter is not 18 years or older"));
            }
        }
}

