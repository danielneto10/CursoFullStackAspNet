using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebCsharp.API.Data;
using WebCsharp.API.Models;

namespace WebCsharp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly DataContext dataContext;

        public EventosController(DataContext dataContext)
        {
            this.dataContext = dataContext;

        }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return dataContext.Eventos;
        }

        [HttpGet("{id}")]
        public Evento GetById(int id)
        {
            return dataContext.Eventos.FirstOrDefault(x => x.EventoId == id);
        }

        [HttpPost]
        public string Post()
        {
            return "Exemplo de post";
        }

        [HttpPut("{id}")]
        public string Put(int id)
        {
            return $"Exemplo de put com id = {id}";
        }

        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            return $"Exemplo delete com id = {id}";
        }
    }
}
