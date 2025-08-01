﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SkySpeedService.ExecuteQuery
{
    internal class ExecuteQueries
    {
        protected bool ExecuteNonQuery(string query, string connectionString)
        {
            bool isQueryExecuted = false;
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                sqlCon.Open();
                cmd.ExecuteNonQuery();
                isQueryExecuted = true;
            }
            return isQueryExecuted;
        }

        protected bool ExecuteScalar(string query, string connectionString)
        {
            bool exist = false;
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                sqlCon.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    int value = (int)result;
                    exist = value > 0;
                }
            }
            return exist;
        }

        protected Dictionary<int, Dictionary<string, object>> ExecuteReaderQuery(string query, string connectionString)
        {
            Dictionary<int, Dictionary<string, object>> result = new Dictionary<int, Dictionary<string, object>>();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                sqlCon.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    for (int i = 1; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        object columnValue = reader.GetValue(i);
                        row.Add(columnName, columnValue);
                    }
                    result.Add((int)reader.GetValue(0), row); // Primary Key will be the the ID for all the tables
                }
            }
            return result;
        }
    }
}