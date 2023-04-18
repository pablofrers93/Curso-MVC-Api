using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CursoMVC_Api.Models.WS
{
    public class AnimalViewModel:SecurityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Patas { get; set; }
    }
}