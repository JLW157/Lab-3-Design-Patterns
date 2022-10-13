using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2_v2.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool AddCustom(this Dictionary<int, List<ICourse>> targetDictionary, int key, ICourse course)
        {
            if (targetDictionary.ContainsKey(key) == false)
            {
                targetDictionary.Add(key, new List<ICourse>());
                targetDictionary[key].Add(course);
                return true;
            }
            else
            {
                if (targetDictionary[key].Contains(course))
                {
                    return false;
                }

                targetDictionary[key].Add(course);
                return true;
            }
        }

        public static bool AddCustom(this Dictionary<string, List<Student>> targetDictionary, string key, Student student)
        {
            if (targetDictionary.ContainsKey(key) == false)
            {
                targetDictionary.Add(key, new List<Student>());
                targetDictionary[key].Add(student);
                return true;
            }
            else
            {
                if (targetDictionary[key].Contains(student))
                {
                    return false;
                }

                targetDictionary[key].Add(student);
                return true;
            }
        }

        public static bool AddCustom(this Dictionary<string, IProfessor> targetDictionary, string key, IProfessor professor)
        {
            if (targetDictionary.ContainsKey(key) == false)
            {
                targetDictionary.Add(key, professor);
                return true;
            }
            return false;
        }

        public static bool RemoveCustom(this Dictionary<string, List<Student>> targetDictionary, string key, Student student)
        {
            if (targetDictionary.ContainsKey(key))
            {
                if (targetDictionary[key].Contains(student))
                {
                    targetDictionary[key].Remove(student);
                    return true;
                }
                return false;
            }
            return false;
        }

        public static bool RemoveCustom(this Dictionary<int, List<ICourse>> targetDictionary, int key, ICourse course)
        {
            if (targetDictionary.ContainsKey(key))
            {
                if (targetDictionary[key].Contains(course))
                {
                    targetDictionary[key].Remove(course);
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
