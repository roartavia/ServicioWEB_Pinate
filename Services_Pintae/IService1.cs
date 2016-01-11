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
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        string ConexionPrueba();

        [OperationContract]
        List<TramiteRealizado> ConsultaTramitesCiudadano(string cedula);

        [OperationContract]
        string ConsultaTramitesPorCedulaJson(string cedula);
        // TODO: Add your service operations here

        [OperationContract]
        string ConsultaTramitePorIdJson(int id);

        [OperationContract]
        string ConsultaTodosLosTipoDeTramites();
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
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
        int id_tipo_tramite = 0;
        
        [DataMember]
        public string Nombre_tramite
        {
            get { return nombre_tramite; }
            set { nombre_tramite = value; }
        }

        [DataMember]
        public int Id_tipo_tramite
        {
            get { return id_tipo_tramite; }
            set { id_tipo_tramite = value; }
        }    
    }
}
