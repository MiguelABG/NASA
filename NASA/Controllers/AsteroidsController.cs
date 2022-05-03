using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NASA.Client;
using Newtonsoft.Json;

namespace NASA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsteroidsController : ControllerBase
    {
        public readonly NASAClient nasaClient;

        public AsteroidsController(NASAClient client)
        {
            nasaClient = client;
        }

        // GET api/asteroids
        [HttpGet]
        public async Task<IActionResult> GetAsteroids(string planet)
        {
            if (string.IsNullOrEmpty(planet))
            {
               return Content($"Failed parameter planet is required, try again with: /asteroids?planet=[value]");
            }
        
            try
            {
                var response = await nasaClient.Get();
                
                return Content(JsonConvert.SerializeObject(response.near_earth_objects.SelectMany(n => n.Value).Select(a => new
                {
                    nombre = a.name,
                    diametro = a.estimated_diameter.kilometers.diameter_mid,
                    velocidad = a.close_approach_data.FirstOrDefault().relative_velocity.kilometers_per_hour,
                    fecha = a.close_approach_data.FirstOrDefault().close_approach_date,
                    planeta = a.close_approach_data.FirstOrDefault().orbiting_body
                }).Where(o=> o.planeta==planet).Take(3)));
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}