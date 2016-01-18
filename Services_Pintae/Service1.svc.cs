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
using System.Data;

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

                string query = "select nombre_tramite,id_tramite_solicitado,fecha_solicitud,estado from tramite_solicitado as tramite inner join tipotramite as tipo on tramite.id_tipo_tramite = tipo.id_tramite where tramite.ciudadano = '" + cedula + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        TramiteRealizado tramiteTemp = new TramiteRealizado();
                        tramiteTemp.Id_tramite = int.Parse(dataReader["id_tramite_solicitado"].ToString());
                        tramiteTemp.Nombre_tramite = dataReader["nombre_tramite"].ToString();
                        tramiteTemp.Fecha_solicitud = dataReader["fecha_solicitud"].ToString();
                        tramiteTemp.Estado = dataReader["estado"].ToString();
                        listTramites.Add(tramiteTemp);
                    }
                    dataReader.Close();
                    connection.Close();

                    return listTramites;
                }
                else
                {
                    dataReader.Close();
                    connection.Close();
                    return listTramites; //No hay trámites para ese ciudadano.
                }
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

                string query = "select nombre_tramite,id_tramite_solicitado,fecha_solicitud,estado from tramite_solicitado as tramite inner join tipotramite as tipo on tramite.id_tipo_tramite = tipo.id_tramite where tramite.ciudadano = '" + cedula + "'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        TramiteRealizado tramiteTemp = new TramiteRealizado();
                        tramiteTemp.Id_tramite = int.Parse(dataReader["id_tramite_solicitado"].ToString());
                        tramiteTemp.Nombre_tramite = dataReader["nombre_tramite"].ToString();
                        tramiteTemp.Fecha_solicitud = dataReader["fecha_solicitud"].ToString();
                        tramiteTemp.Estado = dataReader["estado"].ToString();
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

        //soolicita trámite
        public string SolicitarTramite(int id_tramite, string cedula)
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

                string query = "insert into tramite_solicitado (id_tipo_tramite, ciudadano,fecha_solicitud,estado) values ("+id_tramite+",'"+cedula+"',curdate(),'PENDIENTE')";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteReader();
                connection.Close();

                return "1";
                
            }
            catch { return "-1"; } //Error de conexión.
        }

        //consultar que peticiones estan de datos que la institución pueda ofrecer
        public List<Peticion> ConsultarPeticiones(int id_institucion)
        {
            MySqlConnection connection;
            string server = "localhost";
            string database = "pintae";
            string uid = "Rodolfo";
            string password = "1234qwer";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);

            List<Peticion> listPeticiones = new List<Peticion>();

            try
            {

                connection.Open();

                string query = "select contandoDatosYaEntregados.id_dato, contandoDatosYaEntregados.ciudadano, contandoDatosYaEntregados.id_tramite_solicitado " +
                "from (select ds.ciudadano, ds.id_tipodato from dato_solicitado ds) datosEntregados " +
                "right join " +
                "(select tipodatoinstitucion.id_dato, temp.ciudadano, temp.id_tramite_solicitado from " +
                "(select rtd.id_dato, rtd.id_tramite, td.id_institucion from requisitotramitedato rtd " +
                "inner join tipodato td on td.id_dato = rtd.id_dato) as tipodatoinstitucion " +
                "inner join(select ts.id_tipo_tramite, ts.ciudadano, ts.id_tramite_solicitado from tramite_solicitado ts where ts.estado = 'PENDIENTE') as temp " +
                "on tipodatoinstitucion.id_tramite = temp.id_tipo_tramite where tipodatoinstitucion.id_institucion = "+id_institucion+") " +
                "as contandoDatosYaEntregados " +
                "on datosEntregados.id_tipodato = contandoDatosYaEntregados.id_dato and datosEntregados.ciudadano = contandoDatosYaEntregados.ciudadano " +
                "where datosEntregados.id_tipodato is null and datosEntregados.ciudadano is null";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        Peticion peticion = new Peticion();
                        peticion.Id_tipo_dato = int.Parse(dataReader["id_dato"].ToString());
                        peticion.Id_tramite_solicitado = int.Parse(dataReader["id_tramite_solicitado"].ToString());
                        peticion.Cedula = dataReader["ciudadano"].ToString();
                        listPeticiones.Add(peticion);
                    }
                    dataReader.Close();
                    connection.Close();

                    return listPeticiones;
                }
                else
                {
                    dataReader.Close();
                    connection.Close();
                    return listPeticiones; //No hay peticiones.
                }
            }
            catch { return null; } //Error de conexión.
        }

        //consultar que peticiones estan de datos que la institución pueda ofrecer
        public string ConsultarPeticionesJson(int id_institucion)
        {
            MySqlConnection connection;
            string server = "localhost";
            string database = "pintae";
            string uid = "Rodolfo";
            string password = "1234qwer";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);

            List<Peticion> listPeticiones = new List<Peticion>();

            try
            {

                connection.Open();

                string query = "select contandoDatosYaEntregados.id_dato, contandoDatosYaEntregados.nombre_dato, contandoDatosYaEntregados.ciudadano, contandoDatosYaEntregados.id_tramite_solicitado " +
"from (select ds.ciudadano, ds.id_tipodato from dato_solicitado ds) datosEntregados " +
"right join " +
"(select tipodatoinstitucion.id_dato, tipodatoinstitucion.nombre_dato, temp.ciudadano, temp.id_tramite_solicitado from " +
"(select rtd.id_dato, rtd.id_tramite, td.id_institucion, td.nombre_dato from requisitotramitedato rtd " +
"inner join tipodato td on td.id_dato = rtd.id_dato) as tipodatoinstitucion " +
"inner join(select ts.id_tipo_tramite, ts.ciudadano, ts.id_tramite_solicitado from tramite_solicitado ts where ts.estado = 'PENDIENTE') as temp " +
"on tipodatoinstitucion.id_tramite = temp.id_tipo_tramite where tipodatoinstitucion.id_institucion = " + id_institucion + ") " +
"as contandoDatosYaEntregados " +
"on datosEntregados.id_tipodato = contandoDatosYaEntregados.id_dato and datosEntregados.ciudadano = contandoDatosYaEntregados.ciudadano " +
"where datosEntregados.id_tipodato is null and datosEntregados.ciudadano is null";
                //string query = "select contandoDatosYaEntregados.id_dato, contandoDatosYaEntregados.ciudadano, contandoDatosYaEntregados.id_tramite_solicitado " +
                //"from (select ds.ciudadano, ds.id_tipodato from dato_solicitado ds) datosEntregados " +
                //"right join " +
                //"(select tipodatoinstitucion.id_dato, temp.ciudadano, temp.id_tramite_solicitado from " +
                //"(select rtd.id_dato, rtd.id_tramite, td.id_institucion from requisitotramitedato rtd " +
                //"inner join tipodato td on td.id_dato = rtd.id_dato) as tipodatoinstitucion " +
                //"inner join(select ts.id_tipo_tramite, ts.ciudadano, ts.id_tramite_solicitado from tramite_solicitado ts where ts.estado = 'PENDIENTE') as temp " +
                //"on tipodatoinstitucion.id_tramite = temp.id_tipo_tramite where tipodatoinstitucion.id_institucion = " + id_institucion + ") " +
                //"as contandoDatosYaEntregados " +
                //"on datosEntregados.id_tipodato = contandoDatosYaEntregados.id_dato and datosEntregados.ciudadano = contandoDatosYaEntregados.ciudadano " +
                //"where datosEntregados.id_tipodato is null and datosEntregados.ciudadano is null";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        Peticion peticion = new Peticion();
                        peticion.Id_tipo_dato = int.Parse(dataReader["id_dato"].ToString());
                        peticion.Id_tramite_solicitado = int.Parse(dataReader["id_tramite_solicitado"].ToString());
                        peticion.Cedula = dataReader["ciudadano"].ToString();
                        peticion.Nombre_dato = dataReader["nombre_dato"].ToString();
                        listPeticiones.Add(peticion);
                    }
                    dataReader.Close();
                    connection.Close();

                    return JsonConvert.SerializeObject(listPeticiones);
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

        public string EntregarDato(int id_tipo_dato, int id_tramite_solicitado, byte[] valor, string cedula)
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
                string query = "insert into dato_solicitado (id_tipodato, ciudadano, valor) values (@pid_tipo_dato,@pciudadano,@pvalor)";
                MySqlCommand cmd = new MySqlCommand();
                cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                cmd.Parameters.Add("@pid_tipo_dato", MySqlDbType.Int32).Value = id_tipo_dato;
                cmd.Parameters.Add("@pciudadano", MySqlDbType.VarChar).Value = cedula;
                cmd.Parameters.Add("@pvalor", MySqlDbType.VarBinary).Value = valor;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                return "1";
            }
            catch { return "-1"; }    
        }

        public string GetNombreCatalogoTipoDato(int id_tipo_dato)
        {
            MySqlConnection connection;
            string server = "localhost";
            string database = "pintae";
            string uid = "Rodolfo";
            string password = "1234qwer";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);

            string nombre_catalogo = "";

            try
            {

                connection.Open();

                string query = "select ctd.nombre_tipo from catalogo_tipo_dato ctd inner join tipodato td on ctd.id_catalogo_tipo_dato = td.id_catalogo_tipo_dato where td.id_dato = "+id_tipo_dato;
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        nombre_catalogo = dataReader["nombre_tipo"].ToString();
                    }
                    dataReader.Close();
                    connection.Close();

                    return nombre_catalogo;
                }
                else
                {
                    dataReader.Close();
                    connection.Close();
                    return "-2"; //No existe ese tipo dato.
                }
            }
            catch { return "-1"; } //Error de conexión.
        }

        public string CallProcEntregarDato(int id_tipo_dato, int id_tramite_solicitado, byte[] valor, string cedula)
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
                MySqlCommand cmd = new MySqlCommand("entregar_dato", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("p_id_dato", id_tipo_dato));
                cmd.Parameters.Add(new MySqlParameter("p_ciudadano", cedula));
                cmd.Parameters.Add(new MySqlParameter("p_valor", valor));
                cmd.Parameters.Add(new MySqlParameter("p_id_tramite_solicitado", id_tramite_solicitado));
                cmd.Connection.Open();
                int i = cmd.ExecuteNonQuery();
                cmd.Connection.Close();

                return ""+i;
            }
            catch { return "-1"; }
        }


    }
}
