using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CursoMVC_Api.Models.WS;
using CursoMVC_Api.Models;

namespace CursoMVC_Api.Controllers
{
    public class AnimalController : BaseController
    {
        [HttpPost]
        public Reply Get([FromBody] SecurityViewModel model)
        {
            Reply oR = new Reply();
            oR.Result = 0;

            if (!Verify(model.Token))
            {
                oR.Message = "No autorizado";
                return oR;
            }
            try
            {
                using (cursomvcapiEntities1 db = new cursomvcapiEntities1())
                {
                    List<ListAnimalsViewModel> lst = List(db);
                    oR.Data = lst;
                    oR.Result = 1;
                }
            }
            catch(Exception ex)
            {
                oR.Message = "Ocurrió un error en el servidor, intenta mas tarde";
            }
            return oR;
        }

        [HttpPost]
        public Reply Add([FromBody] AnimalViewModel model)
        {
            Reply oR = new Reply();
            oR.Result = 0;

            if (!Verify(model.Token))
            {
                oR.Message = "No autorizado";
                return oR;
            }
            //Validaciones acá
            if (!Validate(model))
            {
                oR.Message = error;
                return oR;
            }
            try
            {
                using (cursomvcapiEntities1 db = new cursomvcapiEntities1())
                {
                    animal oAnimal = new animal();
                    oAnimal.idState = 1;
                    oAnimal.name = model.Name;
                    oAnimal.patas = model.Patas;

                    db.animal.Add(oAnimal);
                    db.SaveChanges();

                    List<ListAnimalsViewModel> lst = List(db);
                    oR.Data = lst;
                    oR.Result = 1;
                }
                
            }
            catch (Exception ex)
            {
                oR.Message = ("Ocurrio un error en el servidor, intenta mas tarde");
            }
            return oR;
        }

        [HttpPut]
        public Reply Edit([FromBody] AnimalViewModel model)
        {
            Reply oR = new Reply();
            oR.Result = 0;

            if (!Verify(model.Token))
            {
                oR.Message = "No autorizado";
                return oR;
            }
            //Validaciones acá
            if (!Validate(model))
            {
                oR.Message = error;
                return oR;
            }
            try
            {
                using (cursomvcapiEntities1 db = new cursomvcapiEntities1())
                {
                    animal oAnimal = db.animal.Find(model.Id);
                    oAnimal.name = model.Name;
                    oAnimal.patas = model.Patas;
                    db.Entry(oAnimal).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    List<ListAnimalsViewModel> lst = List(db);
                    oR.Data = lst;
                    oR.Result = 1;
                }

            }
            catch (Exception ex)
            {
                oR.Message = ("Ocurrio un error en el servidor, intenta mas tarde");
            }
            return oR;
        }
        [HttpPost]
        public Reply Delete([FromBody] AnimalViewModel model)
        {
            Reply oR = new Reply();
            oR.Result = 0;

            if (!Verify(model.Token))
            {
                oR.Message = "No autorizado";
                return oR;
            }
            try
            {
                using (cursomvcapiEntities1 db = new cursomvcapiEntities1())
                {
                    animal oAnimal = db.animal.Find(model.Id);
                    oAnimal.idState = 2;
                    db.Entry(oAnimal).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    List<ListAnimalsViewModel> lst = List(db);
                    oR.Data = lst;
                    oR.Result = 1;
                }

            }
            catch (Exception ex)
            {
                oR.Message = ("Ocurrio un error en el servidor, intenta mas tarde");
            }
            return oR;
        }
        #region HELPERS

        private bool Validate(AnimalViewModel model)
        {
            if (model.Name == "")
            {
                error = "El nombre es obligatorio";
                return false;
            }
            return true;
        }

        private List<ListAnimalsViewModel> List(cursomvcapiEntities1 db)
        {
            List<ListAnimalsViewModel> lst = (from p in db.animal
                   where p.idState == 1
                   select new ListAnimalsViewModel
                   {
                       Name = p.name,
                       Patas = p.patas
                   }).ToList();
            return lst;
        }
        #endregion
    }
}
