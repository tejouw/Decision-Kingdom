using System.Collections.Generic;
using UnityEngine;

namespace DecisionKingdom.Utils
{
    /// <summary>
    /// Yardımcı extension metodları
    /// </summary>
    public static class Extensions
    {
        #region List Extensions
        /// <summary>
        /// Listeden rastgele bir eleman seç
        /// </summary>
        public static T GetRandom<T>(this IList<T> list)
        {
            if (list == null || list.Count == 0)
                return default;

            return list[Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Listeyi karıştır (Fisher-Yates shuffle)
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Liste boş mu kontrol et
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }
        #endregion

        #region String Extensions
        /// <summary>
        /// String boş mu kontrol et
        /// </summary>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// String'i kısalt
        /// </summary>
        public static string Truncate(this string str, int maxLength, string suffix = "...")
        {
            if (string.IsNullOrEmpty(str) || str.Length <= maxLength)
                return str;

            return str.Substring(0, maxLength - suffix.Length) + suffix;
        }
        #endregion

        #region Vector Extensions
        /// <summary>
        /// Vector2 ile Vector3 arasında dönüşüm
        /// </summary>
        public static Vector3 ToVector3(this Vector2 v, float z = 0)
        {
            return new Vector3(v.x, v.y, z);
        }

        /// <summary>
        /// Vector3'ü Vector2'ye çevir
        /// </summary>
        public static Vector2 ToVector2(this Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }

        /// <summary>
        /// Vector3'ün belirli bileşenini değiştir
        /// </summary>
        public static Vector3 WithX(this Vector3 v, float x)
        {
            return new Vector3(x, v.y, v.z);
        }

        public static Vector3 WithY(this Vector3 v, float y)
        {
            return new Vector3(v.x, y, v.z);
        }

        public static Vector3 WithZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }
        #endregion

        #region Color Extensions
        /// <summary>
        /// Rengin alpha değerini değiştir
        /// </summary>
        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }
        #endregion

        #region Transform Extensions
        /// <summary>
        /// Tüm çocukları yok et
        /// </summary>
        public static void DestroyChildren(this Transform transform)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(transform.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// Transform'u sıfırla
        /// </summary>
        public static void Reset(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
        #endregion

        #region GameObject Extensions
        /// <summary>
        /// Komponenti al veya ekle
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            T component = go.GetComponent<T>();
            if (component == null)
            {
                component = go.AddComponent<T>();
            }
            return component;
        }
        #endregion

        #region Float Extensions
        /// <summary>
        /// Değeri yeniden haritalandır
        /// </summary>
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        /// <summary>
        /// Değerin yaklaşık eşit olup olmadığını kontrol et
        /// </summary>
        public static bool Approximately(this float a, float b, float tolerance = 0.001f)
        {
            return Mathf.Abs(a - b) <= tolerance;
        }
        #endregion

        #region Int Extensions
        /// <summary>
        /// Değeri clamp et
        /// </summary>
        public static int Clamp(this int value, int min, int max)
        {
            return Mathf.Clamp(value, min, max);
        }
        #endregion
    }
}
