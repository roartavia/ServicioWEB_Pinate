using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using MySql.Data.MySqlClient;
using System.Collections;
using Newtonsoft.Json;

namespace Services_Pintae
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        //recibe la cedula de un ciudano y consulta todos los trámites hechos por ese ciudadano
        public List<TramiteRealizado> ConsultaTramitesCiudadano(string cedula)
        {
            MySqlConnection connection;
            string server = "localhost";
            string database = "pintae";
            string uid = "Rodolfo";
            string password = "1234qwer";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);

            List<TramiteRealizado> listTramites = new List<TramiteRealizado>();

            try
            {
                connection.Open();

                string query = "select nombre_tramite,id_tramite_solicitado from tramite_solicitado as tramite inner join tipotramite as tipo on tramite.id_tipo_tramite = tipo.id_tramite where tramite.ciudadano = '" + cedula + "'";
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    TramiteRealizado tramiteTemp = new TramiteRealizado();
                    tramiteTemp.Id_tramite = int.Parse(dataReader["id_tramite_solicitado"].ToString());
                    tramiteTemp.Nombre_tramite = dataReader["nombre_tramite"].ToString();

                    listTramites.Add(tramiteTemp);
                }

                dataReader.Close();

                connection.Close();

                return listTramites;
            }
            catch
            {
                return listTramites;
            }
        }

        public string ConsultaTramitesPorCedulaJson(string cedula)
        {
            MySqlConnection connection;
            string server = "localhost";
            string database = "pintae";
            string uid = "Rodolfo";
            string password = "1234qwer";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);

            GetTramitesRealizados listTramites = new GetTramitesRealizados(); //lista de tramites realizados, es lo que se serializa si hay resultado.

            try {
                connection.Open();

                string query = "select nombre_tramite,id_tramite_solicitado from tramite_solicitado as tramite inner join tipotramite as tipo on tramite.id_tipo_tramite = tipo.id_tramite where tramite.ciudadano = '" + cedula + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        TramiteRealizado tramiteTemp = new TramiteRealizado();
                        tramiteTemp.Id_tramite = int.Parse(dataReader["id_tramite_solicitado"].ToString());
                        tramiteTemp.Nombre_tramite = dataReader["nombre_tramite"].ToString();

                        listTramites.listaTramites.Add(tramiteTemp);
                    }
                    dataReader.Close();
                    connection.Close();
                    return JsonConvert.SerializeObject(listTramites.listaTramites);
                }
                else
                {
                    dataReader.Close();
                    connection.Close();
                    return "-2"; //No hay trámites para ese ciudadano.
                }              
            }
            catch { return "-1"; } //Error de conexión.
        }

        //Trae la info del trámite en string formato json.
        public string ConsultaTramitePorIdJson(int id_tramite_solicitado)
        {
            MySqlConnection connection;
            string server = "localhost";
            string database = "pintae";
            string uid = "Rodolfo";
            string password = "1234qwer";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);

            Tramite tramiteConsultado = new Tramite();

            try {

                connection.Open();

                string query = "select nombre_tramite,fecha_solicitud,estado from tramite_solicitado as tramite inner join tipotramite as tipo on tramite.id_tipo_tramite = tipo.id_tramite where tramite.id_tramite_solicitado = '" + id_tramite_solicitado + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        tramiteConsultado.Nombre_tramite = dataReader["nombre_tramite"].ToString();
                        tramiteConsultado.Fecha_solicitud = dataReader["fecha_solicitud"].ToString();
                        tramiteConsultado.Estado = dataReader["estado"].ToString();
                    }
                    dataReader.Close();
                    connection.Close();

                    return JsonConvert.SerializeObject(tramiteConsultado);
                }
                else
                {
                    dataReader.Close();
                    connection.Close();
                    return "-2"; //No existe ese trámite.
                }
            }
            catch { return "-1"; } //Error de conexión.
        }
        
        //Trae el id y nombre de todos lor tipos de tramites.
        public List<TipoTramite> GetAllTipoTramites()
        {
            MySqlConnection connection;
            string server = "localhost";
            string database = "pintae";
            string uid = "Rodolfo";
            string password = "1234qwer";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);

            List<TipoTramite> listTipoTramites = new List<TipoTramite>(); //lista de tramites realizados, es lo que se serializa si hay resultado.

            try
            {
                connection.Open();

                string query = "select id_tramite,nombre_tramite from tipotramite";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        TipoTramite tipoTramiteTemp = new TipoTramite();
                        tipoTramiteTemp.Id_tramite = int.Parse(dataReader["id_tramite"].ToString());
                        tipoTramiteTemp.Nombre_tramite = dataReader["nombre_tramite"].ToString();

                        listTipoTramites.Add(tipoTramiteTemp);
                    }
                    dataReader.Close();
                    connection.Close();
                    return listTipoTramites;
                }
                else
                {
                    dataReader.Close();
                    connection.Close();
                    return listTipoTramites; //No hay tipos de tramite.
                }
            }
            catch { return null; } //Error de conexión.
        }

        //Trae el id y nombre de todos lor tipos de tramites en formato json.
        public string GetAllTipoTramitesJson()
        {
            MySqlConnection connection;
            string server = "localhost";
            string database = "pintae";
            string uid = "Rodolfo";
            string password = "1234qwer";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);

            List<TipoTramite> listTramites = new List<TipoTramite>(); //lista de tramites realizados, es lo que se serializa si hay resultado.

            try
            {
                connection.Open();

                string query = "select id_tramite,nombre_tramite from tipotramite";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        TipoTramite tramiteTemp = new TipoTramite();
                        tramiteTemp.Id_tramite = int.Parse(dataReader["id_tramite"].ToString());
                        tramiteTemp.Nombre_tramite = dataReader["nombre_tramite"].ToString();

                        listTramites.Add(tramiteTemp);
                    }
                    dataReader.Close();
                    connection.Close();
                    return JsonConvert.SerializeObject(listTramites);
                }
                else
                {
                    dataReader.Close();
                    connection.Close();
                    return "-2"; //No hay tipos de trámites.
                }
            }
            catch { return "-1"; } //Error de conexión.
        }

        //Trae el nombre y si es obligatorio de todos los datos que son requisitos para un tramite.
        public List<TipoDato> GetRequisitosPorIdTramite(int id_tramite)
        {
            MySqlConnection connection;
            string server = "localhost";
            string database = "pintae";
            string uid = "Rodolfo";
            string password = "1234qwer";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);

            List<TipoDato> requisitosParaTramite = new List<TipoDato>();

            try
            {

                connection.Open();

                string query = "SELECT td.nombre_dato, rtd.es_obligatorio  FROM pintae.tipodato td INNER JOIN pintae.requisitotramitedato rtd ON td.id_dato = rtd.id_dato WHERE rtd.id_tramite = '" + id_tramite + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        TipoDato requisitoTemp = new TipoDato();
                        requisitoTemp.Nombre_dato = dataReader["nombre_dato"].ToString();
                        requisitoTemp.Es_obligatorio = dataReader["es_obligatorio"].ToString();
                        requisitosParaTramite.Add(requisitoTemp);
                    }
                    dataReader.Close();
                    connection.Close();

                    return requisitosParaTramite;
                }
                else
                {
                    dataReader.Close();
                    connection.Close();
                    return requisitosParaTramite; //No existe ese trámite o no hay requisitos para ese tramite.
                }
            }
            catch { return null; } //Error de conexión.
        }

        //Trae el nombre y si es obligatorio de todos los datos que son requisitos para un tramite en formato json.
        public string GetRequisitosPorIdTramiteJson(int id_tramite)
        {
            MySqlConnection connection;
            string server = "localhost";
            string database = "pintae";
            string uid = "Rodolfo";
            string password = "1234qwer";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);

            List<TipoDato> requisitosParaTramite = new List<TipoDato>();

            try
            {

                connection.Open();

                string query = "SELECT td.nombre_dato, rtd.es_obligatorio  FROM pintae.tipodato td INNER JOIN pintae.requisitotramitedato rtd ON td.id_dato = rtd.id_dato WHERE rtd.id_tramite = '" + id_tramite + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        TipoDato requisitoTemp = new TipoDato();
                        requisitoTemp.Nombre_dato = dataReader["nombre_dato"].ToString();
                        requisitoTemp.Es_obligatorio = dataReader["es_obligatorio"].ToString();
                        requisitosParaTramite.Add(requisitoTemp);
                    }
                    dataReader.Close();
                    connection.Close();

                    return JsonConvert.SerializeObject(requisitosParaTramite);
                }
                else
                {
                    dataReader.Close();
                    connection.Close();
                    return "-2"; //No existe ese trámite.
                }
            }
            catch { return "-1"; } //Error de conexión.
        }
    }
}
