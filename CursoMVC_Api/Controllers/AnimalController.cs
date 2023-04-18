using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CursoMVC_Api.Models.WS;
using CursoMVC_Api.Models;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Text;

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
        [HttpDelete]
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

        [HttpPost]
        public async Task<Reply> Photo([FromUri] AnimalPictureViewModel model)
        {
            Reply oR = new Reply();
            oR.Result = 0;

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            if (!Verify(model.Token))
            {
                oR.Message = "No autorizado";
                return oR;
            }
            // viene multipart

            if (!Request.Content.IsMimeMultipartContent())
            {
                oR.Message = "No viene imagen";
                return oR;
            }

            await Request.Content.ReadAsMultipartAsync(provider);

            FileInfo fileInfoPicture = null;

            foreach (MultipartFileData filedata in provider.FileData)
            {
                if (filedata.Headers.ContentDisposition.Name.Replace("\\", "").Replace("/", "").Equals("picture"))
                {
                    fileInfoPicture = new FileInfo(filedata.LocalFileName);
                }
            }

            if (fileInfoPicture != null)
            {
                using (FileStream fs = fileInfoPicture.Open(FileMode.Open, FileAccess.Read))
                {
                    byte[] b = new byte[fileInfoPicture.Length];
                    UTF8Encoding temp = new UTF8Encoding(true);
                    while (fs.Read(b, 0, b.Length) > 0) ;

                    try
                    {
                            using (cursomvcapiEntities1 db = new cursomvcapiEntities1())
                            {
                                var oAnimal = db.animal.Find(model.Id);
                                oAnimal.picture = b;
                                db.Entry(oAnimal).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                                oR.Result = 1;
                            }
                    }
                    catch (Exception)
                    {

                            oR.Message = "Intenta mas tarde";
                    }
                }
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
