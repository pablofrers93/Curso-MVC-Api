using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CursoMVC_Api.Models.WS;
using CursoMVC_Api.Models;
using System.Web.UI.WebControls;

namespace CursoMVC_Api.Controllers
{
    public class AccessController : ApiController
    {
        [HttpGet]
        public Reply HelloWorld()
        {
            Reply oR = new Reply();
            oR.Result = 1;
            oR.Message = "Hi World!";

            return oR;
        }

        [HttpPost]
        public Reply Login([FromBody] AccessViewModel model)
        {
            Reply oR = new Reply();
            oR.Result = 0;
            try
            {
                using (cursomvcapiEntities1 db = new cursomvcapiEntities1())
                {
                    var lst = db.user.Where(p=>p.email==model.Email && p.password==model.Password && p.idStatus==1);

                    if (lst.Count()>0)
                    {
                        oR.Result = 1;
                        oR.Data = Guid.NewGuid().ToString();

                        user oUser = lst.First();
                        oUser.token = (string)oR.Data; 
                        db.Entry(oUser).State = System.Data.Entity.EntityState.Modified;    
                        db.SaveChanges();
                    }
                    else
                    {
                        oR.Message = "Datos incorrectos";
                    }
                }
            }
            catch (Exception ex)
            {
                oR.Result = 1;
                oR.Message = "Ocurrió un error, estamos corrigiendo";
            }

            return oR;
        }
    }
}
