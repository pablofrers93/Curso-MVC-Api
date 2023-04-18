using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CursoMVC_Api.Models;
using CursoMVC_Api.Models.WS;

namespace CursoMVC_Api.Controllers
{
    public class BaseController : ApiController
    {
        public bool Verify(string token)
        {
            using (cursomvcapiEntities1 db = new cursomvcapiEntities1())
            {
                if (db.user.Where(p=>p.token == token && p.idStatus==1).Count()>0)
                {
                   return true;
                }
                else { return false; }
            }
        }
    }
}
