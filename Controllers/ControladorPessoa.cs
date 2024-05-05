using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace PROVA_N2.Controllers
{
    [ApiController]
    [Route("CreditScore")]
    public class ControladorPessoa : ControllerBase
    {
        private const string CsvCacheKey = "CsvData";
        private readonly IMemoryCache _cache;
        private readonly HttpClient _httpClient;

        public ControladorPessoa(IMemoryCache memoryCache, HttpClient httpClient)
        {
            _cache = memoryCache;
            _httpClient = httpClient;
        }

        [HttpGet("CsvData",Name = "GetCsvData")]
        public async Task<IEnumerable<string>> GetCsvData()
        {
            string csvContent;
            if (!_cache.TryGetValue(CsvCacheKey, out csvContent))
            {
                var response = await _httpClient.GetAsync(Constants.ENDERECOBLOB);
                response.EnsureSuccessStatusCode(); // Throw if not successful
                csvContent = await response.Content.ReadAsStringAsync();
                _cache.Set(CsvCacheKey, csvContent, new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(20)
                });
            }
            return csvContent.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        }

        [HttpGet(Name = "Buscar o Score")]
        public async Task<IActionResult> Get([FromQuery] PessoaConsulta pessoaConsulta)
        {
            try
            {
                if (pessoaConsulta == null)
                {
                    return BadRequest("Dados inválidos");
                }

                var csvData = await GetCsvData();
                List<CarInsuranceModel> dataset;

                using (var reader = new StringReader(string.Join(Environment.NewLine, csvData)))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {   
                    csv.Context.RegisterClassMap<CarInsuranceModelMap>();
                    dataset = csv.GetRecords<CarInsuranceModel>().ToList();
                }

                var registro = dataset.FirstOrDefault(x => x.Age == pessoaConsulta.GetAgeRange() &&
                                                            x.Gender == pessoaConsulta.Gender &&
                                                            x.DrivingExperience == pessoaConsulta.GetDrivingExperienceRange() &&
                                                            x.Education == pessoaConsulta.Education &&
                                                            x.Income == pessoaConsulta.Income &&
                                                            x.VehicleYear == pessoaConsulta.GetVehicleYear() &&
                                                            x.VehicleType == pessoaConsulta.VehicleType &&
                                                            x.AnnualMileage == pessoaConsulta.AnnualMileage);
                if (registro == null)
                {
                    return NotFound("Registro não encontrado");
                }
                return Ok(registro.CreditScore);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest("Falha ao obter o conteúdo do CSV: " + ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Chamou o endpoint mas deu erro: " + Constants.CAMINHO_ARQUIVO + ex.Message + ex.StackTrace);
            }
        }
    }
}
