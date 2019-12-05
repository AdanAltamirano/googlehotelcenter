using System;
using APIServices.Models;
using APIServices.Models.DTO;
using System.Linq;
using System.Collections.Generic;
using APIServices.Extension;

namespace APIServices
{
    public class PortalsService
    {
        public OzHotelesEntities DbContextHoteles = new OzHotelesEntities(); // coorporativo
        public OzUniEntities DbContextUnivist = new OzUniEntities();  // portales

        /// <summary>
        /// Search Coorporative by hotel
        /// </summary>
        /// <param name="id"></param>
        /// <returns>All portals by Coorporative</returns>
        public List<Portal> GetPortals(int id) 
        {
            List<Portal> portals = new List<Portal>();

           int? idCorp = DbContextHoteles.Hoteles.
                FirstOrDefault(h => h.idHotel == id).idCorporativo;

            if(idCorp != null)
            {
                string corp = DbContextHoteles.Corporativos.
                    FirstOrDefault(c => c.idCorporativo == idCorp).NombreCorp;

                portals = DbContextUnivist.Portales.
                Where(p => p.idCorporativo == idCorp).
                Select(s => new Portal { Id = s.IdPortal, Name = s.Nombre, Corp = corp, WebPage = s.PaginaWeb }).
                OrderBy(o => o.Id).
                ToList();
            }

            return portals;
        }

        /// <summary>
        /// Search all coorporatives
        /// </summary>
        /// <returns>All Coorporatives</returns>
        public List<Coorporative>GetCoorporatives()
        {
            var coorporatives = DbContextHoteles.Corporativos.
                Select(s => new Coorporative { Id = s.idCorporativo, Name = s.NombreCorp }).
                OrderBy(o => o.Name).
                ToList();

            return coorporatives;
        }

        /// <summary>
        /// Create a new coorporative
        /// </summary>
        /// <param name="coorporative"></param>
        /// <returns>Boolean if coorporative was created </returns>
        public bool CreateCoorporative(Coorporative coorporative)
        {
           
            var corporatesList = DbContextHoteles.Corporativos
                .Select(s => s.NombreCorp)
                .ToList();

            string corps = coorporative.Name.RemoveAllWhiteSpaces();
           
            for(int i = 0; i < corporatesList.Count; i++)
            {
                string corporate = corporatesList.ElementAt(i).RemoveAllWhiteSpaces();
                if (String.Equals(corporate, corps,StringComparison.CurrentCultureIgnoreCase))
                    return false;
            }

            using (System.Data.Entity.DbContextTransaction transaction = DbContextHoteles.Database.BeginTransaction())
            {
                try
                {


                    Corporativos corp = new Corporativos();
                    corp.NombreCorp = coorporative.Name.ToUpper().TrimEnd();
                    corp.idEmpresa = coorporative.CompanyId;
                    corp.EmailContact = coorporative.Email;
                    corp.FacturacionGrupal = coorporative.Billing;
                    corp.MostrarCatalogo = coorporative.Catalog;
                    corp.Type = coorporative.Type;
                    DbContextHoteles.Corporativos.Add(corp);
                    DbContextHoteles.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Create portals 
        /// </summary>
        /// <param name="portals"></param>
        /// <returns>Boolean if portals were created</returns>
        public string CreatePortals(NewPortal portals)
        {

            string webPage = "https://secure.internetpower.com.mx/portals/";
            string booking = "http://booking.internetpower.com.mx/";

            var portalsList = DbContextUnivist.Portales
                .Select(s => s.Nombre)
                .ToList();

            string hotelName = portals.Name.TrimEnd() + " - Espanol";

           for(int i = 0; i < portalsList.Count; i++)
           {
                if (portalsList.ElementAt(i).Contains(hotelName))
                    return "El hotel ya cuenta con portales";
           }


            bool exist = DbContextUnivist.Portales
                .Where(p => p.PaginaWeb != ""
                && (p.PaginaWeb.Contains(webPage) || p.PaginaWeb.Contains(booking)))
                .ToList()
                .Any(p => p.PaginaWeb.ToLower().Contains(portals.Application.ToLower().RemoveAllWhiteSpaces()));

            if (exist)
                return "El nombre de la aplicacion ya existe";


            string[] suffix = new string[4];
            suffix[0] = " - Espanol";
            suffix[1] = " - Ingles";
            suffix[2] = " - Espanol Movil";
            suffix[3] = " - Ingles Movil";
        
            using (System.Data.Entity.DbContextTransaction transaction = DbContextUnivist.Database.BeginTransaction())
            {
                try
                {

                    var lastId = DbContextUnivist.Portales.
                        Take(1).
                        OrderByDescending(o => o.IdPortal).
                        FirstOrDefault().
                        IdPortal;
                   
                   for(int i = 0; i < 4; i++)
                   {
                        Portales portal = new Portales();
                        portal.IdPortal = ++lastId; 
                        portal.Nombre = portals.Name.TrimEnd() + suffix[i];
                        portal.Descripcion = portals.Description.TrimEnd() + suffix[i];
                        portal.PaginaWeb = (i >= 2)? "" 
                            : (i == 0)? webPage + portals.Application
                            .Capitalize()
                            .ReplaceHotel()
                            .RemoveAllWhiteSpaces()
                            : webPage + portals.Application
                            .Capitalize()
                            .ReplaceHotel()
                            .RemoveAllWhiteSpaces()
                             + "Eng";
                        portal.idCorporativo = portals.CoorporativeId;
                        portal.idasociacion = portals.AsosciationId;
                        portal.IdSegmento = portals.SegmentId;
                        portal.activo = portals.Active;
                        portal.idAfiliate = portals.AfiliationId;
                        portal.idempresa = portals.CompanyId;
                        DbContextUnivist.Portales.Add(portal);
                    }
                    DbContextUnivist.SaveChanges();
                    transaction.Commit();
                }
                catch(Exception e)
                {
                    var msg = e.Message;
                    transaction.Rollback();
                    return "No se crearon los portales";
                }
                string response = portals.Name + suffix[0] + " - " + webPage + portals.Application;
                return response;
            }
        }

    }
}
