using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Reflection;

namespace  Common
{
    
    /// 描 述：扩展.json序列反序列化
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 转成json对象
        /// </summary>
        /// <param name="Json">json字串</param>
        /// <returns></returns>
        public static object ToJson(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject(Json);
        }
       
        /// <summary>
        /// 转成json字串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="datetimeformats">时间格式化</param>
        /// <returns></returns>
        public static string ToJson(this object obj, string datetimeformats)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = datetimeformats };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }
        /// <summary>
        /// 字串反序列化成指定对象实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="Json">字串</param>
        /// <returns></returns>
        public static T ToObject<T>(this string Json)
        {
            return Json == null ? default(T) : JsonConvert.DeserializeObject<T>(Json);
        }
        /// <summary>
        /// 字串反序列化成指定对象实体(列表)
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="Json">字串</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<List<T>>(Json);
        }
        /// <summary>
        /// 字串反序列化成DataTable
        /// </summary>
        /// <param name="Json">字串</param>
        /// <returns></returns>
        public static DataTable ToTable(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<DataTable>(Json);
        }
        /// <summary>
        /// 字串反序列化成linq对象
        /// </summary>
        /// <param name="Json">字串</param>
        /// <returns></returns>
        public static JObject ToJObject(this string Json)
        {
            return Json == null ? JObject.Parse("{}") : JObject.Parse(Json.Replace("&nbsp;", ""));
        }
        public static JToken ReadJSON(this string Json)
        {
            JObject jobj = (Json == null ? JObject.Parse("{}") : JObject.Parse(Json.Replace("&nbsp;", "")));
            JToken result = jobj as JToken;
            return result;
        }
        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            if (obj == null) return map;
            Type t = obj.GetType(); // 获取对象对应的类， 对应的类型 
            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance); // 获取当前type公共属性 
            foreach (PropertyInfo p in pi)
            {
                MethodInfo m = p.GetGetMethod(); 
                if (m != null && m.IsPublic)
                {
                    // 进行判NULL处理 
                    if (m.Invoke(obj, new object[] { }) != null)
                    {
                        map.Add(p.Name, m.Invoke(obj, new object[] { })); // 向字典添加元素
                    }
                    else
                    {
                        map.Add(p.Name, null); // 向字典添加元素
                    }
                }
            }
            return map;
        }
        /// <summary>
        /// 转换成动态对象
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static dynamic ToDynamic(this string jsonString)
        {
            return JsonConvert.DeserializeObject<dynamic>(jsonString);
        }

        /// <summary>
        /// 将DataTable 转换成 List<dynamic>
        /// reverse 反转：控制返回结果中是只存在 FilterField 指定的字段,还是排除.
        /// [flase 返回FilterField 指定的字段]|[true 返回结果剔除 FilterField 指定的字段]
        /// FilterField  字段过滤，FilterField 为空 忽略 reverse 参数；返回DataTable中的全部数
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <param name="reverse">
        /// 反转：控制返回结果中是只存在 FilterField 指定的字段,还是排除.
        /// [flase 返回FilterField 指定的字段]|[true 返回结果剔除 FilterField 指定的字段]
        ///</param>
        /// <param name="FilterField">字段过滤，FilterField 为空 忽略 reverse 参数；返回DataTable中的全部数据</param>
        /// <returns>List<dynamic></returns>
        public static List<dynamic> ToDynamicList(this DataTable table, bool reverse = true, params string[] FilterField)
        {
            var modelList = new List<dynamic>();
            foreach (DataRow row in table.Rows)
            {
                dynamic model = new ExpandoObject();
                var dict = (IDictionary<string, object>)model;
                foreach (DataColumn column in table.Columns)
                {
                    if (FilterField.Length != 0)
                    {
                        if (reverse == true)
                        {
                            if (!((IList<string>)FilterField).Contains(column.ColumnName))
                            {
                                dict[column.ColumnName] = row[column];
                            }
                        }
                        else
                        {
                            if (((IList<string>)FilterField).Contains(column.ColumnName))
                            {
                                dict[column.ColumnName] = row[column];
                            }
                        }
                    }
                    else
                    {
                        dict[column.ColumnName] = row[column];
                    }
                }
                modelList.Add(model);
            }
            return modelList;
        }
    }
}
