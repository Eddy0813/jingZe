using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace JingZeServer.DataUtil
{
    class DbContextFactory
    {
        //todo:这里可以自己通过注入的方式来实现，就会更加灵活
        private static readonly RandomStrategy ReadDbStrategy = new RandomStrategy();
        public DbContext GetWriteDbContext()
        {
            string key = typeof(DbContextFactory).Name + "WriteDbContext";
            DbContext dbContext = CallContext.GetData(key) as DbContext;
            if (dbContext == null)
            {
                dbContext = new JZZSEntities1(ConfigurationManager.ConnectionStrings["JZZSEntities1"].ToString());

                CallContext.SetData(key, dbContext);
            }
            return dbContext;
        }

        public DbContext GetReadDbContext()
        {
            string key = typeof(DbContextFactory).Name + "ReadDbContext";
            DbContext dbContext = CallContext.GetData(key) as DbContext;
            if (dbContext == null)
            {
                dbContext = ReadDbStrategy.GetDbContext();
                CallContext.SetData(key, dbContext);
            }
            return dbContext;
        }

    }
}
