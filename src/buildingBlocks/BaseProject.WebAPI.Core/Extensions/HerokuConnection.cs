using System;

namespace BaseProject.WebAPI.Core.Extensions
{
    public static class HerokuConnection
    {
        public static string GetHerokuConnection()
        {
            string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            string constr = null;
            if (!string.IsNullOrEmpty(connectionUrl))
            {
                var databaseUri = new Uri(connectionUrl);

                string db = databaseUri.LocalPath.TrimStart('/');
                string[] userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);

                constr = $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;";
            }
            return constr;
        }
    }
}
