using Npgsql;
using System.Reflection;

namespace Dokotera.DataAccess
{
    public class ObjectDAO
    {
        private ConnectionManager _connectionManager;

        public ObjectDAO()
        {
            _connectionManager = new ConnectionManager("dokotera");
        }

        public void insertObject(Object obj)
        {
            string className = obj.GetType().Name;
            using (var conn = _connectionManager.OpenConnection())
            {
                var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                var cmd = new NpgsqlCommand($"INSERT INTO {className} ({string.Join(", ", properties.Select(p => p.Name))}) VALUES ({string.Join(", ", properties.Select(p => $"@{p.Name}"))})", conn);

                foreach (var property in properties)
                {
                    cmd.Parameters.AddWithValue($"@{property.Name}", property.GetValue(obj));
                }
                cmd.ExecuteNonQuery();
                conn.Close();
            }

        }

        public void updateObject(Object obj, string idEtat)
        {
            string className = obj.GetType().Name;
            using (var conn = _connectionManager.OpenConnection())
            {
                var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                var cmd = new NpgsqlCommand($"UPDATE {className} SET {string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"))} WHERE idEtat = '{idEtat}'", conn);

                foreach (var property in properties)
                {
                    cmd.Parameters.AddWithValue($"@{property.Name}", property.GetValue(obj));
                }
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void insertObjects(Object[] objs)
        {
            string className = objs[0].GetType().Name;
            using (var conn = _connectionManager.OpenConnection())
            {
                var properties = objs[0].GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                //var valueSets = objs.Select(o => $"({string.Join(", ", properties.Select(p => $"@{p.Name}"))})");
                //var values = string.Join(", ", valueSets);

                var cmd = new NpgsqlCommand($"INSERT INTO {className} ({string.Join(", ", properties.Select(p => p.Name))}) VALUES ({string.Join(", ", properties.Select(p => $"@{p.Name}"))})", conn);

                foreach (var item in objs)
                {
                    properties = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var property in properties)
                    {
                        cmd.Parameters.AddWithValue($"@{property.Name}", property.GetValue(item));
                    }
                    Console.WriteLine(cmd.CommandText);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                conn.Close();
            }
        }

        public void insertTableObject(Object obj)
        {
            string className = obj.GetType().Name;
            using (var conn = _connectionManager.OpenConnection())
            {
                var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                var cmd = new NpgsqlCommand($"INSERT INTO {className} ({string.Join(", ", properties.Select(p => p.Name))}) VALUES ({string.Join(", ", properties.Select(p => $"@{p.Name}"))})", conn);

                foreach (var property in properties)
                {
                    cmd.Parameters.AddWithValue($"@{property.Name}", property.GetValue(obj));
                }
                cmd.ExecuteNonQuery();
                _connectionManager.CloseConnection();
            }

        }

        public Object[] getObjects(Object obj)
        {
            string className = obj.GetType().Name;

            string query = $"SELECT * FROM {className}";
            var cmd = new NpgsqlCommand(query, _connectionManager.OpenConnection());

            using (var read = cmd.ExecuteReader())
            {
                List<Object> listObject = new List<Object>();
                while (read.Read())
                {
                    Object objet = Activator.CreateInstance(obj.GetType());
                    PropertyInfo[] properties = obj.GetType().GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        string propertyName = properties[i].Name;
                        Type propertyType = properties[i].PropertyType;
                        object value = read.GetValue(i);
                        if (value != DBNull.Value)
                        {
                            properties[i].SetValue(objet, Convert.ChangeType(value, propertyType));
                        }
                    }
                    listObject.Add(objet);
                }
                _connectionManager.CloseConnection();
                return listObject.ToArray();
            }
        }

        public Object[] getView(Object obj)
        {
            string className = obj.GetType().Name;

            string query = $"SELECT * FROM v_{className}";
            var cmd = new NpgsqlCommand(query, _connectionManager.OpenConnection());

            using (var read = cmd.ExecuteReader())
            {
                List<Object> listObject = new List<Object>();
                while (read.Read())
                {
                    Object objet = Activator.CreateInstance(obj.GetType());
                    PropertyInfo[] properties = obj.GetType().GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        string propertyName = properties[i].Name;
                        Type propertyType = properties[i].PropertyType;
                        object value = read.GetValue(i);
                        if (value != DBNull.Value)
                        {
                            properties[i].SetValue(objet, Convert.ChangeType(value, propertyType));
                        }
                    }
                    listObject.Add(objet);
                }
                _connectionManager.CloseConnection();
                return listObject.ToArray();
            }
        }

        public Object[] getValeurSymptome(Object obj, string id1, string id2)
        {
            string className = obj.GetType().Name;

            string query = $"SELECT valeurtraitemet FROM v_{className} where idmedicament = '{id1}' and idsymptomes = '{id2}'";
            var cmd = new NpgsqlCommand(query, _connectionManager.OpenConnection());

            using (var read = cmd.ExecuteReader())
            {
                List<Object> listObject = new List<Object>();
                while (read.Read())
                {
                    Object objet = Activator.CreateInstance(obj.GetType());
                    PropertyInfo[] properties = obj.GetType().GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        string propertyName = properties[i].Name;
                        Type propertyType = properties[i].PropertyType;
                        object value = read.GetValue(i);
                        if (value != DBNull.Value)
                        {
                            properties[i].SetValue(objet, Convert.ChangeType(value, propertyType));
                        }
                    }
                    listObject.Add(objet);
                }
                _connectionManager.CloseConnection();
                return listObject.ToArray();
            }
        }

        public Object[] getViewOrderBy(Object obj, string order, string valeur)
        {
            string className = obj.GetType().Name;

            string query = $"SELECT * FROM v_{className} order by {order} {valeur}";
            var cmd = new NpgsqlCommand(query, _connectionManager.OpenConnection());

            using (var read = cmd.ExecuteReader())
            {
                List<Object> listObject = new List<Object>();
                while (read.Read())
                {
                    Object objet = Activator.CreateInstance(obj.GetType());
                    PropertyInfo[] properties = obj.GetType().GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        string propertyName = properties[i].Name;
                        Type propertyType = properties[i].PropertyType;
                        object value = read.GetValue(i);
                        if (value != DBNull.Value)
                        {
                            properties[i].SetValue(objet, Convert.ChangeType(value, propertyType));
                        }
                    }
                    listObject.Add(objet);
                }
                _connectionManager.CloseConnection();
                return listObject.ToArray();
            }
        }

        public Object[] getViewBetweenOrderBy(Object obj, int age, string order, string valeur)
        {
            string className = obj.GetType().Name;

            string query = $"SELECT * FROM v_{className} where agemin <= {age} and agemax >= {age} order by {order} {valeur}";
            var cmd = new NpgsqlCommand(query, _connectionManager.OpenConnection());

            using (var read = cmd.ExecuteReader())
            {
                List<Object> listObject = new List<Object>();
                while (read.Read())
                {
                    Object objet = Activator.CreateInstance(obj.GetType());
                    PropertyInfo[] properties = obj.GetType().GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        string propertyName = properties[i].Name;
                        Type propertyType = properties[i].PropertyType;
                        object value = read.GetValue(i);
                        if (value != DBNull.Value)
                        {
                            properties[i].SetValue(objet, Convert.ChangeType(value, propertyType));
                        }
                    }
                    listObject.Add(objet);
                }
                _connectionManager.CloseConnection();
                return listObject.ToArray();
            }
        }

        public Object[] getViewOrdersBy(Object obj, string order1, string valeur1, string order2, string valeur2)
        {
            string className = obj.GetType().Name;

            string query = $"SELECT * FROM v_{className} order by {order1} {valeur1}, {order2} {valeur2}";
            var cmd = new NpgsqlCommand(query, _connectionManager.OpenConnection());

            using (var read = cmd.ExecuteReader())
            {
                List<Object> listObject = new List<Object>();
                while (read.Read())
                {
                    Object objet = Activator.CreateInstance(obj.GetType());
                    PropertyInfo[] properties = obj.GetType().GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        string propertyName = properties[i].Name;
                        Type propertyType = properties[i].PropertyType;
                        object value = read.GetValue(i);
                        if (value != DBNull.Value)
                        {
                            properties[i].SetValue(objet, Convert.ChangeType(value, propertyType));
                        }
                    }
                    listObject.Add(objet);
                }
                _connectionManager.CloseConnection();
                return listObject.ToArray();
            }
        }

        public Object[] getViewWithExternalWhereOrdersBy(Object obj, string where, string order1, string valeur1, string order2, string valeur2)
        {
            string className = obj.GetType().Name;

            string query = $"SELECT * FROM v_{className} where {where} order by {order1} {valeur1}, {order2} {valeur2}";
            var cmd = new NpgsqlCommand(query, _connectionManager.OpenConnection());

            using (var read = cmd.ExecuteReader())
            {
                List<Object> listObject = new List<Object>();
                while (read.Read())
                {
                    Object objet = Activator.CreateInstance(obj.GetType());
                    PropertyInfo[] properties = obj.GetType().GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        string propertyName = properties[i].Name;
                        Type propertyType = properties[i].PropertyType;
                        object value = read.GetValue(i);
                        if (value != DBNull.Value)
                        {
                            properties[i].SetValue(objet, Convert.ChangeType(value, propertyType));
                        }
                    }
                    listObject.Add(objet);
                }
                _connectionManager.CloseConnection();
                return listObject.ToArray();
            }
        }

        public int getViewSequence(string sequencename)
        {
            int sequence = 0;

            string query = $"SELECT * FROM v_{sequencename}";
            var cmd = new NpgsqlCommand(query, _connectionManager.OpenConnection());

            using (var read = cmd.ExecuteReader())
            {
                if (read.Read())
                {
                    sequence = read.GetInt32(0);
                }
            }
            return sequence;
        }

        public Object[] getViewWithConditionInf(Object obj, string references, string condition)
        {
            string className = obj.GetType().Name;

            string query = $"SELECT * FROM v_{className} WHERE {references} < '{condition}'";
            var cmd = new NpgsqlCommand(query, _connectionManager.OpenConnection());

            using (var read = cmd.ExecuteReader())
            {
                List<Object> listObject = new List<Object>();
                while (read.Read())
                {
                    Object objet = Activator.CreateInstance(obj.GetType());
                    PropertyInfo[] properties = obj.GetType().GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        string propertyName = properties[i].Name;
                        Type propertyType = properties[i].PropertyType;
                        object value = read.GetValue(i);
                        if (value != DBNull.Value)
                        {
                            properties[i].SetValue(objet, Convert.ChangeType(value, propertyType));
                        }
                    }
                    listObject.Add(objet);
                }
                _connectionManager.CloseConnection();
                return listObject.ToArray();
            }
        }

        public Object[] getViewWithCondition(Object obj, string references, string condition)
        {
            string className = obj.GetType().Name;

            string query = $"SELECT * FROM v_{className} WHERE {references} = '{condition}'";
            var cmd = new NpgsqlCommand(query, _connectionManager.OpenConnection());

            using (var read = cmd.ExecuteReader())
            {
                List<Object> listObject = new List<Object>();
                while (read.Read())
                {
                    Object objet = Activator.CreateInstance(obj.GetType());
                    PropertyInfo[] properties = obj.GetType().GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        string propertyName = properties[i].Name;
                        Type propertyType = properties[i].PropertyType;
                        object value = read.GetValue(i);
                        if (value != DBNull.Value)
                        {
                            properties[i].SetValue(objet, Convert.ChangeType(value, propertyType));
                        }
                    }
                    listObject.Add(objet);
                }
                _connectionManager.CloseConnection();
                return listObject.ToArray();
            }
        }

        public Object[] getViewWithConditionOrderBy(Object obj, string references, string condition, string order, string valeure)
        {
            string className = obj.GetType().Name;

            string query = $"SELECT * FROM v_{className} WHERE {references} = '{condition}' order by {order} {valeure}";
            var cmd = new NpgsqlCommand(query, _connectionManager.OpenConnection());

            using (var read = cmd.ExecuteReader())
            {
                List<Object> listObject = new List<Object>();
                while (read.Read())
                {
                    Object objet = Activator.CreateInstance(obj.GetType());
                    PropertyInfo[] properties = obj.GetType().GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        string propertyName = properties[i].Name;
                        Type propertyType = properties[i].PropertyType;
                        object value = read.GetValue(i);
                        if (value != DBNull.Value)
                        {
                            properties[i].SetValue(objet, Convert.ChangeType(value, propertyType));
                        }
                    }
                    listObject.Add(objet);
                }
                _connectionManager.CloseConnection();
                return listObject.ToArray();
            }
        }

        public Object[] getViewWithConditionsOrderBy(Object obj, string references, string condition, string references1, int intcondition, string order, string valeure, string order1, string valeure1)
        {
            string className = obj.GetType().Name;

            string query = $"SELECT * FROM v_{className} WHERE {references} = '{condition}' and {references1} < {intcondition} order by {order} {valeure}, {order1} {valeure1}";
            var cmd = new NpgsqlCommand(query, _connectionManager.OpenConnection());

            using (var read = cmd.ExecuteReader())
            {
                List<Object> listObject = new List<Object>();
                while (read.Read())
                {
                    Object objet = Activator.CreateInstance(obj.GetType());
                    PropertyInfo[] properties = obj.GetType().GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        string propertyName = properties[i].Name;
                        Type propertyType = properties[i].PropertyType;
                        object value = read.GetValue(i);
                        if (value != DBNull.Value)
                        {
                            properties[i].SetValue(objet, Convert.ChangeType(value, propertyType));
                        }
                    }
                    listObject.Add(objet);
                }
                _connectionManager.CloseConnection();
                return listObject.ToArray();
            }
        }

        public Object[] getViewWithConditions(Object obj, string references1, string condition1, string references2, int condition2, string references3, string condition3)
        {
            string className = obj.GetType().Name;

            string query = $"SELECT * FROM v_{className} WHERE {references1} = '{condition1}' and {references2} = {condition2} and {references3} < '{condition3}'";
            var cmd = new NpgsqlCommand(query, _connectionManager.OpenConnection());

            using (var read = cmd.ExecuteReader())
            {
                List<Object> listObject = new List<Object>();
                while (read.Read())
                {
                    Object objet = Activator.CreateInstance(obj.GetType());
                    PropertyInfo[] properties = obj.GetType().GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        string propertyName = properties[i].Name;
                        Type propertyType = properties[i].PropertyType;
                        object value = read.GetValue(i);
                        if (value != DBNull.Value)
                        {
                            properties[i].SetValue(objet, Convert.ChangeType(value, propertyType));
                        }
                    }
                    listObject.Add(objet);
                }
                _connectionManager.CloseConnection();
                return listObject.ToArray();
            }
        }

        public Object[] getObjectsByID(Object obj, string id)
        {
            string className = obj.GetType().Name;
            PropertyInfo[] props = obj.GetType().GetProperties();
            string references = props[0].Name;

            string query = $"SELECT * FROM {className} WHERE {references} = '{id}'";
            var cmd = new NpgsqlCommand(query, _connectionManager.OpenConnection());

            using (var read = cmd.ExecuteReader())
            {
                List<Object> listObject = new List<Object>();
                while (read.Read())
                {
                    Object objet = Activator.CreateInstance(obj.GetType());
                    PropertyInfo[] properties = obj.GetType().GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        string propertyName = properties[i].Name;
                        Type propertyType = properties[i].PropertyType;
                        object value = read.GetValue(i);
                        if (value != DBNull.Value)
                        {
                            properties[i].SetValue(objet, Convert.ChangeType(value, propertyType));
                        }
                    }
                    listObject.Add(objet);
                }
                _connectionManager.CloseConnection();
                return listObject.ToArray();
            }
        }

        public string getSequence(int length, string prefix, int sequence)
        {
            string seq = "";
            string query = $"SELECT * FROM getSequence({length},'{prefix}',{sequence})";
            var cmd = new NpgsqlCommand(query, _connectionManager.OpenConnection());
            using (var read = cmd.ExecuteReader())
            {
                if (read.Read())
                {
                    seq = read.GetString(0);
                }
            }
            return seq;
        }

        public Object[] getObjectsBy(Object obj, string references, string valeur)
        {
            string className = obj.GetType().Name;

            string query = $"SELECT * FROM {className} WHERE {references} = '{valeur}'";
            var cmd = new NpgsqlCommand(query, _connectionManager.OpenConnection());

            using (var read = cmd.ExecuteReader())
            {
                List<Object> listObject = new List<Object>();
                while (read.Read())
                {
                    Object objet = Activator.CreateInstance(obj.GetType());
                    PropertyInfo[] properties = obj.GetType().GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        string propertyName = properties[i].Name;
                        Type propertyType = properties[i].PropertyType;
                        object value = read.GetValue(i);
                        if (value != DBNull.Value)
                        {
                            properties[i].SetValue(objet, Convert.ChangeType(value, propertyType));
                        }
                    }
                    listObject.Add(objet);
                }
                _connectionManager.CloseConnection();
                return listObject.ToArray();
            }
        }
    }
}
