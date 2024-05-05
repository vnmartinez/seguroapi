using Microsoft.AspNetCore.Mvc;

namespace PROVA_N2.Controllers
{
    [ApiController]
    [Route("CreditScore")]
    public class ControladorPessoa : ControllerBase
    {       
        [HttpGet]
        public IActionResult Get([FromQuery] PessoaConsulta pessoaConsulta)
        {
            try
           {
                if (pessoaConsulta == null) { 
                    return BadRequest("Dados inválidos");
                }
                var dados = new CarregaDadosCSV().Search();
                
                var registro = dados.FirstOrDefault(x => x.Age == pessoaConsulta.GetAgeRange() &&
                                                        x.Gender == pessoaConsulta.Gender &&
                                                        x.DrivingExperience == pessoaConsulta.GetDrivingExperienceRange() &&
                                                        x.Education == pessoaConsulta.Education &&
                                                        x.Income == pessoaConsulta.Income &&
                                                        x.VehicleYear == pessoaConsulta.GetVehicleYear() &&
                                                        x.VehicleType == pessoaConsulta.VehicleType &&
                                                        x.AnnualMileage == pessoaConsulta.AnnualMileage);
                if (registro == null) {
                    return NotFound("Registro não encontrado");
                }
                return Ok(registro.CreditScore);   
            } catch (System.Exception ex) {
                return BadRequest("Chamou o endpoint mas deu erro: " + ex.Message + ex.StackTrace + Constants.CAMINHO_ARQUIVO);
            }
        }
    }
}
