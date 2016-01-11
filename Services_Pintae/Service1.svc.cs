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
        public string ConexionPrueba()
        {
            MySqlConnection connection;
            string server = "localhost";
            string database = "pintae";
            string uid = "Rodolfo";
            string password = "1234qwer";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                string query = "insert into instituciones (nombre_institucion) values ('MICITT')";
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                string nombre = "";

                while (dataReader.Read())
                {
                    nombre = dataReader["nombre_institucion"].ToString();
                }
                dataReader.Close();
                
                connection.Close();

                return "done";
            }
            catch
            {
                return "-1";
            }
        }


        //sin json
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

        public string ConsultaTodosLosTipoDeTramites()
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

            try
            {
                connection.Open();

                string query = "select nombre_tramite,fecha_solicitud,estado from tramite_solicitado as tramite inner join tipotramite as tipo on tramite.id_tipo_tramite = tipo.id_tramite where tramite.id_tramite_solicitado = '" + '1' + "'";
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    if (dataReader.HasRows)
                    {
                        tramiteConsultado.Nombre_tramite = dataReader["nombre_tramite"].ToString();
                        tramiteConsultado.Fecha_solicitud = dataReader["fecha_solicitud"].ToString();
                        tramiteConsultado.Estado = dataReader["estado"].ToString();
                    }
                    else
                    {
                        dataReader.Close();
                        connection.Close();
                        return "No existe ese trámite";
                    }
                }
                dataReader.Close();
                connection.Close();

                return JsonConvert.SerializeObject(tramiteConsultado);
            }
            catch { return "Error"; }
        }


        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
