using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;

namespace template.Code.Helpers;

public class SQLHelperClass
{
    // CREATE PARAMETER
    public static DbParameter CreateParam(object source, string columnname)
    {
        source ??= DBNull.Value;

        string name = iif(columnname.StartsWith("@"), columnname, string.Concat("@", columnname)).ToString();
        SqlParameter SqlP = new SqlParameter(columnname, source) { SourceColumn = columnname.Remove(0, 1) };
        try
        {
            // Special code for types
            SqlP.SqlDbType = source.GetType().ToString().ToLower() switch
            {
                "system.data.datatable" =>
                    // For datatable to be used, SqlDBType must be structured
                    SqlDbType.Structured,
                _ => SqlP.SqlDbType
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine("CREATEPARAM ERROR: " + ex.Message);
        }

        return SqlP;
    }

    // CREATE PARAMETER with custom size
    public static DbParameter CreateParam(object source, string columnname, int size)
    {
        source ??= DBNull.Value;

        string name = iif(columnname.StartsWith("@"), columnname, string.Concat("@", columnname)).ToString();
        SqlParameter SqlP = new SqlParameter(columnname, source) { SourceColumn = columnname.Remove(0, 1) };
        SqlP.Size = size;
        try
        {
            // Special code for types
            SqlP.SqlDbType = source.GetType().ToString().ToLower() switch
            {
                "system.data.datatable" =>
                    // For datatable to be used, SqlDBType must be structured
                    SqlDbType.Structured,
                _ => SqlP.SqlDbType
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine("CREATEPARAM ERROR: " + ex.Message);
        }

        return SqlP;
    }

    // CREATE PARAMETER W. Custom Type Name
    public static DbParameter CreateParam(object source, string columnname, string TypeName)
    {
        source ??= DBNull.Value;

        string name = iif(columnname.StartsWith("@"), columnname, string.Concat("@", columnname));
        SqlParameter SqlP = new SqlParameter(columnname, source) { SourceColumn = columnname.Remove(0, 1) };

        // Special code for types
        SqlP.SqlDbType = source.GetType().ToString().ToLower() switch
        {
            "system.data.datatable" =>
                // For datatable to be used, SqlDBType must be structured
                SqlDbType.Structured,
            _ => SqlP.SqlDbType
        };

        if (TypeName != "") SqlP.TypeName = TypeName;
        return SqlP;
    }

    // RETURN PARAMTER
    public static DbParameter CreateReturnParam(object source, string columnname)
    {
        source ??= DBNull.Value;
        string name = iif(columnname.StartsWith("@"), columnname, string.Concat("@", columnname));
        SqlParameter SqlP = new SqlParameter(columnname, source) { SourceColumn = columnname.Remove(0, 1) };
        SqlP.Direction = ParameterDirection.Output;
        return SqlP;
    }

    public static DbParameter CreateReturnParam(object source, string columnname, int size)
    {
        source ??= DBNull.Value;
        string name = iif(columnname.StartsWith("@"), columnname, string.Concat("@", columnname));
        SqlParameter SqlP = new SqlParameter(columnname, source) { SourceColumn = columnname.Remove(0, 1) };
        SqlP.Size = size;
        SqlP.Direction = ParameterDirection.Output;
        return SqlP;
    }

    // C# IIF
    private static T iif<T>(bool expression, T truePart, T falsePart)
    {
        return expression ? truePart : falsePart;
    }
}