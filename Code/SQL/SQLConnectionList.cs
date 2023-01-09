// using System.Data;
// using static template.Code.DataTypeConversionHelperClass;
//
// namespace template.SQL
// {
//     /// <summary>
//     /// The Connection string array contains the following:
//     /// 0: IP Address
//     /// 1: Port
//     /// 2: Instance
//     /// 3: Database Name
//     /// 4: User
//     /// 5: Password
//     /// </summary>
//     public class SQLConnectionList
//     {
//
//         public static string[] MasterDBConn = new string[6]{
//           "mdb.template.dk",
//           "1434",
//           "template",
//           "MasterDB",
//           "sa",
//           "Moghaddam169#"};
//
//         //public static string GetConnectionString(string[] Conn)
//         //{
//         //    return string.Concat("Server=", Conn[0],
//         //        ";database=", Conn[1],
//         //        ";user id=", Conn[2],
//         //        ";password=", Conn[3]);
//         //}
//
//         public static string GetConnectionString(string[] ConnVariables)
//         {
//             string Address = ConnVariables[0];
//             string Port = ConnVariables[1];
//             string Instance = ConnVariables[2];
//             string SQLDB = ConnVariables[3];
//             string UserName = ConnVariables[4];
//             string Password = ConnVariables[5];
//             if (!string.IsNullOrEmpty(Instance)) Instance = $"\\{Instance}";
//             if (!string.IsNullOrEmpty(Port)) Port = $",{Port}";
//             //Build connection string
//             string SQLConnectionString = $"Data Source={Address}{Instance}{Port};Initial Catalog={SQLDB};User Id={UserName};Password={Password}";
//             return SQLConnectionString;
//         }
//
//         public static string[] GetConnForUser(string Email)
//         {
//             DataTable dtLoginData = SQLHelperClass.GetDataTable($"SELECT TOP 1 * FROM vWU_UserLogin WHERE Email='{Email}'", MasterDBConn);
//
//             if (dtLoginData.Rows.Count > 0)
//             {
//                 DataRow r = dtLoginData.Rows[0];
//                 string ServerName = cStr(r["ServerName"]);
//                 string CustomerID = cStr(r["CustomerID"]);
//                 string Port = cStr(r["PortNo"]);
//                 string Instance = cStr(r["Instance"]);
//                 string User = "sa";
//                 string Password = "Moghaddam169#";
//
//                 return new string[] { ServerName, Port, Instance, CustomerID, User, Password };
//             }
//             return null;
//         }
//
//        
//     }
// }
