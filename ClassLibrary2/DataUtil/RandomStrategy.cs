using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JingZeServer.DataUtil
{
    class RandomStrategy
    {
        //所有读库类型
        public static List<string> DbTypes;

        static RandomStrategy()
        {
            LoadDbs();
        }

        //加载所有的读库类型
        static void LoadDbs()
        {
            DbTypes = new List<string>();

            foreach (ConnectionStringSettings Con in ConfigurationManager.ConnectionStrings)
            {
                if (Con.Name.ToString().StartsWith("SlaveConnectionString"))
                    DbTypes.Add(Con.ToString());
            }
        }

        public DbContext GetDbContext()
        {
            int randomIndex = new Random().Next(0, DbTypes.Count);
            var dbType = DbTypes[randomIndex];
            var dbContext = new JZZSEntities1(dbType);
            //var dbContext = new Model_Data();

            return dbContext;
            //return null;
        }
    }
}
