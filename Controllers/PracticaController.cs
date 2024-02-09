using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.IO;

namespace PracticaEval01.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PracticaController : ControllerBase
    {
        [HttpGet]
        public Result Get()
        {
            var origin = Environment.GetEnvironmentVariable("Origen");
            
            string mongousr =  Environment.GetEnvironmentVariable("MONGO_USR");
            string mongopwd =  Environment.GetEnvironmentVariable("MONGO_PWD");

            if (origin != "kubernetes")
            {
                mongousr = System.IO.File.ReadAllText(mongousr);
                mongopwd = System.IO.File.ReadAllText(mongopwd);
            }

            string connectionstring="mongodb://" + mongousr + ":" + mongopwd + "@mongodb:27017";
            
            var dbClient= new MongoClient(connectionstring);
            
            IMongoDatabase db = dbClient.GetDatabase("testdb");
            
            var cars = db.GetCollection<BsonDocument>("cars");

            var result = new Result
            {
                Version = Environment.GetEnvironmentVariable("Version"),
                Entorno = Environment.GetEnvironmentVariable("Entorno"),
                List = cars.Find(new BsonDocument()).ToList()
            };
            
            return result;
        }
    }

    public class Result
    {
        public string? Version;
        public string? Entorno;
        public object? List;
    }
}