using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace WR.Utils
{
    public class DtToListHelper
    {
        /// <summary>
        /// datatable数据转换为list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(DataTable dt) where T : class, new()
        {
            if (dt == null || dt.Rows.Count < 1)
                return new List<T>();

            //创建一个属性的列表
            List<PropertyInfo> prlist = new List<PropertyInfo>();

            //获取TResult的类型实例  反射的入口  
            Type t = typeof(T);

            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表  
            Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });

            //创建返回的集合
            List<T> oblist = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                //创建TResult的实例
                T ob = new T();

                //找到对应的数据  并赋值
                prlist.ForEach(p => { if (row[p.Name] != DBNull.Value) p.SetValue(ob, row[p.Name], null); });

                //放入到返回的集合中
                oblist.Add(ob);
            }

            return oblist;
        }

        /// <summary>
        /// datatable数据转换为list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(SqlDataReader dr) where T : class, new()
        {
            if (!dr.HasRows)
            {
                dr.Close();
                return new List<T>();
            }
            //创建值的列表
            List<T> resLst = new List<T>();

            //获取TResult的类型实例  反射的入口  
            Type t = typeof(T);
            PropertyInfo[] properties = t.GetProperties();

            while (dr.Read())
            {
                object ent = Activator.CreateInstance(t);

                foreach (PropertyInfo propertyInfo in properties)
                {
                    object value = dr[propertyInfo.Name];
                    if (value == null || value == DBNull.Value)
                        continue;

                    propertyInfo.SetValue(ent, value, null);
                }
                resLst.Add((T)ent);
            }
            dr.Close();

            return resLst;
        }

        /// <summary>
        /// 获取reader中的首行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T Get<T>(SqlDataReader dr) where T : class, new()
        {
            if (!dr.HasRows)
            {
                dr.Close();
                return default(T);
            }

            //获取TResult的类型实例  反射的入口  
            Type t = typeof(T);
            PropertyInfo[] properties = t.GetProperties();
            object ent = Activator.CreateInstance(t);

            if (dr.Read())
            {
                foreach (PropertyInfo propertyInfo in properties)
                {
                    object value = dr[propertyInfo.Name];
                    if (value == null || value == DBNull.Value)
                        continue;

                    propertyInfo.SetValue(ent, value, null);
                }
            }
            dr.Close();

            return (T)ent;
        }
    }
}
