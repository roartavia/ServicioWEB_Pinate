using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Collections;

namespace Services_Pintae
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        List<TramiteRealizado> ConsultaTramitesCiudadano(string cedula);

        [OperationContract]
        string ConsultaTramitesPorCedulaJson(string cedula);

        [OperationContract]
        string ConsultaTramitePorIdJson(int id);

        [OperationContract]
        List<TipoTramite> GetAllTipoTramites();

        [OperationContract]
        string GetAllTipoTramitesJson();

        [OperationContract]
        List<TipoDato> GetRequisitosPorIdTramite(int id_tramite);

        [OperationContract]
        string GetRequisitosPorIdTramiteJson(int id_tramite);
    }

    [DataContract]
    public class TramiteRealizado
    {
        int id_tramite = 0;
        string nombre_tramite = "";
      
        [DataMember]
        public int Id_tramite
        {
            get { return id_tramite; }
            set { id_tramite = value; }
        }

        [DataMember]
        public string Nombre_tramite
        {
            get { return nombre_tramite; }
            set { nombre_tramite = value; }
        }        
    }

    [DataContract]
    public class GetTramitesRealizados
    {
        public List<TramiteRealizado> listaTramites = new List<TramiteRealizado>();       
    }

    public class GetTiposTramites
    {
        public List<TipoTramite> listaTiposTramites = new List<TipoTramite>();
    }

    [DataContract]
    public class Tramite
    {
        string nombre_tramite = "";
        string fecha_solicitud = "";
        string estado = "";

        [DataMember]
        public string Nombre_tramite
        {
            get { return nombre_tramite; }
            set { nombre_tramite = value; }
        }

        [DataMember]
        public string Fecha_solicitud
        {
            get { return fecha_solicitud; }
            set { fecha_solicitud = value; }
        }

        [DataMember]
        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }
    }

    [DataContract]
    public class TipoTramite
    {
        string nombre_tramite = "";
        int id_tramite = 0;
        
        [DataMember]
        public string Nombre_tramite
        {
            get { return nombre_tramite; }
            set { nombre_tramite = value; }
        }

        [DataMember]
        public int Id_tramite
        {
            get { return id_tramite; }
            set { id_tramite = value; }
        }    
    }

    [DataContract]
    public class TipoDato
    {
        string nombre_dato = "";
        string es_obligatorio = "";

        [DataMember]
        public string Nombre_dato
        {
            get { return nombre_dato; }
            set { nombre_dato = value; }
        }

        [DataMember]
        public string Es_obligatorio
        {
            get { return es_obligatorio; }
            set { es_obligatorio = value; }
        }
    }
}
